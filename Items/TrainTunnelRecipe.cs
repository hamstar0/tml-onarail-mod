using Terraria.ID;
using Terraria.ModLoader;


namespace OnARail.Items {
	class TrainTunnelRecipe : ModRecipe {
		public TrainTunnelRecipe( TrainTunnelItem myitem ) : base( myitem.mod ) {
			this.AddTile( TileID.TinkerersWorkbench );

			this.AddRecipeGroup( "HamstarHelpers:WarpPotions", 5 );
			this.AddIngredient( ItemID.Explosives, 1 );
			this.AddIngredient( ItemID.WoodenBeam, 50 );

			this.SetResult( myitem );
		}

		public override bool RecipeAvailable() {
			var mymod = (OnARailMod)this.mod;
			return mymod.Config.CraftableTrainTunnel;
		}
	}
}
