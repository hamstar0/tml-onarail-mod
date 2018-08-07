using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Components.CustomEntity.Components;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;


namespace OnARail.Entities.Components {
	class TrainRespectsTerrainEntityComponent : RespectsTerrainEntityComponent {
		private TrainRespectsTerrainEntityComponent( PacketProtocolDataConstructorLock ctor_lock ) : this() { }

		public TrainRespectsTerrainEntityComponent() {
			this.ConfirmLoad();
		}

		////////////////

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
		private TrainRespectsGravityEntityComponent( PacketProtocolDataConstructorLock ctor_lock ) : this() { }

		public TrainRespectsGravityEntityComponent() {
			this.ConfirmLoad();
		}

		////////////////

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
		private TrainRailBoundEntityComponent( PacketProtocolDataConstructorLock ctor_lock ) : this() { }

		public TrainRailBoundEntityComponent() {
			this.ConfirmLoad();
		}

		////////////////

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


	class TrainPeriodicSyncEntityComponent : PeriodicSyncEntityComponent {
		private TrainPeriodicSyncEntityComponent( PacketProtocolDataConstructorLock ctor_lock ) : this() { }

		public TrainPeriodicSyncEntityComponent() : base() {
			this.ConfirmLoad();
		}

		////////////////

		public override void UpdateClient( CustomEntity ent ) {
			var train_comp = ent.GetComponentByType<TrainBehaviorEntityComponent>();

			if( train_comp.IsMountedBy == -1 ) {
				base.UpdateClient( ent );
			}
		}
		public override void UpdateServer( CustomEntity ent ) { }
	}
}
