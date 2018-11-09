using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Components.CustomEntity.Components;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.PlayerHelpers;
using HamstarHelpers.Helpers.TmlHelpers;
using HamstarHelpers.Services.Promises;
using HamstarHelpers.Services.Timers;
using Microsoft.Xna.Framework;
using OnARail.Entities.Train;
using OnARail.Entities.Train.Components;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace OnARail {
	class PlayerPromiseArguments : PromiseArguments {
		public int Who;
	}




	partial class OnARailPlayer : ModPlayer {
		internal readonly static object MyValidatorKey;
		internal readonly static PromiseValidator PlayerFuncsValidator;


		////////////////

		static OnARailPlayer() {
			OnARailPlayer.MyValidatorKey = new object();
			OnARailPlayer.PlayerFuncsValidator = new PromiseValidator( OnARailPlayer.MyValidatorKey );
		}



		////////////////

		public bool IsInInitLockdown { get; private set; }
		public int MyTrainWho { get; private set; }
		
		private bool IsInitializedwithTrain = false;
		private Vector2 PrevPosition = default( Vector2 );



		////////////////

		public override bool CloneNewInstances { get { return false; } }

		public override void Initialize() {
			this.IsInInitLockdown = true;
			this.MyTrainWho = -1;
		}


		////////////////

		public override void Load( TagCompound tags ) {
			if( tags.ContainsKey("is_init") ) {
				this.IsInitializedwithTrain = tags.GetBool( "is_init" );
			}
		}

		public override TagCompound Save() {
			return new TagCompound { {"is_init", this.IsInitializedwithTrain} };
		}


		////////////////

		public override void SyncPlayer( int to_who, int from_who, bool new_player ) {
			if( Main.netMode == 2 ) {
				if( to_who == -1 && from_who == this.player.whoAmI ) {
					this.OnConnectServer();
				}
			}
		}

		public override void OnEnterWorld( Player player ) {
			if( player.whoAmI != Main.myPlayer ) { return; }
			if( this.player.whoAmI != Main.myPlayer ) { return; }

			if( Main.netMode == 0 ) {
				this.OnConnectSingle();
			} else if( Main.netMode == 1 ) {
				this.OnConnectClient();
			}
		}


		////////////////

		public override void OnRespawn( Player player ) {
			Promises.AddValidatedPromise( SaveableEntityComponent.LoadAllValidator, () => {
				Timers.SetTimer( "OnARailRespawn", 30, () => {
					if( this.MyTrainWho != -1 ) {
						TrainEntity.WarpPlayerToTrain( player );
					}
					return false;
				} );
				return false;
			} );
		}


		private void HandleRecall() {
			CustomEntity ent = CustomEntityManager.GetEntityByWho( this.MyTrainWho );
			var train_comp = ent.GetComponentByType<TrainBehaviorEntityComponent>();

			if( train_comp.IsMountedBy != -1 ) {
				PlayerHelpers.Teleport( this.player, this.PrevPosition );	// return to train's last position
			} else {
				if( ( (OnARailMod)this.mod ).Config.DebugModeInfo ) {
					Main.NewText( "Warping to train..." );
				}

				TrainEntity.WarpPlayerToTrain( player );	// return to train
			}
		}
	}
}
