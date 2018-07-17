using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Helpers.DebugHelpers;
using OnARail.CustomEntities;
using Terraria.ModLoader;


namespace OnARail {
	partial class OnARailPlayer : ModPlayer {
		/*public override void Load( TagCompound tag ) {
			var self = this;
			var mymod = (OnARailMod)this.mod;
		}

		public override TagCompound Save() {
			var tags = new TagCompound { };

			return tags;
		}*/

		
		////////////////
		
		public void OnEnterWorldForSingle() {
			TrainEntity.Spawn( this.player.position );
		}

		public void OnEnterWorldForClient() {
		}

		public void OnEnterWorldForServer() {
		}
	}
}
