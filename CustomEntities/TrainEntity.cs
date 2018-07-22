using System.Collections.Generic;
using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Components.CustomEntity.Components;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Services.Promises;
using Microsoft.Xna.Framework;
using Terraria;


namespace OnARail.CustomEntities {
	class TrainEntityFactory {
		private static IList<CustomEntityComponent> Components;


		////////////////

		public static int CreateTrain( Vector2 pos ) {
			var ent = new CustomEntity( TrainEntityFactory.Components );
			var draw_comp = ent.GetComponentByType<TrainDrawEntityComponent>();

			pos.X = MathHelper.Clamp( pos.X, 160, ( Main.maxTilesX - 10 ) * 16 );
			pos.Y = MathHelper.Clamp( pos.Y, 160, ( Main.maxTilesY - 10 ) * 16 );

			ent.position = pos;
			ent.width = draw_comp.Texture.Width;
			ent.height = ( draw_comp.Texture.Height / draw_comp.FrameCount ) - 12;

			return CustomEntityManager.Instance.Add( ent );
		}


		////////////////

		internal TrainEntityFactory() {
			Promises.AddPostModLoadPromise( () => {
				TrainEntityFactory.Components = new List<CustomEntityComponent> {
					new TrainDrawEntityComponent(),
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
