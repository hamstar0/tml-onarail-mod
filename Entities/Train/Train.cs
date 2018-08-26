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
	public class TrainEntityHandler {
		public static int TrainEntityID { get; private set; }



		////////////////

		public static int SpawnMyTrain( Player player ) {
			if( Main.netMode == 1 ) {
				throw new HamstarException( "No client." );
			}

			bool success;
			string uid = PlayerIdentityHelpers.GetUniqueId( player, out success );
			if( !success ) {
				LogHelpers.Log( "OnARail.TrainEntityHandler.SpawnTrain - Player uid not found for " + player.name );
				return -1;
			}

			var ent = CustomEntityTemplateManager.CreateEntityByID( TrainEntityHandler.TrainEntityID );
			ent.Core.DisplayName = player.name + "'s Train";

			Vector2 pos = player.Center;
			pos.Y -= 16;
			pos.X = MathHelper.Clamp( pos.X, 160, ( Main.maxTilesX - 10 ) * 16 );
			pos.Y = MathHelper.Clamp( pos.Y, 160, ( Main.maxTilesY - 10 ) * 16 );

			ent.Core.Center = pos;

			int who = CustomEntityManager.AddEntity( ent );

			var train_comp = ent.GetComponentByType<TrainBehaviorEntityComponent>();
			train_comp.OwnerUID = uid;
			train_comp.OwnerPlayerWho = player.whoAmI;

			if( Main.netMode == 2 ) {
				ent.SyncTo();
			}

			return who;
		}


		public static int FindMyTrain( Player player ) {
			if( !SaveableEntityComponent.IsLoaded ) {
				throw new HamstarException( "OnARail.TrainEntityHandler.FindMyTrain - Entities not loaded." );
			}

			ISet<CustomEntity> ents = CustomEntityManager.GetEntitiesByComponent<TrainBehaviorEntityComponent>();

			foreach( var ent in ents ) {
				var train_comp = ent.GetComponentByType<TrainBehaviorEntityComponent>();

				if( train_comp.OwnsMe( player ) ) {
					return ent.Core.whoAmI;
				}
			}

			return -1;
		}


		////////////////

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

			if( from_tunnel.GetRectangle().Intersects( train_ent.Core.Hitbox ) ) {
				if( behav_comp.IsMountedBy == -1 ) {
					// TODO
				} else {
					Vector2 to_tunnel_pos = to_tunnel.GetRectangle().Center.ToVector2() * 16f;

					if( Timers.GetTimerTickDuration(timer_name) <= 0 ) {
						Player plr = Main.player[ behav_comp.OwnerPlayerWho ];
						PlayerHelpers.Teleport( plr, to_tunnel_pos );
					}

					has_tunneled = true;
				}
				
				Timers.SetTimer( timer_name, 4, () => false );
			}

			return has_tunneled;
		}



		////////////////

		internal TrainEntityHandler() {
			Promises.AddPostModLoadPromise( () => {
				var comps = new List<CustomEntityComponent> {
					new TrainBehaviorEntityComponent(),
					new TrainDrawInGameEntityComponent(),
					new TrainDrawOnMapEntityComponent(),
					new TrainMouseInteractionEntityComponent(),
					new TrainRespectsTerrainEntityComponent(),
					new TrainRespectsGravityEntityComponent(),
					new TrainRailBoundEntityComponent(),
					new TrainPeriodicSyncEntityComponent(),
					new TrainSaveableEntityComponent( OnARailMod.Instance.Config.SaveTrainDataAsJson )
				};

				TrainEntityHandler.TrainEntityID = CustomEntityTemplateManager.Add( "Unnamed Train", 64, 48, comps );
			} );
		}
	}
}
