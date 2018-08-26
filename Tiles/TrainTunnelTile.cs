using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.TileHelpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;


namespace OnARail.Tiles {
	public class TrainTunnelTile : ModTile {
		public const int Width = 4;
		public const int Height = 3;

		
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


		public override void KillMultiTile( int i, int j, int frameX, int frameY ) {
			this.mod.GetTileEntity<TrainTunnelTileEntity>().Kill( i, j );
		}
	}




	public partial class TrainTunnelTileEntity : ModTileEntity {
		private static int BaseTunnelID = 1;

		internal static Point16 ExitTunnelPosition = default( Point16 );
		


		////////////////

		private static bool CreateTunnelEndpoint( int tunnel_id, int tile_x, int tile_y ) {
			var mymod = OnARailMod.Instance;

			if( !TileHelpers.PlaceTile( tile_x, tile_y, mymod.TileType<TrainTunnelTile>() ) ) {
				return false;
			}

			var base_ent = mymod.GetTileEntity<TrainTunnelTileEntity>();
			if( base_ent == null ) {
				throw new HamstarException( "Could not find base tile entity." );
			}

			int id = base_ent.Place( tile_x-2, tile_y-2 );
			var ent = (TrainTunnelTileEntity)ModTileEntity.ByID[ id ];
			if( ent == null ) {
				throw new HamstarException( "Cannot create tunnel exit - No train tunnel entity associated with id " + id + " at x:" + tile_x + ", y:" + tile_y );
			}
			
			ent.TunnelID = tunnel_id;

			return true;
		}

		

		////////////////
		
		private int TunnelID = -1;



		////////////////

		public TrainTunnelTileEntity() : base() { }


		public override int Hook_AfterPlacement( int tile_x, int tile_y, int tile_type, int placement_style, int direction ) {
			if( Main.netMode == 1 ) {
				NetMessage.SendTileRange( Main.myPlayer, tile_x, tile_y, TrainTunnelTile.Width, TrainTunnelTile.Height );
				NetMessage.SendData( MessageID.TileEntityPlacement, -1, -1, null, tile_x, tile_y, this.Type, 0f, 0, 0, 0 );
				return -1;
			}

			int id = this.Place( tile_x, tile_y );
			var ent = (TrainTunnelTileEntity)ModTileEntity.ByID[ id ];
			if( ent == null ) {
				throw new HamstarException( "No train tunnel entity associated with id "+id+" at x:"+tile_x+", y:"+tile_y );
			}

			ent.InitializeMe( tile_x, tile_y );

			return id;
		}


		////////////////

		public void InitializeMe( int tile_x, int tile_y ) {
			if( TrainTunnelTileEntity.ExitTunnelPosition == default(Point16) ) {
				throw new HamstarException( "No exit tunnel position." );
			}

			this.TunnelID = TrainTunnelTileEntity.BaseTunnelID++;

			TrainTunnelTileEntity.CreateTunnelEndpoint( this.TunnelID, ExitTunnelPosition.X, ExitTunnelPosition.Y );
		}

		////////////////

		public override bool ValidTile( int i, int j ) {
			Tile tile = Main.tile[i, j];
			return tile.active() && tile.type == mod.TileType<TrainTunnelTile>();
		}

		////////////////


		/*public override void Load( TagCompound tags ) {
			if( tags.ContainsKey("tunnel_id") ) {
				this.TunnelID = tags.GetInt( "tunnel_id" );
			}
			if( this.TunnelID > TrainTunnelTileEntity.BaseTunnelID ) {
				TrainTunnelTileEntity.BaseTunnelID = this.TunnelID + 1;
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
		}*/


		////////////////

		public override void Update() {
Dust.NewDust( new Vector2(this.Position.X*16, this.Position.Y*16), 0, 0, 1 );
		}
	}
}
