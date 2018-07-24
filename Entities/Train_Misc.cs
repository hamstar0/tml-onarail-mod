using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Components.CustomEntity.Components;
using HamstarHelpers.Helpers.DebugHelpers;


namespace OnARail.Entities {
	class TrainRespectsTerrainEntityComponent : RespectsTerrainEntityComponent {
		public override void Update( CustomEntity ent ) {
			var train_comp = ent.GetComponentByType<TrainBehaviorEntityComponent>();

			if( train_comp.IsMountedBy == -1 ) {
				base.Update( ent );
			}
		}
	}

	class TrainRespectsGravityEntityComponent : RespectsGravityEntityComponent {
		public override void Update( CustomEntity ent ) {
			var train_comp = ent.GetComponentByType<TrainBehaviorEntityComponent>();

			if( train_comp.IsMountedBy == -1 ) {
				base.Update( ent );
			}
		}
	}

	class TrainRailBoundEntityComponent : RailBoundEntityComponent {
		public override void Update( CustomEntity ent ) {
			var train_comp = ent.GetComponentByType<TrainBehaviorEntityComponent>();

			if( train_comp.IsMountedBy == -1 ) {
				base.Update( ent );
			}
		}
	}
}
