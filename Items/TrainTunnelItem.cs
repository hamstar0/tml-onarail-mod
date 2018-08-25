﻿using HamstarHelpers.Services.CustomHotkeys;
using HamstarHelpers.Services.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OnARail.Tiles;
using ReLogic.Graphics;
using Terraria;
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
			this.DisplayName.SetDefault( "Train Tunnel Kit" );
			this.Tooltip.SetDefault( "Creates a tunnel useable by trains on a wall" + '\n' +
				"Right-click to adjust direction of tunnel" );
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

		public override bool CanUseItem( Player player ) {
			int min_range = OnARailMod.Instance.Config.TrainTunnelMinRange;
			int range_span = OnARailMod.Instance.Config.TrainTunnelMaxRange - min_range;

			float rad = MathHelper.Pi / 180;
			float base_rads = TrainTunnelItem.GetRadiansOfDirection( this.Direction ) - ( rad * 5 );
			Vector2? dest = null;

			for( int i=0; i<100; i++ ) {
				float rads = base_rads + ( rad * (Main.rand.NextFloat() * 10f) );
				float dist = min_range + ( range_span * Main.rand.NextFloat() );

				dest = TrainTunnelItem.ScanForExitCandidate( player.position, rads, dist );
				if( dest != null ) { break; }
			}

			if( dest != null ) {
				Vector2 mydest = (Vector2)dest;
				TrainTunnelTileData.CreateTunnelEndpoint( (int)(mydest.X/16f), (int)(mydest.Y/16f) );
			}

			return dest != null;
		}


		////////////////

		public override void PostDrawInInventory( SpriteBatch sb, Vector2 pos, Rectangle frame, Color draw_color, Color item_color, Vector2 origin, float scale ) {
			float new_scale = 2.75f;
			Vector2 str_origin = Main.fontMouseText.MeasureString( ">" );
			Vector2 new_pos = new Vector2(
				( pos.X + ( scale * (float)frame.Width / 2f ) ),// - ( (new_scale * str_origin.X) / 2 ),
				( pos.Y + ( scale * (float)frame.Height / 2f ) )// - ( (new_scale * str_origin.Y) / 2 )
			);

			float rads = TrainTunnelItem.GetRadiansOfDirection( this.Direction );

			sb.DrawString( Main.fontMouseText, ">", new_pos, Color.Red, rads, str_origin / new_scale, new_scale * scale, SpriteEffects.None, 1f );
		}
	}
}
