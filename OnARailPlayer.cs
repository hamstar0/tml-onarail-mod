using HamstarHelpers.Components.CustomEntity;
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
	internal class PlayerPromiseValidator : PromiseValidator {
		internal readonly static object MyValidatorKey;
		internal readonly static PlayerPromiseValidator RunAll;

		////////////////

		static PlayerPromiseValidator() {
			PlayerPromiseValidator.MyValidatorKey = new object();
			PlayerPromiseValidator.RunAll = new PlayerPromiseValidator();
		}

		////////////////

		public Player MyPlayer;

		////////////////

		private PlayerPromiseValidator() : base( PlayerPromiseValidator.MyValidatorKey ) { }
	}




	partial class OnARailPlayer : ModPlayer {
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
			if( Main.netMode == 0 ) {
				this.OnConnectSingle();
			} else if( Main.netMode == 1 ) {
				this.OnConnectClient();
			}
		}


		////////////////

		public override void PreUpdate() {
			if( this.player.dead ) { return; }

			if( this.MyTrainWho == -1 ) {
				this.MyTrainWho = TrainEntityHandler.FindMyTrain( this.player );
			}

			if( this.MyTrainWho != -1 && LoadHelpers.IsWorldSafelyBeingPlayed() ) {
				if( Vector2.Distance( this.player.position, PlayerHelpers.GetSpawnPoint( this.player ) ) <= 8 ) {	// at spawn
					if( Vector2.Distance( this.player.position, this.PrevPosition ) > 16 * 4 ) {	// 4+ blocks away from prev location
						if( Main.netMode == 0 ) {
							this.HandleRecall();
						} else {
							Timers.SetTimer( "OnARailPlayerEvac", 15, () => {
								this.HandleRecall();
								return false;
							} );
						}
					}
				}

				this.PrevPosition = this.player.position;

				PlayerPromiseValidator.RunAll.MyPlayer = this.player;
				Promises.TriggerValidatedPromise( PlayerPromiseValidator.RunAll, PlayerPromiseValidator.MyValidatorKey );
			} else {
				this.player.noItems = true;
				this.player.noBuilding = true;
				this.player.stoned = true;
				this.player.immune = true;
				this.player.immuneTime = 2;
			}
		}


		////////////////

		public override void OnRespawn( Player player ) {
			Timers.SetTimer( "OnARailRespawn", 30, () => {
				if( this.MyTrainWho != -1 ) {
					TrainEntityHandler.WarpPlayerToTrain( player );
				}
				return false;
			} );
		}


		////////////////

		internal void SpawnMyTrain() {
			this.MyTrainWho = TrainEntityHandler.SpawnMyTrain( this.player );

			if( this.MyTrainWho == -1 ) {
				LogHelpers.Log( "OnARail.OnARailPlayer.SpawnMyTrain - Could not spawn train for " + this.player.name );
				return;
			}
		}


		private void HandleRecall() {
			CustomEntity ent = CustomEntityManager.GetEntityByWho( this.MyTrainWho );
			var train_comp = ent.GetComponentByType<TrainBehaviorEntityComponent>();

			if( train_comp.IsMountedBy != -1 ) {
				PlayerHelpers.Teleport( this.player, this.PrevPosition );
			} else {
				if( ( (OnARailMod)this.mod ).Config.DebugModeInfo ) {
					Main.NewText( "Warping to train..." );
				}

				TrainEntityHandler.WarpPlayerToTrain( player );
			}
		}
	}
}
