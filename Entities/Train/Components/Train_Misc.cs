using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Components.CustomEntity.Components;
using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using Terraria;


namespace OnARail.Entities.Train.Components {
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
			base.UpdateMe( ent );
		}
		public override void UpdateServer( CustomEntity ent ) { }
	}




	class TrainSaveableEntityComponent : SaveableEntityComponent {
		private TrainSaveableEntityComponent( PacketProtocolDataConstructorLock ctor_lock ) : base(false) { }

		public TrainSaveableEntityComponent( bool as_json ) : base(as_json) { }


		////////////////

		private void OnLoad( CustomEntity ent ) {
			var behav_comp = ent.GetComponentByType<TrainBehaviorEntityComponent>();
			if( behav_comp == null ) {
				throw new HamstarException( "Train entity "+ent.ToString()+" is missing TrainBehaviorEntityComponent." );
			}
		}

		protected override void OnLoadSingle( CustomEntity ent ) {
			this.OnLoad( ent );
		}
		protected override void OnLoadClient( CustomEntity ent ) {
			this.OnLoad( ent );
		}
		protected override void OnLoadServer( CustomEntity ent ) {
			this.OnLoad( ent );
		}
	}
}
