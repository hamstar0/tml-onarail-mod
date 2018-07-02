using HamstarHelpers.DebugHelpers;
using Terraria;
using Terraria.ModLoader;


namespace OnARail {
	partial class OnARailPlayer : ModPlayer {
		public override bool CloneNewInstances { get { return false; } }

		public override void Initialize() {
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
			if( player.whoAmI != this.player.whoAmI ) { return; }

			if( Main.netMode == 0 ) {
				this.OnEnterWorldForSingle();
			} else if( Main.netMode == 1 ) {
				this.OnEnterWorldForClient();
			}
		}
	}
}
