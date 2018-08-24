using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace OnARail.Items {
	class TrainTunnelItem : ModItem {
		private int Direction = 0;


		////////////////

		public override bool CloneNewInstances => true;

		public override ModItem Clone() {
			var clone = (TrainTunnelItem)base.Clone();
			clone.Direction = this.Direction;
			return clone;
		}


		////////////////
		
		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "Train Tunnel Kit" );
			this.Tooltip.SetDefault( "Creates a tunnel useable by trains on a wall" + '\n' +
				"Right-click to adjust direction of tunnel" );
		}

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
			this.item.value = Item.buyPrice( 0, 10, 0, 0 );
			this.item.rare = 1;
			this.item.createTile = mod.TileType( "TrainTunnelTile" );
			//this.item.placeStyle = 1;
		}

		////////////////

		public override void AddRecipes() {
			var recipe = new TrainTunnelRecipe( this );
			recipe.AddRecipe();
		}


		////////////////

		public override void OnConsumeItem( Player player ) {
			Main.NewText( "eat me" );
		}
	}



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
