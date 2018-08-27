using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.TileHelpers;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;


namespace OnARail.Tiles {
	public partial class TrainTunnelTileEntity : ModTileEntity {
		private static TrainTunnelTileEntity CreateTunnelEndpoint( TrainTunnelTileEntity start_tunnel_ent, int beg_tile_x, int beg_tile_y, int end_tile_x, int end_tile_y ) {
			var mymod = OnARailMod.Instance;
			
			if( !TileHelpers.PlaceTile( end_tile_x+2, end_tile_y+2, mymod.TileType<TrainTunnelTile>() ) ) {
				return null;
			}

			var base_ent = mymod.GetTileEntity<TrainTunnelTileEntity>();
			if( base_ent == null ) {
				throw new HamstarException( "Could not find base tile entity." );
			}

			int id = base_ent.Place( end_tile_x, end_tile_y );
			var exit_tunnel_ent = (TrainTunnelTileEntity)ModTileEntity.ByID[ id ];
			if( exit_tunnel_ent == null ) {
				throw new HamstarException( "Cannot create tunnel exit - No train tunnel entity associated with id " + id + " at x:" + end_tile_x + ", y:" + end_tile_y );
			}

			start_tunnel_ent.ExitTileX = end_tile_x;
			start_tunnel_ent.ExitTileY = end_tile_y;
			exit_tunnel_ent.ExitTileX = beg_tile_x;
			exit_tunnel_ent.ExitTileY = beg_tile_y;

			start_tunnel_ent.IsInitialized = true;
			exit_tunnel_ent.IsInitialized = true;

			return exit_tunnel_ent;
		}

		

		////////////////

		public override int Hook_AfterPlacement( int tile_x, int tile_y, int tile_type, int placement_style, int direction ) {
			if( Main.netMode == 1 ) {
				NetMessage.SendTileRange( Main.myPlayer, tile_x, tile_y, TrainTunnelTile.Width, TrainTunnelTile.Height );
				NetMessage.SendData( MessageID.TileEntityPlacement, -1, -1, null, tile_x, tile_y, this.Type, 0f, 0, 0, 0 );
				return -1;
			}
			
			int id = this.Place( tile_x, tile_y );

			var entry_tunnel_ent = (TrainTunnelTileEntity)ModTileEntity.ByID[ id ];
			if( entry_tunnel_ent == null ) {
				throw new HamstarException( "No train tunnel entity associated with id "+id+" at x:"+tile_x+", y:"+tile_y );
			}

			if( TrainTunnelTileEntity.ExitTunnelPosition == default( Point16 ) ) {
				throw new HamstarException( "No exit tunnel position available for tunnel "+id+" at x:"+tile_x+", y:"+tile_y );
			}
			
			TrainTunnelTileEntity.CreateTunnelEndpoint( entry_tunnel_ent, tile_x, tile_y, ExitTunnelPosition.X, ExitTunnelPosition.Y );

			return id;
		}
	}
}
