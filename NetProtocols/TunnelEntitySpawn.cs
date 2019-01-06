using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using OnARail.Tiles;
using Terraria;
using Terraria.DataStructures;


namespace OnARail.NetProtocols {
	class TunnelEntitySpawnProtocol : PacketProtocolSendToServer {
		protected class MyFactory : Factory<TunnelEntitySpawnProtocol> {
			private readonly int BegTileX;
			private readonly int BegTileY;
			private readonly int EndTileX;
			private readonly int EndTileY;


			////////////////

			public MyFactory( int beg_tile_x, int beg_tile_y, int end_tile_x, int end_tile_y ) {
				this.BegTileX = beg_tile_x;
				this.BegTileY = beg_tile_y;
				this.EndTileX = end_tile_x;
				this.EndTileY = end_tile_y;
			}

			////

			protected override void Initialize( TunnelEntitySpawnProtocol data ) {
				data.BegTileX = this.BegTileX;
				data.BegTileY = this.BegTileY;
				data.EndTileX = this.EndTileX;
				data.EndTileY = this.EndTileY;
			}
		}



		////////////////

		public static void SendToServer( int beg_tile_x, int beg_tile_y, int end_tile_x, int end_tile_y ) {
			var factory = new MyFactory( beg_tile_x, beg_tile_y, end_tile_x, end_tile_y );
			TunnelEntitySpawnProtocol protocol = factory.Create();

			protocol.SendToServer( false );
		}


		////////////////

		public static void PlaceEntity( int tile_x, int tile_y ) {
			var mymod = OnARailMod.Instance;

			if( !WorldGen.InWorld( tile_x, tile_y, 0 ) ) {
				throw new HamstarException( "!OnARail.TunnelEntitySpawnProtocol.PlaceEntity - Tile entity spawn point ("
					+ tile_x + ", " + tile_y + ") blocked." );
			}
			if( TileEntity.ByPosition.ContainsKey( new Point16( tile_x, tile_y ) ) ) {
				throw new HamstarException( "!OnARail.TunnelEntitySpawnProtocol.PlaceEntity - Tile entity already spawned at "
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

		protected TunnelEntitySpawnProtocol( PacketProtocolDataConstructorLock ctor_lock ) : base( ctor_lock ) { }

		protected override void InitializeClientSendData() { }


		////////////////

		protected override void Receive( int from_who ) {
			TrainTunnelTileEntity.AwaitingExitTunnelPosition = new Point16( this.EndTileX, this.EndTileY );

			TunnelEntitySpawnProtocol.PlaceEntity( this.BegTileX, this.BegTileY );
		}
	}
}
