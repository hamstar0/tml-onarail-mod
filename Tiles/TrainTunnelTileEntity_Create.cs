using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.TileHelpers;
using Microsoft.Xna.Framework;
using OnARail.NetProtocols;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;


namespace OnARail.Tiles {
	public partial class TrainTunnelTileEntity : ModTileEntity {
		public override int Hook_AfterPlacement( int tile_x, int tile_y, int tile_type, int placement_style, int direction ) {
			if( Main.netMode == 1 ) {
				NetMessage.SendTileRange( Main.myPlayer, tile_x, tile_y, TrainTunnelTile.Width, TrainTunnelTile.Height );
				TunnelEntitySpawnProtocol.SendToServer( tile_x, tile_y,
					TrainTunnelTileEntity.AwaitingExitTunnelPosition.X, TrainTunnelTileEntity.AwaitingExitTunnelPosition.Y );
				//NetMessage.SendData( MessageID.TileEntityPlacement, -1, -1, null, tile_x, tile_y, this.Type, 0f, 0, 0, 0 );
			}

			if( Main.netMode == 0 ) {
				int id = this.Place( tile_x, tile_y );

				var enter_ent = (TrainTunnelTileEntity)ModTileEntity.ByID[id];
				if( enter_ent == null ) {
					throw new HamstarException( "!OnARail.TrainTunnelTileEntity.Hook_AfterPlacement - "
						+ "No train tunnel entity associated with id " + id + " at x:" + tile_x + ", y:" + tile_y );
				}

				enter_ent.OnPlace();

				return id;
			}

			return -1;
		}
		
		public override void OnNetPlace() {
			this.OnPlace();
		}
		

		////////////////

		private void OnPlace() {
			var mymod = OnARailMod.Instance;
			Point16 exit_pos = TrainTunnelTileEntity.AwaitingExitTunnelPosition;
			
			if( exit_pos == default(Point16) ) {
				throw new HamstarException( "No exit tunnel position available for tunnel "+this.ID+" at "+this.Position );
			}
			
			var exit_ent = TrainTunnelTileEntity.CreateTunnelEndpoint( this, exit_pos.X, exit_pos.Y );
			if( exit_ent == null ) {
				throw new HamstarException( "Could not create exit tunnel (at "+exit_pos+") for tunnel " + this.ID + " at "+ this.Position );
			}

			if( mymod.Config.DebugModeInfo ) {
				LogHelpers.Log( "OnARail.Tiles.TrainTunnelTileEntity.OnNetPlace - Creating train tunnel from "+ this.Position+" to "+exit_pos+"..." );
			}
		}


		
		////////////////

		private static TrainTunnelTileEntity CreateTunnelEndpoint( TrainTunnelTileEntity start_tunnel_ent, int end_tile_x, int end_tile_y ) {
			var mymod = OnARailMod.Instance;
			
			if( !TileHelpers.PlaceTile( end_tile_x + 2, end_tile_y + 2, mymod.TileType<TrainTunnelTile>() ) ) {
				start_tunnel_ent.IsInitialized = true;  // This destroys the tunnel
				Main.NewText( "Error placing tunnel exit.", Color.Red );
				return null;
			}

			var base_ent = mymod.GetTileEntity<TrainTunnelTileEntity>();
			if( base_ent == null ) {
				throw new HamstarException( "Could not find base tile entity." );
			}

			int id = base_ent.Place( end_tile_x, end_tile_y );
			var exit_tunnel_ent = (TrainTunnelTileEntity)ModTileEntity.ByID[id];
			if( exit_tunnel_ent == null ) {
				throw new HamstarException( "Cannot create tunnel exit - No train tunnel entity associated with id " + id + " at x:" + end_tile_x + ", y:" + end_tile_y );
			}

			start_tunnel_ent.ExitTileX = end_tile_x;
			start_tunnel_ent.ExitTileY = end_tile_y;
			exit_tunnel_ent.ExitTileX = start_tunnel_ent.Position.X;
			exit_tunnel_ent.ExitTileY = start_tunnel_ent.Position.Y;

			start_tunnel_ent.IsInitialized = true;
			exit_tunnel_ent.IsInitialized = true;
			
			return exit_tunnel_ent;
		}
	}
}
