using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Components.CustomEntity.Components;
using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;


namespace OnARail.Entities.Train.Components {
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
}
