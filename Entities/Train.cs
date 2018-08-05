using System.Collections.Generic;
using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Components.CustomEntity.Components;
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

		public static int SpawnTrain( Player player ) {
			bool success;
			string uid = PlayerIdentityHelpers.GetUniqueId( player, out success );
			if( !success ) {
				LogHelpers.Log( "OnARail.TrainEntityHandler.SpawnTrain - Player uid not found for " + player.name );
				return -1;
			}

			var ent = CustomEntityTemplates.CreateFromTemplateByID( TrainEntityHandler.TrainEntityID );
			ent.Core.DisplayName = player.name + "'s Train";
			
			var draw_comp = ent.GetComponentByType<TrainDrawInGameEntityComponent>();

			Vector2 pos = player.Center;
			pos.Y -= 16;
			pos.X = MathHelper.Clamp( pos.X, 160, ( Main.maxTilesX - 10 ) * 16 );
			pos.Y = MathHelper.Clamp( pos.Y, 160, ( Main.maxTilesY - 10 ) * 16 );

			ent.Core.Center = pos;
			
			int who = CustomEntityManager.Add( ent );

			var train_comp = ent.GetComponentByType<TrainBehaviorEntityComponent>();
			train_comp.OwnerUID = uid;

			if( Main.netMode != 0 ) {
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

			ISet<CustomEntity> ents = CustomEntityManager.GetByComponentType<TrainBehaviorEntityComponent>();

			foreach( var ent in ents ) {
				var train_comp = ent.GetComponentByType<TrainBehaviorEntityComponent>();

				if( train_comp.OwnerUID == uid ) {
					return ent.Core.whoAmI;
				}
			}

			return -1;
		}


		////////////////

		public static void SetTrainEntityFollowing_Synced( Player player ) {
			if( !SaveableEntityComponent.IsLoaded ) {
				throw new HamstarException( "OnARail.TrainEntityHandler.SetTrainEntityFollowing - Entities not loaded." );
			}

			var myplayer = player.GetModPlayer<OnARailPlayer>();
			if( myplayer.MyTrainID == -1 ) {
				throw new HamstarException( "OnARail.TrainEntityHandler.SetTrainEntityFollowing - Player " + player.name + " (" + player.whoAmI + ") has no train." );
			}

			CustomEntity ent = CustomEntityManager.Get( myplayer.MyTrainID );
			if( ent == null ) {
				throw new HamstarException( "OnARail.TrainEntityHandler.SetTrainEntityFollowing - Player " + player.name + " (" + player.whoAmI + ") has invalid train." );
			}

			var train_comp = ent.GetComponentByType<TrainBehaviorEntityComponent>();

			if( train_comp.SetTrainEntityFollowing_NoSync( ent, player ) ) {
				if( Main.netMode != 0 ) {
					ent.SyncTo();
				}
			}
		}


		public static void SetTrainEntityStanding_Synced( Player player ) {
			if( !SaveableEntityComponent.IsLoaded ) {
				throw new HamstarException( "OnARail.TrainEntityHandler.SetTrainEntityStanding - Entities not loaded." );
			}

			var myplayer = player.GetModPlayer<OnARailPlayer>();
			if( myplayer.MyTrainID == -1 ) {
				throw new HamstarException( "OnARail.TrainEntityHandler.SetTrainEntityStanding - Player " + player.name + " (" + player.whoAmI + ") has no train." );
			}

			CustomEntity ent = CustomEntityManager.Get( myplayer.MyTrainID );
			if( ent == null ) {
				throw new HamstarException( "OnARail.TrainEntityHandler.SetTrainEntityStanding - Player " + player.name + " (" + player.whoAmI + ") has invalid train." );
			}

			var train_comp = ent.GetComponentByType<TrainBehaviorEntityComponent>();
			if( train_comp.SetTrainEntityStanding_NoSync( ent, player ) ) {
				if( Main.netMode != 0 ) {
					ent.SyncTo();
				}
			}
		}


		////////////////

		public static void WarpPlayerToTrain( Player player ) {
			if( !SaveableEntityComponent.IsLoaded ) {
				throw new HamstarException( "OnARail.TrainEntityHandler.WarpPlayerToTrain - Entities not loaded." );
			}

			var myplayer = player.GetModPlayer<OnARailPlayer>();
			if( myplayer.MyTrainID == -1 ) {
				throw new HamstarException( "OnARail.TrainEntityHandler.WarpPlayerToTrain - Player " + player.name + " (" + player.whoAmI + ") has no train." );
			}

			CustomEntity ent = CustomEntityManager.Get( myplayer.MyTrainID );

			//if( player.whoAmI == Main.myPlayer ) {
			//	Main.BlackFadeIn = 255;
			//}

			PlayerHelpers.Teleport( player, ent.Core.Center + new Vector2(0, -16) );
			
			// Also mount train
			int train_buff_id = OnARailMod.Instance.BuffType<TrainMountBuff>();
			player.AddBuff( train_buff_id, 3 );
		}


		////////////////

		internal TrainEntityHandler() {
			//ent.width = draw_comp.Texture.Width;
			//ent.height = draw_comp.Texture.Height / draw_comp.FrameCount) - 16;
			Promises.AddPostModLoadPromise( () => {
				var comps = new List<CustomEntityComponent> {
					new TrainBehaviorEntityComponent(),
					new TrainDrawInGameEntityComponent(),
					new TrainDrawOnMapEntityComponent(),
					new TrainMouseInteractionEntityComponent(),
					new TrainRespectsTerrainEntityComponent(),
					new TrainRespectsGravityEntityComponent(),
					new TrainRailBoundEntityComponent(),
					new PeriodicSyncEntityComponent(),
					new SaveableEntityComponent( OnARailMod.Instance.Config.SaveTrainDataAsJson )
				};

				TrainEntityHandler.TrainEntityID = CustomEntityTemplates.Add( "Unnamed Train", 64, 48, comps );
			} );
		}
	}
}
