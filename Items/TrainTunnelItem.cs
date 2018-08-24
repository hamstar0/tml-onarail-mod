using HamstarHelpers.Services.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using Terraria;
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
		
		public override bool CanRightClick() {
			if( Timers.GetTimerTickDuration("TrainTunnelRotate") > 0 ) {
				return false;
			}
			Timers.SetTimer( "TrainTunnelRotate", 15, () => false );

			this.Direction = (this.Direction + 1) % 6;

			return false;
		}

		public override void OnConsumeItem( Player player ) {
			Main.NewText( "eat me" );
		}


		////////////////

		public override void PostDrawInInventory( SpriteBatch sb, Vector2 pos, Rectangle frame, Color draw_color, Color item_color, Vector2 origin, float scale ) {
			float rads;
			float new_scale = 3.25f * scale;
			Vector2 str_origin = Main.fontMouseText.MeasureString( ">" );
			Vector2 new_pos = new Vector2(
				( pos.X + ( scale * (float)frame.Width / 2f ) ),// - ( (new_scale * str_origin.X) / 2 ),
				( pos.Y + ( scale * (float)frame.Height / 2f ) )// - ( (new_scale * str_origin.Y) / 2 )
			);

			switch( this.Direction ) {
			case 0:
				rads = 0;
				break;
			case 1:
				rads = (MathHelper.Pi / 180f) * 45;
				break;
			case 2:
				rads = ( MathHelper.Pi / 180f ) * 135;
				break;
			case 3:
				rads = MathHelper.Pi;
				break;
			case 4:
				rads = ( MathHelper.Pi / 180f ) * 225;
				break;
			case 5:
				rads = ( MathHelper.Pi / 180f ) * 315;
				break;
			default:
				throw new Exception( "Invalid direction." );
			}

			sb.DrawString( Main.fontMouseText, ">", new_pos, Color.Red, rads, str_origin / new_scale, new_scale, SpriteEffects.None, 1f );
		}
	}
}
