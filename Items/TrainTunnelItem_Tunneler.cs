using Microsoft.Xna.Framework;
using System;
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


		public static Vector2 ScanForExitCandidate( Vector2 src, float rad, float range ) {
			return new Vector2( src.X+80, src.Y );
		}
	}
}
