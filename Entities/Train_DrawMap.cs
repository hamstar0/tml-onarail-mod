using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Components.CustomEntity.Components;
using HamstarHelpers.Helpers.DebugHelpers;
using Microsoft.Xna.Framework.Graphics;


namespace OnARail.Entities {
	class TrainDrawOnMapEntityComponent : DrawsOnMapEntityComponent {
		public TrainDrawOnMapEntityComponent() : base( "OnARail", "Entities/TrainIcon", 1, 0.25f ) { }


		////////////////

		public override bool PreDrawFullscreenMap( SpriteBatch sb, CustomEntity ent ) {
			var train_comp = ent.GetComponentByType<TrainBehaviorEntityComponent>();

			return train_comp.IsLocallyOwned( ent );
		}

		public override bool PreDrawMiniMap( SpriteBatch sb, CustomEntity ent ) {
			var train_comp = ent.GetComponentByType<TrainBehaviorEntityComponent>();

			return train_comp.IsLocallyOwned( ent );
		}

		public override bool PreDrawOverlayMap( SpriteBatch sb, CustomEntity ent ) {
			var train_comp = ent.GetComponentByType<TrainBehaviorEntityComponent>();

			return train_comp.IsLocallyOwned( ent );
		}
	}
}
