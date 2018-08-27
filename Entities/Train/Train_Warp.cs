using System;
using System.Collections.Generic;
using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Components.CustomEntity.Components;
using HamstarHelpers.Components.CustomEntity.Templates;
using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.PlayerHelpers;
using HamstarHelpers.Services.Promises;
using HamstarHelpers.Services.Timers;
using Microsoft.Xna.Framework;
using OnARail.Buffs;
using OnARail.Entities.Train.Components;
using OnARail.Tiles;
using Terraria;


namespace OnARail.Entities.Train {
	public partial class TrainEntityHandler {
		public static void WarpPlayerToTrain( Player player ) {
			if( !SaveableEntityComponent.IsLoaded ) {
				throw new HamstarException( "OnARail.TrainEntityHandler.WarpPlayerToTrain - Entities not loaded." );
			}

			var myplayer = player.GetModPlayer<OnARailPlayer>();
			if( myplayer.MyTrainWho == -1 ) {
				throw new HamstarException( "OnARail.TrainEntityHandler.WarpPlayerToTrain - Player " + player.name + " (" + player.whoAmI + ") has no train." );
			}

			CustomEntity train_ent = CustomEntityManager.GetEntityByWho( myplayer.MyTrainWho );
			if( train_ent == null ) {
				throw new HamstarException( "OnARail.TrainEntityHandler.WarpPlayerToTrain - Player " + player.name + " (" + player.whoAmI + ") has no train entity." );
			}

			PlayerHelpers.Teleport( player, train_ent.Core.Center - new Vector2( player.width / 2, ( player.height / 2 ) + 16 ) );

			int train_buff_id = OnARailMod.Instance.BuffType<TrainMountBuff>();
			player.AddBuff( train_buff_id, 30 );
		}


		////////////////

		public static bool CheckTunnel( int train_who, TrainTunnelTileEntity from_tunnel, TrainTunnelTileEntity to_tunnel ) {
			bool has_tunneled = false;
			
			CustomEntity train_ent = CustomEntityManager.GetEntityByWho( train_who );
			var behav_comp = train_ent.GetComponentByType<TrainBehaviorEntityComponent>();
			string timer_name = "TrainTunnelWarp" + train_ent.ID;

			if( from_tunnel.GetWorldRectangle().Intersects( train_ent.Core.Hitbox ) ) {
				if( behav_comp.IsMountedBy == -1 ) {
					// TODO
				} else {
					Vector2 to_tunnel_pos = to_tunnel.GetWorldRectangle().Center.ToVector2();
					to_tunnel_pos.Y -= 32;

					if( Timers.GetTimerTickDuration(timer_name) <= 0 ) {
						Player plr = Main.player[ behav_comp.OwnerPlayerWho ];
						Vector2 vel = plr.velocity;

						PlayerHelpers.Teleport( plr, to_tunnel_pos );
						plr.velocity = vel;
					}

					has_tunneled = true;
				}
				
				Timers.SetTimer( timer_name, 4, () => false );
			}

			return has_tunneled;
		}
	}
}
