using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.PlayerHelpers;
using Microsoft.Xna.Framework;
using OnARail.CustomEntities;
using OnARail.Mounts;
using Terraria;
using Terraria.ModLoader;


namespace OnARail {
	partial class OnARailPlayer : ModPlayer {
		public int MyTrainId { get; private set; }



		////////////////

		public override bool CloneNewInstances { get { return false; } }

		public override void Initialize() {
			this.MyTrainId = -1;
		}


		////////////////

		public override void SyncPlayer( int to_who, int from_who, bool new_player ) {
			if( Main.netMode == 2 ) {
				if( to_who == -1 && from_who == this.player.whoAmI ) {
					this.OnEnterWorldForServer();
				}
			}
		}

		public override void OnEnterWorld( Player player ) {
			if( Main.netMode == 0 ) {
				this.OnEnterWorldForSingle();
			} else if( Main.netMode == 1 ) {
				this.OnEnterWorldForClient();
			}
		}


		////////////////

		private Vector2 PrevPosition = default( Vector2 );

		public override void PreUpdate() {
			if( this.player.dead ) { return; }

			if( Vector2.Distance( this.player.position, PlayerHelpers.GetSpawnPoint(this.player) ) <= 8 ) {
				if( Vector2.Distance( this.player.position, this.PrevPosition ) > 16 * 4 ) {
					TrainEntityHandler.WarpPlayer( player );
				}
			}

			this.PrevPosition = this.player.position;
		}


		////////////////

		//public override void OnRespawn( Player player ) {
		//	TrainEntityHandler.WarpPlayer( player );
		//}


		////////////////

		public void MountTrainMount() {
			int mount_type = this.mod.MountType<TrainMount>();

			this.player.mount.SetMount( mount_type, this.player );
		}
	}
}
