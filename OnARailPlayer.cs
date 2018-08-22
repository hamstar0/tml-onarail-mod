using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Components.CustomEntity.Components;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.PlayerHelpers;
using HamstarHelpers.Helpers.TmlHelpers;
using HamstarHelpers.Services.Promises;
using HamstarHelpers.Services.Timers;
using Microsoft.Xna.Framework;
using OnARail.Entities;
using OnARail.Entities.Components;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace OnARail {
	class PlayerPromiseArguments : PromiseArguments {
		public int Who;
	}




	partial class OnARailPlayer : ModPlayer {
		internal readonly static object MyValidatorKey;
		internal readonly static PromiseValidator RunAllValidator;


		////////////////

		static OnARailPlayer() {
			OnARailPlayer.MyValidatorKey = new object();
			OnARailPlayer.RunAllValidator = new PromiseValidator( OnARailPlayer.MyValidatorKey );
		}



		////////////////

		public bool IsInInitLockdown { get; private set; }
		public int MyTrainWho { get; private set; }
		
		private bool IsInitializedwithTrain = false;
		private Vector2 PrevPosition = default( Vector2 );



		////////////////

		public override bool CloneNewInstances { get { return false; } }

		public override void Initialize() {
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

		public override void PreUpdate() {
			if( this.MyTrainWho == -1 ) {
				this.MyTrainWho = TrainEntityHandler.FindMyTrain( this.player );
			}
			
			if( !this.player.dead ) {
				if( this.MyTrainWho != -1 && LoadHelpers.IsWorldSafelyBeingPlayed() ) {
					if( Main.netMode != 2 ) {
						if( Vector2.Distance( this.player.position, PlayerHelpers.GetSpawnPoint( this.player ) ) <= 8 ) {   // is at spawn
							if( Vector2.Distance( this.player.position, this.PrevPosition ) > 16 * 4 ) {    // is 4+ blocks away since prev tick
								this.HandleRecall();
							}
						}

						this.PrevPosition = this.player.position;
					}
					this.IsInInitLockdown = false;
				} else {
					this.IsInInitLockdown = true;
					PlayerHelpers.LockdownPlayerPerTick( this.player );
				}

				var args = new PlayerPromiseArguments { Who = this.player.whoAmI };

				Promises.TriggerValidatedPromise( OnARailPlayer.RunAllValidator, OnARailPlayer.MyValidatorKey, args );
			}
		}


		////////////////

		public override void OnRespawn( Player player ) {
			Promises.AddValidatedPromise( SaveableEntityComponent.LoadAllValidator, () => {
				Timers.SetTimer( "OnARailRespawn", 30, () => {
					if( this.MyTrainWho != -1 ) {
						TrainEntityHandler.WarpPlayerToTrain( player );
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

				TrainEntityHandler.WarpPlayerToTrain( player );	// return to train
			}
		}
	}
}
