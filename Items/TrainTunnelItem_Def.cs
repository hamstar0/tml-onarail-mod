using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Services.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OnARail.Tiles;
using ReLogic.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;


namespace OnARail.Items {
	partial class TrainTunnelItem : ModItem {
		private int Direction = 0;


		////////////////

		public override bool CloneNewInstances => true;

		public override ModItem Clone() {
			var clone = (TrainTunnelItem)base.Clone();
			clone.Direction = this.Direction;
			return clone;
		}


		////////////////
		
//private static float TESTSCALE=0f;
		public override void SetStaticDefaults() {
			var mymod = (OnARailMod)this.mod;

			this.DisplayName.SetDefault( "Train Tunnel Kit" );
			this.Tooltip.SetDefault( "Creates a wall-tunnel useable by trains" + '\n' +
				"Right-click item to adjust direction of tunnel" +
				"Tunnels can reach between "+mymod.Config.TrainTunnelMinTileRange+" and "+mymod.Config.TrainTunnelMaxTileRange+" blocks" );
//CustomHotkeys.BindActionToKey1("blah", () => {
//	TESTSCALE -= 0.25f;
//	Main.NewText( "-TESTSCALE: " + TESTSCALE );
//} );
//CustomHotkeys.BindActionToKey2("blah", () => {
//	TESTSCALE += 0.25f;
//	Main.NewText( "+TESTSCALE: " + TESTSCALE );
//} );
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
			this.item.createTile = mod.TileType<TrainTunnelTile>();
			//this.item.placeStyle = 1;
		}

		////////////////

		public override void AddRecipes() {
			var recipe = new TrainTunnelRecipe( this );
			recipe.AddRecipe();
		}


		////////////////
		
		public override bool CanRightClick() {
			if( Timers.GetTimerTickDuration("TrainTunnelRotate") > 0 ) {
				return false;
			}
			Timers.SetTimer( "TrainTunnelRotate", 15, () => false );

			this.Direction = (this.Direction + 1) % 6;

			return false;
		}

		////////////////

		public override bool CanUseItem( Player player ) {
			if( Timers.GetTimerTickDuration( "TrainTunnelPlaceError" ) > 0 ) {
				Timers.SetTimer( "TrainTunnelPlaceError", 15, () => false );
				return false;
			}
			Timers.SetTimer( "TrainTunnelPlaceError", 15, () => false );

			int min_range = OnARailMod.Instance.Config.TrainTunnelMinTileRange;
			int range_span = OnARailMod.Instance.Config.TrainTunnelMaxTileRange - min_range;

			float rad = MathHelper.Pi / 180f;
			float base_rads = TrainTunnelItem.GetRadiansOfDirection( this.Direction ) - ( rad * 5f );
			Vector2? _dest = null;

			for( int i=0; i<100; i++ ) {
				float rads = base_rads + ( rad * (Main.rand.NextFloat() * 10f) );
				float dist = min_range + ( range_span * Main.rand.NextFloat() );

				_dest = TrainTunnelItem.ScanForExitCandidate( player.position, rads, dist );
				if( _dest != null ) { break; }
			}

			if( _dest != null ) {
				var dest = (Vector2)_dest;
				TrainTunnelTileEntity.AwaitingExitTunnelPosition = new Point16( (int)(dest.X / 16f), (int)(dest.Y / 16f) );
			} else {
				Main.NewText( "Could not find suitable tunnel exit. Try again, or change tunnel direction.", Color.Red );
			}
			
			return _dest != null;	// Tile hasn't been placed yet; Terraria could still decide it will fail
		}

		public override bool ConsumeItem( Player player ) { // ConsumeItem runs only if a valid start tunnel location found (not just our exit)
			if( TrainTunnelTileEntity.AwaitingExitTunnelPosition == default(Point16) ) {
				throw new HamstarException( "No candidate exit tunnel location set." );
			}

			var mymod = (OnARailMod)this.mod;
			
			return !mymod.Config.DebugModeTunnelTester;
		}


		////////////////

		public override void PostDrawInInventory( SpriteBatch sb, Vector2 pos, Rectangle frame, Color draw_color, Color item_color, Vector2 origin, float scale ) {
			float new_scale = 2.75f;
			Vector2 str_origin = Main.fontMouseText.MeasureString( ">" );
			Vector2 new_pos = new Vector2(
				( pos.X + ( scale * (float)frame.Width / 2f ) ),// - ( (new_scale * str_origin.X) / 2 ),
				( pos.Y + ( scale * (float)frame.Height / 2f ) )// - ( (new_scale * str_origin.Y) / 2 )
			);

			float rads = -TrainTunnelItem.GetRadiansOfDirection( this.Direction );

			sb.DrawString( Main.fontMouseText, ">", new_pos, Color.Red, rads, str_origin / new_scale, new_scale * scale, SpriteEffects.None, 1f );
		}
	}
}
