using Terraria.ModLoader;


namespace OnARail.Items {
	class TrainTunnelItem : ModItem {
		public override void SetDefaults() {
			this.item.width = 30;
			this.item.height = 30;
			this.item.maxStack = 1;
			this.item.useTurn = true;
			this.item.autoReuse = true;
			this.item.useAnimation = 15;
			this.item.useTime = 10;
			this.item.useStyle = 1;
			this.item.consumable = true;
			this.item.value = 100000;
			this.item.rare = 1;
			this.item.createTile = mod.TileType( "TrainTunnelTile" );
			//this.item.placeStyle = 1;
		}
	}
}
