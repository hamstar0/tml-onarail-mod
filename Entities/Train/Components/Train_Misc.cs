using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Components.CustomEntity.Components;
using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;


namespace OnARail.Entities.Train.Components {
	class TrainRespectsTerrainEntityComponent : RespectsTerrainEntityComponent {
		private class TrainRespectsTerrainEntityComponentFactory : CustomEntityComponentFactory<TrainRespectsTerrainEntityComponent> {
			public TrainRespectsTerrainEntityComponentFactory() { }
			public override void InitializeComponent( TrainRespectsTerrainEntityComponent data ) { }
		}

		////////////////

		public static TrainRespectsTerrainEntityComponent CreateTrainRespectsTerrainEntityComponent() {
			var factory = new TrainRespectsTerrainEntityComponentFactory();
			return factory.Create();
		}


		////////////////

		private TrainRespectsTerrainEntityComponent( PacketProtocolDataConstructorLock ctor_lock ) : base( ctor_lock ) { }


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
		protected class TrainRespectsGravityEntityComponentFactory : CustomEntityComponentFactory<TrainRespectsGravityEntityComponent> {
			public TrainRespectsGravityEntityComponentFactory() { }
			public override void InitializeComponent( TrainRespectsGravityEntityComponent data ) { }
		}

		////////////////

		public static TrainRespectsGravityEntityComponent CreateTrainRespectsGravityEntityComponent() {
			var factory = new TrainRespectsGravityEntityComponentFactory();
			return factory.Create();
		}


		////////////////
		
		private TrainRespectsGravityEntityComponent( PacketProtocolDataConstructorLock ctor_lock ) : base( ctor_lock ) { }


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
		protected class TrainRailBoundEntityComponentFactory : CustomEntityComponentFactory<TrainRailBoundEntityComponent> {
			public TrainRailBoundEntityComponentFactory() { }
			public override void InitializeComponent( TrainRailBoundEntityComponent data ) { }
		}

		////////////////

		public static TrainRailBoundEntityComponent CreateTrainRailBoundEntityComponent() {
			var factory = new TrainRailBoundEntityComponentFactory();
			return factory.Create();
		}


		////////////////
		
		private TrainRailBoundEntityComponent( PacketProtocolDataConstructorLock ctor_lock ) : base( ctor_lock ) { }

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
		protected class TrainPeriodicSyncEntityComponentFactory : CustomEntityComponentFactory<TrainPeriodicSyncEntityComponent> {
			public TrainPeriodicSyncEntityComponentFactory() { }
			public override void InitializeComponent( TrainPeriodicSyncEntityComponent data ) { }
		}

		////////////////

		public static TrainPeriodicSyncEntityComponent CreateTrainPeriodicSyncEntityComponent() {
			var factory = new TrainPeriodicSyncEntityComponentFactory();
			return factory.Create();
		}


		////////////////
		
		private TrainPeriodicSyncEntityComponent( PacketProtocolDataConstructorLock ctor_lock ) : base( ctor_lock ) { }

		////////////////

		public override void UpdateClient( CustomEntity ent ) {
			var train_comp = ent.GetComponentByType<TrainBehaviorEntityComponent>();
			base.UpdateMe( ent );
		}
		public override void UpdateServer( CustomEntity ent ) { }
	}




	class TrainSaveableEntityComponent : SaveableEntityComponent {
		protected class TrainPeriodicSyncEntityComponentFactory : SaveableEntityComponentFactory<TrainSaveableEntityComponent> {
			public TrainPeriodicSyncEntityComponentFactory( bool as_json ) : base( as_json ) { }
		}

		////////////////

		public static TrainSaveableEntityComponent CreateTrainSaveableEntityComponent( bool as_json ) {
			var factory = new TrainPeriodicSyncEntityComponentFactory( as_json );
			return factory.Create();
		}


		////////////////

		protected TrainSaveableEntityComponent( PacketProtocolDataConstructorLock ctor_lock ) : base( ctor_lock ) { }


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
