using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Components.CustomEntity.Components;
using HamstarHelpers.Helpers.DebugHelpers;


namespace OnARail.Entities {
	class TrainRespectsTerrainEntityComponent : RespectsTerrainEntityComponent {
		public override void UpdateSingle( CustomEntity ent ) {
			var train_comp = ent.GetComponentByType<TrainBehaviorEntityComponent>();

			if( train_comp.IsMountedBy == -1 ) {
				base.UpdateSingle( ent );
			}
		}
		public override void UpdateClient( CustomEntity ent ) {
			var train_comp = ent.GetComponentByType<TrainBehaviorEntityComponent>();

			if( train_comp.IsMountedBy == -1 ) {
				base.UpdateClient( ent );
			}
		}
		public override void UpdateServer( CustomEntity ent ) {
			var train_comp = ent.GetComponentByType<TrainBehaviorEntityComponent>();

			if( train_comp.IsMountedBy == -1 ) {
				base.UpdateServer( ent );
			}
		}
	}


	class TrainRespectsGravityEntityComponent : RespectsGravityEntityComponent {
		public override void UpdateSingle( CustomEntity ent ) {
			var train_comp = ent.GetComponentByType<TrainBehaviorEntityComponent>();

			if( train_comp.IsMountedBy == -1 ) {
				base.UpdateSingle( ent );
			}
		}
		public override void UpdateClient( CustomEntity ent ) {
			var train_comp = ent.GetComponentByType<TrainBehaviorEntityComponent>();

			if( train_comp.IsMountedBy == -1 ) {
				base.UpdateClient( ent );
			}
		}
		public override void UpdateServer( CustomEntity ent ) {
			var train_comp = ent.GetComponentByType<TrainBehaviorEntityComponent>();

			if( train_comp.IsMountedBy == -1 ) {
				base.UpdateServer( ent );
			}
		}
	}


	class TrainRailBoundEntityComponent : RailBoundEntityComponent {
		public override void UpdateSingle( CustomEntity ent ) {
			var train_comp = ent.GetComponentByType<TrainBehaviorEntityComponent>();

			if( train_comp.IsMountedBy == -1 ) {
				base.UpdateSingle( ent );
			}
		}
		public override void UpdateClient( CustomEntity ent ) {
			var train_comp = ent.GetComponentByType<TrainBehaviorEntityComponent>();

			if( train_comp.IsMountedBy == -1 ) {
				base.UpdateClient( ent );
			}
		}
		public override void UpdateServer( CustomEntity ent ) {
			var train_comp = ent.GetComponentByType<TrainBehaviorEntityComponent>();

			if( train_comp.IsMountedBy == -1 ) {
				base.UpdateServer( ent );
			}
		}
	}
}
