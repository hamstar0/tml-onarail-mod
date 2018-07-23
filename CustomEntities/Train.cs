using System.Collections.Generic;
using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Components.CustomEntity.Components;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Services.Promises;
using Microsoft.Xna.Framework;
using Terraria;


namespace OnARail.CustomEntities {
	class TrainEntityHandler {
		private static IList<CustomEntityComponent> CommonComponents;


		////////////////

		public static int CreateTrain( Vector2 pos ) {
			var ent = new CustomEntity( TrainEntityHandler.CommonComponents );
			var draw_comp = ent.GetComponentByType<TrainDrawEntityComponent>();

			pos.X = MathHelper.Clamp( pos.X, 160, ( Main.maxTilesX - 10 ) * 16 );
			pos.Y = MathHelper.Clamp( pos.Y, 160, ( Main.maxTilesY - 10 ) * 16 );

			ent.position = pos;
			ent.width = draw_comp.Texture.Width;
			ent.height = ( draw_comp.Texture.Height / draw_comp.FrameCount ) - 12;

			return CustomEntityManager.Instance.Add( ent );
		}


		public static void ActivateTrainEntity( Player player ) {
			var myplayer = player.GetModPlayer<OnARailPlayer>();
			if( myplayer.MyTrainId == -1 ) {
				LogHelpers.Log( "OnARail.CustomEntities.TrainEntityHandler.ActivateTrainEntity - Player "+player.name+" ("+player.whoAmI+") has no train." );
				return;
			}

			CustomEntity ent = CustomEntityManager.Instance[ myplayer.MyTrainId ];
			var train_comp = ent.GetComponentByType<TrainBehaviorEntityComponent>();

			train_comp.DismountTrain_NoSync( ent );

			if( Main.netMode != 0 ) {
				ent.Sync();
			}
		}


		////////////////

		internal TrainEntityHandler() {
			Promises.AddPostModLoadPromise( () => {
				TrainEntityHandler.CommonComponents = new List<CustomEntityComponent> {
					new TrainBehaviorEntityComponent(),
					new TrainDrawEntityComponent(),
					new TrainMouseInteractionEntityComponent(),
					new TrainRespectsTerrainEntityComponent(),
					new TrainRespectsGravityEntityComponent(),
					new TrainRailBoundEntityComponent(),
					new PeriodicSyncEntityComponent()
					//new PerWorldSaveEntityComponent( OnARailMod.Instance.Config.SaveTrainDataAsJson )
				};
			} );
		}
	}
}
