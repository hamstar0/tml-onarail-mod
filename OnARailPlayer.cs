using HamstarHelpers.Helpers.DebugHelpers;
using OnARail.Mounts;
using Terraria;
using Terraria.ModLoader;


namespace OnARail {
	partial class OnARailPlayer : ModPlayer {
		private int MyTrainId;



		////////////////

		public override bool CloneNewInstances { get { return false; } }

		public override void Initialize() { }


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

		public void MountTrain() {
			int mount_type = this.mod.MountType<TrainMount>();

			this.player.mount.SetMount( mount_type, this.player );
		}
	}
}
