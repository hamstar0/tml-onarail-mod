using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using OnARail.Tiles;
using Terraria;
using Terraria.DataStructures;


namespace OnARail.NetProtocols {
	class TunnelEntitySpawnProtocol : PacketProtocol {
		public static void SendToServer( int beg_tile_x, int beg_tile_y, int end_tile_x, int end_tile_y ) {
			var protocol = new TunnelEntitySpawnProtocol( beg_tile_x, beg_tile_y, end_tile_x, end_tile_y );
			protocol.SendToServer( false );
		}


		////////////////

		public static void PlaceEntity( int tile_x, int tile_y ) {
			var mymod = OnARailMod.Instance;

			if( !WorldGen.InWorld( tile_x, tile_y, 0 ) ) {
				throw new HamstarException( "!OnARail.TunnelEntitySpawnProtocol.ReceiveWithClient - Tile entity spawn point ("
					+ tile_x + ", " + tile_y + ") blocked." );
			}
			if( TileEntity.ByPosition.ContainsKey( new Point16( tile_x, tile_y ) ) ) {
				throw new HamstarException( "!OnARail.TunnelEntitySpawnProtocol.ReceiveWithClient - Tile entity already spawned at "
					+ tile_x + ", " + tile_y );
			}

			TileEntity.PlaceEntityNet( tile_x, tile_y, mymod.TileEntityType<TrainTunnelTileEntity>() );
		}


		////////////////

		public int BegTileX;
		public int BegTileY;
		public int EndTileX;
		public int EndTileY;


		////////////////

		private TunnelEntitySpawnProtocol( PacketProtocolDataConstructorLock ctor_lock ) { }

		private TunnelEntitySpawnProtocol( int beg_tile_x, int beg_tile_y, int end_tile_x, int end_tile_y ) {
			this.BegTileX = beg_tile_x;
			this.BegTileY = beg_tile_y;
			this.EndTileX = end_tile_x;
			this.EndTileY = end_tile_y;
		}


		////////////////

		protected override void ReceiveWithServer( int from_who ) {
			TrainTunnelTileEntity.AwaitingExitTunnelPosition = new Point16( this.EndTileX, this.EndTileY );

			TunnelEntitySpawnProtocol.PlaceEntity( this.BegTileX, this.BegTileY );
		}
	}
}
