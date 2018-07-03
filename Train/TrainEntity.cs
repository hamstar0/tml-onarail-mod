using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Services.Promises;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;


namespace OnARail.Train {
	class TrainEntity : CustomEntity {
		private static Texture2D Tex;
		public static int FrameCount { get; private set; }
		public static int Width { get; private set; }
		public static int Height { get; private set; }


		////////////////

		static TrainEntity() {
			TrainEntity.FrameCount = 3;
			TrainEntity.Width = 1;
			TrainEntity.Height = 1;

			if( Main.netMode != 2 ) {
				Promises.AddPostModLoadPromise( () => {
					TrainEntity.Width = TrainEntity.Tex.Width;
					TrainEntity.Height = TrainEntity.Tex.Height / TrainEntity.FrameCount;
					TrainEntity.Tex = OnARailMod.Instance.GetTexture( "Mounts/TrainMount_Back" );
				} );
			}
		}



		////////////////

		public Vector2 Pos;
		public Rectangle Rect { get; private set; }



		////////////////

		public TrainEntity( Vector2 pos, Color color ) {
			pos.X = MathHelper.Clamp( pos.X, 160, ( Main.maxTilesX - 10 ) * 16 );
			pos.Y = MathHelper.Clamp( pos.Y, 160, ( Main.maxTilesY - 10 ) * 16 );
			//DebugHelpers.Log( "wall of "+color.ToString()+": "+Main.tile[(int)(pos.X/16f)+2, (int)(pos.Y/16f)+4].wall );

			this.Pos = pos;

			// Clients and single only
			if( Main.netMode != 2 ) {
				this.Rect = new Rectangle( (int)pos.X, (int)pos.Y, TrainEntity.Width, TrainEntity.Height );
			}
		}
		
		public void ChangePosition( Vector2 pos ) {
			this.Pos = pos;
			this.Rect = new Rectangle( (int)pos.X, (int)pos.Y, this.Rect.Width, this.Rect.Height );
		}


		////////////////

		public void Draw( SpriteBatch sb ) {
			if( Main.netMode == 2 ) { return; }
			
			var world_scr_rect = new Rectangle( (int)Main.screenPosition.X, (int)Main.screenPosition.Y, Main.screenWidth, Main.screenHeight );
			if( !this.Rect.Intersects( world_scr_rect ) ) { return; }

			Vector2 scr_scr_pos = this.Pos - Main.screenPosition;
			Rectangle scr_rect = this.Rect;
			scr_rect.X -= world_scr_rect.X;
			scr_rect.Y -= world_scr_rect.Y;

			float scale = 1f;

			sb.Draw( TrainEntity.Tex, scr_scr_pos, scr_rect, Color.White, 0f, new Vector2(), scale, SpriteEffects.None, 1f );

			Dust.NewDust( this.Pos, this.Rect.Width, this.Rect.Height, 15, 0, 0, 150, Color.White, 1f );
		}
	}
}
