using OnARail.Train;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace OnARail {
	class OnARailWorld : ModWorld {
		private TrainManager TrainManager;


		////////////////

		public override void Initialize() {
			this.TrainManager = new TrainManager();
		}

		public override void Load( TagCompound tag ) {
			this.TrainManager.Load( tag );
		}

		public override TagCompound Save() {
			var tags = new TagCompound();
			this.TrainManager.Save( tags );
			return tags;
		}
	}
}
