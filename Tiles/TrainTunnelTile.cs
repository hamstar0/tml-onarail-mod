using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ObjectData;


namespace OnARail.Tiles {
	class TrainTunnelTile : ModTile {
		public override void SetDefaults() {
			ModTranslation name = this.CreateMapEntryName();
			name.SetDefault( "Train Tunnel" );

			Main.tileFrameImportant[this.Type] = true;
			Main.tileLavaDeath[this.Type] = false;

			TileObjectData.newTile.Width = 4;
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.Origin = new Point16( 2, 2 );
			TileObjectData.newTile.AnchorWall = true;
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 32 };
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.StyleWrapLimit = 58; //36;

			TileObjectData.addTile( this.Type );

			this.dustType = 7;
			this.disableSmartCursor = true;
			this.AddMapEntry( new Color( 120, 85, 60 ), name );
		}
	}




	class TrainTunnelTileData : ModTileEntity {
		private int TunnelID = -1;
		

		////////////////

		public override bool ValidTile( int i, int j ) {
			Tile tile = Main.tile[i, j];
			return tile.active() && tile.type == mod.TileType<TrainTunnelTile>();
		}


		////////////////

		public override void Load( TagCompound tags ) {
			if( tags.ContainsKey("tunnel_id") ) {
				this.TunnelID = tags.GetInt( "tunnel_id" );
			}
		}
		public override TagCompound Save() {
			return new TagCompound { { "tunnel_id", this.TunnelID } };
		}

		public override void NetReceive( BinaryReader reader, bool light_receive ) {
			this.TunnelID = reader.ReadInt32();
		}
		public override void NetSend( BinaryWriter writer, bool light_send ) {
			writer.Write( (int)this.TunnelID );
		}


		////////////////

		public override int Hook_AfterPlacement( int i, int j, int type, int style, int direction ) {
			//Main.NewText("i " + i + " j " + j + " t " + type + " s " + style + " d " + direction);
			if( Main.netMode == 1 ) {
				NetMessage.SendTileSquare( Main.myPlayer, i, j, 3 );
				NetMessage.SendData( MessageID.TileEntityPlacement, -1, -1, null, i, j, this.Type, 0f, 0, 0, 0 );
				return -1;
			}
			return this.Place( i, j );
		}
	}
}
