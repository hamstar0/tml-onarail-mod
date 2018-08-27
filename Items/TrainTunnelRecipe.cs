using Terraria.ID;
using Terraria.ModLoader;


namespace OnARail.Items {
	class TrainTunnelRecipe : ModRecipe {
		public TrainTunnelRecipe( TrainTunnelItem myitem ) : base( myitem.mod ) {
			this.AddTile( TileID.TinkerersWorkbench );

			this.AddRecipeGroup( "HamstarHelpers:WarpPotions", 3 );
			this.AddIngredient( ItemID.Explosives, 1 );
			this.AddIngredient( ItemID.WoodenBeam, 25 );

			this.SetResult( myitem );
		}

		public override bool RecipeAvailable() {
			var mymod = (OnARailMod)this.mod;
			return mymod.Config.CraftableTrainTunnel;
		}
	}
}
