using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;


namespace OnARail.Tiles {
	public class TrainTunnelTile : ModTile {
		public override void SetDefaults() {
			ModTranslation name = this.CreateMapEntryName();
			name.SetDefault( "Train Tunnel" );

			Main.tileFrameImportant[ this.Type ] = true;
			Main.tileLavaDeath[ this.Type ] = false;

			TileObjectData.newTile.Width = 4;
			TileObjectData.newTile.Height = 4;
			TileObjectData.newTile.Origin = new Point16( 2, 2 );
			TileObjectData.newTile.AnchorWall = true;
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16, 16 };
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.StyleWrapLimit = 58;	//36;

			TileObjectData.addTile( this.Type );

			this.dustType = 7;
			this.disableSmartCursor = true;
			this.AddMapEntry( new Color( 120, 85, 60 ), name );
		}
	}
}
