using HamstarHelpers.Helpers.DebugHelpers;
using OnARail.CustomEntities;
using Terraria.ModLoader;


namespace OnARail {
	partial class OnARailPlayer : ModPlayer {
		public void OnEnterWorldForSingle() {
			this.MyTrainId = TrainEntityHandler.CreateTrain( this.player.position );
		}

		public void OnEnterWorldForClient() {
		}

		public void OnEnterWorldForServer() {
			this.MyTrainId = TrainEntityHandler.CreateTrain( this.player.position );
		}
	}
}
