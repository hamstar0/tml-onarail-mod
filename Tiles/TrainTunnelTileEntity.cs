using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using Microsoft.Xna.Framework;
using OnARail.Entities.Train.Components;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace OnARail.Tiles {
	public partial class TrainTunnelTileEntity : ModTileEntity {
		internal static Point16 AwaitingExitTunnelPosition = default( Point16 );



		////////////////

		public int ExitTileX { get; private set; }
		public int ExitTileY { get; private set; }

		private bool IsInitialized = false;



		////////////////

		public TrainTunnelTileEntity() : base() {
			this.ExitTileX = -1;
			this.ExitTileY = -1;
		}


		////////////////

		public override bool ValidTile( int i, int j ) {
			Tile tile = Main.tile[i, j];
			return tile.active() && tile.type == mod.TileType<TrainTunnelTile>();
		}

		////////////////


		public override void Load( TagCompound tags ) {
			if( tags.ContainsKey("exit_x") ) {
				this.ExitTileX = tags.GetInt( "exit_x" );
				this.ExitTileY = tags.GetInt( "exit_y" );
			}
			this.IsInitialized = true;
		}

		public override TagCompound Save() {
			return new TagCompound {
				{ "exit_x", this.ExitTileX },
				{ "exit_y", this.ExitTileY }
			};
		}


		public override void NetReceive( BinaryReader reader, bool light_receive ) {
			this.ExitTileX = reader.ReadInt32();
			this.ExitTileY = reader.ReadInt32();
			this.IsInitialized = true;
		}

		public override void NetSend( BinaryWriter writer, bool light_send ) {
			writer.Write( (int)this.ExitTileX );
			writer.Write( (int)this.ExitTileY );
		}


		////////////////

		public override void Update() {
			if( Main.netMode == 1 ) { throw new HamstarException( "Never run on client." ); }

			int exit_id = this.Find( this.ExitTileX, this.ExitTileY );

			// Clear unmatched tunnel entities
			if( exit_id == -1 ) {
				if( this.IsInitialized ) {
					WorldGen.KillTile( this.Position.X, this.Position.Y );
					this.Kill( this.Position.X, this.Position.Y );
				}
				return;
			}

			var exit_ent = (TrainTunnelTileEntity)ModTileEntity.ByID[ exit_id ];
			
			for( int i = 0; i < Main.player.Length; i++ ) {
				Player plr = Main.player[i];
				if( plr == null || !plr.active ) { continue; }

				var myplayer = plr.GetModPlayer<OnARailPlayer>();
				if( myplayer.MyTrainWho == -1 ) { continue; }

				CustomEntity train_ent = CustomEntityManager.GetEntityByWho( myplayer.MyTrainWho );
				if( train_ent == null ) { continue; }

				var behav_comp = train_ent.GetComponentByType<TrainBehaviorEntityComponent>();
				behav_comp.CheckTunnel( train_ent, this, exit_ent );
			}
		}


		////////////////

		public Rectangle GetTileRectangle() {
			return new Rectangle( this.Position.X, this.Position.Y, TrainTunnelTile.Width, TrainTunnelTile.Height );
		}

		public Rectangle GetWorldRectangle() {
			return new Rectangle( this.Position.X * 16, this.Position.Y * 16, TrainTunnelTile.Width * 16, TrainTunnelTile.Height * 16 );
		}
	}
}
