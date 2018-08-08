using System;
using System.Collections.Generic;
using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Components.CustomEntity.Components;
using HamstarHelpers.Components.CustomEntity.Templates;
using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.PlayerHelpers;
using HamstarHelpers.Services.Promises;
using Microsoft.Xna.Framework;
using OnARail.Buffs;
using OnARail.Entities.Components;
using Terraria;


namespace OnARail.Entities {
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
			train_comp.OwnerWho = player.whoAmI;

			if( Main.netMode == 2 ) {
				ent.SyncTo();
			}

			return who;
		}


		public static int FindMyTrain( Player player ) {
			if( !SaveableEntityComponent.IsLoaded ) {
				throw new HamstarException( "OnARail.TrainEntityHandler.FindMyTrain - Entities not loaded." );
			}

			bool success;
			string uid = PlayerIdentityHelpers.GetUniqueId( player, out success );
			if( !success ) {
				LogHelpers.Log( "OnARail.TrainEntityHandler.FindMyTrain - Player uid not found for " + player.name );
				return -1;
			}

			ISet<CustomEntity> ents = CustomEntityManager.GetEntitiesByComponent<TrainBehaviorEntityComponent>();

			foreach( var ent in ents ) {
				var train_comp = ent.GetComponentByType<TrainBehaviorEntityComponent>();

				if( train_comp.OwnerUID == uid ) {
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

			CustomEntity ent = CustomEntityManager.GetEntityByWho( myplayer.MyTrainWho );
			if( ent == null ) {
				throw new HamstarException( "OnARail.TrainEntityHandler.WarpPlayerToTrain - Player " + player.name + " (" + player.whoAmI + ") has no train entity." );
			}
			
			PlayerHelpers.Teleport( player, ent.Core.Center - new Vector2( player.width / 2, (player.height / 2 ) + 16) );
			
			int train_buff_id = OnARailMod.Instance.BuffType<TrainMountBuff>();
			player.AddBuff( train_buff_id, 3 );
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
					new SaveableEntityComponent( OnARailMod.Instance.Config.SaveTrainDataAsJson )
				};

				TrainEntityHandler.TrainEntityID = CustomEntityTemplateManager.Add( "Unnamed Train", 64, 48, comps );
			} );
		}
	}
}
