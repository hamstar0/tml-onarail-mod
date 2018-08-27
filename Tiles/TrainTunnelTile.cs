using HamstarHelpers.Helpers.DebugHelpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;


namespace OnARail.Tiles {
	public class TrainTunnelTile : ModTile {
		public const int Width = 4;
		public const int Height = 4;

		
		////////////////

		public override void SetDefaults() {
			ModTranslation name = this.CreateMapEntryName();
			name.SetDefault( "Train Tunnel" );

			Main.tileFrameImportant[this.Type] = true;
			Main.tileLavaDeath[this.Type] = false;
			
			var placement_hook = new PlacementHook( 
				this.mod.GetTileEntity<TrainTunnelTileEntity>().Hook_AfterPlacement,
				-1, 0, true
			);

			TileObjectData.newTile.HookPostPlaceMyPlayer = placement_hook;
			TileObjectData.newTile.Width = TrainTunnelTile.Width;
			TileObjectData.newTile.Height = TrainTunnelTile.Height;
			TileObjectData.newTile.Origin = new Point16( 2, 2 );
			TileObjectData.newTile.AnchorWall = true;
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16, 32 };
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.StyleWrapLimit = 58; //36;

			TileObjectData.addTile( this.Type );

			this.dustType = 7;
			this.disableSmartCursor = true;
			this.AddMapEntry( new Color( 96, 85, 60 ), name );
		}


		public override void KillMultiTile( int i, int j, int frameX, int frameY ) {
			this.mod.GetTileEntity<TrainTunnelTileEntity>().Kill( i, j );
		}
	}
}
