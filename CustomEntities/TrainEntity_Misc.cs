using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Components.CustomEntity.Components;
using HamstarHelpers.Helpers.DebugHelpers;


namespace OnARail.CustomEntities {
	class TrainRespectsTerrainEntityComponent : RespectsTerrainEntityComponent {
		public override void Update( CustomEntity ent ) {
			var mouse_comp = ent.GetComponentByType<TrainMouseInteractionEntityComponent>();

			if( !mouse_comp.IsMounted ) {
				base.Update( ent );
			}
		}
	}

	class TrainRespectsGravityEntityComponent : RespectsGravityEntityComponent {
		public override void Update( CustomEntity ent ) {
			var mouse_comp = ent.GetComponentByType<TrainMouseInteractionEntityComponent>();

			if( !mouse_comp.IsMounted ) {
				base.Update( ent );
			}
		}
	}

	class TrainRailBoundEntityComponent : RailBoundEntityComponent {
		public override void Update( CustomEntity ent ) {
			var mouse_comp = ent.GetComponentByType<TrainMouseInteractionEntityComponent>();

			if( !mouse_comp.IsMounted ) {
				base.Update( ent );
			}
		}
	}
}
