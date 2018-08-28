using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.TileHelpers;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;


namespace OnARail.Tiles {
	public partial class TrainTunnelTileEntity : ModTileEntity {
		private static TrainTunnelTileEntity CreateTunnelEndpoint( TrainTunnelTileEntity start_tunnel_ent, int end_tile_x, int end_tile_y ) {
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
			exit_tunnel_ent.ExitTileX = start_tunnel_ent.Position.X;
			exit_tunnel_ent.ExitTileY = start_tunnel_ent.Position.Y;

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

			int id = this.Place( tile_x, tile_y ); ;

			if( Main.netMode == 0 && !Main.dedServ ) {
				var enter_ent = (TrainTunnelTileEntity)ModTileEntity.ByID[id];
				if( enter_ent == null ) {
					throw new HamstarException( "No train tunnel entity associated with id " + id + " at x:" + tile_x + ", y:" + tile_y );
				}

				enter_ent.OnPlace();
			}

			return id;
		}
		
		public override void OnNetPlace() {
			this.OnPlace();
		}


		////////////////

		private void OnPlace() {
			var mymod = OnARailMod.Instance;

			if( TrainTunnelTileEntity.ExitTunnelPosition == default(Point16) ) {
				throw new HamstarException( "No exit tunnel position available for tunnel "+this.ID+" at "+this.Position );
			}
			
			var exit_ent = TrainTunnelTileEntity.CreateTunnelEndpoint( this, ExitTunnelPosition.X, ExitTunnelPosition.Y );
			if( exit_ent == null ) {
				throw new HamstarException( "Could not create exit tunnel (at "+exit_ent.Position+") for tunnel " + this.ID + " at "+ this.Position );
			}

			if( mymod.Config.DebugModeInfo ) {
				LogHelpers.Log( "OnARail.Tiles.TrainTunnelTileEntity.OnNetPlace - Creating train tunnel from "+ this.Position+" to "+exit_ent.Position+"..." );
			}
		}
	}
}
