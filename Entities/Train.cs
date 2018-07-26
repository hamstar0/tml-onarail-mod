﻿using System.Collections.Generic;
using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Components.CustomEntity.Components;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.PlayerHelpers;
using HamstarHelpers.Services.Promises;
using Microsoft.Xna.Framework;
using Terraria;


namespace OnARail.Entities {
	class TrainEntityHandler {
		private static IList<CustomEntityComponent> CommonComponents;


		////////////////

		public static int SpawnTrain( Player player ) {
			bool success;
			string uid = PlayerIdentityHelpers.GetUniqueId( player, out success );
			if( !success ) {
				LogHelpers.Log( "OnARail.CustomEntities.TrainEntityHandler.SpawnTrain - Player uid not found for " + player.name );
				return -1;
			}

			var ent = new CustomEntity( player.name+"'s Train", TrainEntityHandler.CommonComponents );
			var draw_comp = ent.GetComponentByType<TrainDrawInGameEntityComponent>();

			Vector2 pos = player.position;
			pos.X = MathHelper.Clamp( pos.X, 160, ( Main.maxTilesX - 10 ) * 16 );
			pos.Y = MathHelper.Clamp( pos.Y, 160, ( Main.maxTilesY - 10 ) * 16 );

			ent.position = pos;
			ent.width = draw_comp.Texture.Width;
			ent.height = (draw_comp.Texture.Height / draw_comp.FrameCount) - 12;

			int who = CustomEntityManager.Instance.Add( ent );

			var train_comp = ent.GetComponentByType<TrainBehaviorEntityComponent>();
			train_comp.OwnerUID = uid;

			if( Main.netMode != 0 ) {
				ent.Sync();
			}

			return who;
		}


		public static int FindMyTrain( Player player ) {
			bool success;
			string uid = PlayerIdentityHelpers.GetUniqueId( player, out success );
			if( !success ) {
				LogHelpers.Log( "OnARail.CustomEntities.TrainEntityHandler.FindMyTrain - Player uid not found for " + player.name );
				return -1;
			}

			ISet<CustomEntity> ents = CustomEntityManager.Instance.GetByComponentType<TrainBehaviorEntityComponent>();

			foreach( var ent in ents ) {
				var train_comp = ent.GetComponentByType<TrainBehaviorEntityComponent>();

				if( train_comp.OwnerUID == uid ) {
					return ent.whoAmI;
				}
			}

			return -1;
		}


		////////////////

		public static void DismountTrainEntity( Player player ) {
			var myplayer = player.GetModPlayer<OnARailPlayer>();
			if( myplayer.MyTrainID == -1 ) {
				LogHelpers.Log( "OnARail.CustomEntities.TrainEntityHandler.ActivateTrainEntity - Player "+player.name+" ("+player.whoAmI+") has no train." );
				return;
			}

			CustomEntity ent = CustomEntityManager.Instance.Get( myplayer.MyTrainID );
			if( ent == null ) {
				LogHelpers.Log( "OnARail.CustomEntities.TrainEntityHandler.ActivateTrainEntity - Player " + player.name + " (" + player.whoAmI + ") has invalid train." );
				return;
			}

			var train_comp = ent.GetComponentByType<TrainBehaviorEntityComponent>();
			if( train_comp.DismountTrain_NoSync( ent ) ) {
				if( Main.netMode != 0 ) {
					ent.Sync();
				}
			}
		}


		////////////////

		public static void WarpPlayerToTrain( Player player ) {
			var myplayer = player.GetModPlayer<OnARailPlayer>();
			if( myplayer.MyTrainID == -1 ) {
				LogHelpers.Log( "OnARail.CustomEntities.TrainEntityHandler.WarpPlayer - Player " + player.name + " (" + player.whoAmI + ") has no train." );
				return;
			}

			CustomEntity ent = CustomEntityManager.Instance.Get( myplayer.MyTrainID );

			//if( player.whoAmI == Main.myPlayer ) {
			//	Main.BlackFadeIn = 255;
			//}

			PlayerHelpers.Teleport( player, ent.position );
		}


		////////////////

		internal TrainEntityHandler() {
			Promises.AddPostModLoadPromise( () => {
				TrainEntityHandler.CommonComponents = new List<CustomEntityComponent> {
					new TrainBehaviorEntityComponent(),
					new TrainDrawInGameEntityComponent(),
					new TrainDrawOnMapEntityComponent(),
					new TrainMouseInteractionEntityComponent(),
					new TrainRespectsTerrainEntityComponent(),
					new TrainRespectsGravityEntityComponent(),
					new TrainRailBoundEntityComponent(),
					new PeriodicSyncEntityComponent(),
					new PerWorldSaveEntityComponent( OnARailMod.Instance.Config.SaveTrainDataAsJson )
				};
			} );
		}
	}
}
