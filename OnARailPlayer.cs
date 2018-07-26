using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.PlayerHelpers;
using HamstarHelpers.Services.Promises;
using Microsoft.Xna.Framework;
using OnARail.Entities;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace OnARail {
	class DecentralizedPlayerUpdates {
		internal static DecentralizedPlayerUpdates Instance = new DecentralizedPlayerUpdates();
		

		////////////////

		public Player MyPlayer;
	}



	partial class OnARailPlayer : ModPlayer {
		public int MyTrainID { get; private set; }
		
		private bool IsInitializedwithTrain = false;



		////////////////

		public override bool CloneNewInstances { get { return false; } }

		public override void Initialize() {
			this.MyTrainID = -1;
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

		private Vector2 PrevPosition = default( Vector2 );

		public override void PreUpdate() {
			if( this.player.dead ) { return; }

			if( Vector2.Distance( this.player.position, PlayerHelpers.GetSpawnPoint(this.player) ) <= 8 ) {
				if( Vector2.Distance( this.player.position, this.PrevPosition ) > 16 * 4 ) {
					TrainEntityHandler.WarpPlayerToTrain( player );
				}
			}

			this.PrevPosition = this.player.position;

			Promises.TriggerCustomPromiseForObject( DecentralizedPlayerUpdates.Instance );
		}


		////////////////

		internal void SpawnMyTrain() {
			this.MyTrainID = TrainEntityHandler.SpawnTrain( this.player );

			if( this.MyTrainID == -1 ) {
				LogHelpers.Log( "OnARail.OnARailPlayer.SpawnMyTrain - Could not spawn train for " + this.player.name );
				return;
			}
		}
	}
}
