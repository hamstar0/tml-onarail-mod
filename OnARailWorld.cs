using Microsoft.Xna.Framework.Graphics;
using OnARail.Train;
using System;
using Terraria;
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


		////////////////

		public override void PostDrawTiles() {
			Player player = Main.player[Main.myPlayer];
			var myplayer = player.GetModPlayer<OnARailPlayer>( this.mod );
			var mymod = (OnARailMod)this.mod;

			//Main.spriteBatch.Begin();
			RasterizerState rasterizer = Main.gameMenu ||
				(double)Main.player[Main.myPlayer].gravDir == 1.0 ?
					RasterizerState.CullCounterClockwise :
					RasterizerState.CullClockwise;
			Main.spriteBatch.Begin( SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, rasterizer, (Effect)null, Main.GameViewMatrix.TransformationMatrix );

			try {
				this.TrainManager.DrawAll( Main.spriteBatch );
			} catch( Exception e ) {
				ErrorLogger.Log( "OnARailWorld.PostDrawTiles - " + e.ToString() );
				throw e;
			}

			Main.spriteBatch.End();
		}
	}
}
