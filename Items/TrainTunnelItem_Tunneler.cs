using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.TileHelpers;
using Microsoft.Xna.Framework;
using OnARail.Tiles;
using System;
using Terraria;
using Terraria.ModLoader;


namespace OnARail.Items {
	partial class TrainTunnelItem : ModItem {
		public static float GetRadiansOfDirection( int direction ) {
			switch( direction ) {
			case 0:
				return 0;
			case 1:
				return ( MathHelper.Pi / 180f ) * 45;
			case 2:
				return ( MathHelper.Pi / 180f ) * 135;
			case 3:
				return MathHelper.Pi;
			case 4:
				return ( MathHelper.Pi / 180f ) * 225;
			case 5:
				return ( MathHelper.Pi / 180f ) * 315;
			default:
				throw new Exception( "Invalid direction." );
			}
		}


		public static Vector2? ScanForExitCandidate( Vector2 src, float rad, float range ) {
			var dir = new Vector2(
				(float)Math.Cos( rad ),
				-(float)Math.Sin( rad )
			);
			var dest = src + (dir * range * 16);

			int x = (int)( dest.X / 16f );
			int y = (int)( dest.Y / 16f );

			for( int i=x; i<x+TrainTunnelTile.Width; i++ ) {
				for( int j=y; j<y+TrainTunnelTile.Height; j++ ) {
					Tile tile = Main.tile[ i, j ];

					if( tile.wall == 0 || TileHelpers.IsSolid(tile) ) {
						return null;
					}
				}
			}

			return dest;
		}
	}
}
