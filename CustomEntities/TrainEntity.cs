using System.Collections.Generic;
using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Components.CustomEntity.Components;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Services.Promises;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;


namespace OnARail.CustomEntities {
	public class TrainEntityFactory {
		public const float BoardingDistance = 96;

		internal static Texture2D TrainTexture;
		internal static Texture2D TrainIcon;
		private static IList<CustomEntityComponent> Components;


		////////////////

		static TrainEntityFactory() {
			if( Main.netMode != 2 ) {
				Promises.AddPostModLoadPromise( () => {
					TrainEntityFactory.TrainTexture = OnARailMod.Instance.GetTexture( "Mounts/TrainMount_Back" );
					TrainEntityFactory.TrainIcon = OnARailMod.Instance.GetTexture( "CustomEntities/TrainIcon" );
				} );
			}

			Promises.AddPostModLoadPromise( () => {
				TrainEntityFactory.Components = new List<CustomEntityComponent> {
					new TrainDrawEntityComponent(),
					new TrainMouseInteractionEntityComponent(),
					new RespectsTerrainEntityComponent(),
					new RespectsGravityEntityComponent(),
					new RailBoundEntityComponent(),
					new PeriodicSyncEntityComponent(),
					new PerWorldSaveEntityComponent( OnARailMod.Instance.Config.SaveTrainDataAsJson )
				};
			} );
		}

		////////////////

		public static int CreateTrain( Vector2 pos ) {
			var ent = new CustomEntity( TrainEntityFactory.Components );
			var draw_comp = (DrawsEntityComponent)ent.GetComponentByType<DrawsEntityComponent>();

			pos.X = MathHelper.Clamp( pos.X, 160, ( Main.maxTilesX - 10 ) * 16 );
			pos.Y = MathHelper.Clamp( pos.Y, 160, ( Main.maxTilesY - 10 ) * 16 );

			ent.position = pos;
			ent.width = TrainEntityFactory.TrainTexture.Width;
			ent.height = ( TrainEntityFactory.TrainTexture.Height / draw_comp.FrameCount ) - 12;

			return CustomEntityManager.Instance.Add( ent );
		}
	}
}
