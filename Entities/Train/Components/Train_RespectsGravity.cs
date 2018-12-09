using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Components.CustomEntity.Components;
using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;


namespace OnARail.Entities.Train.Components {
	class TrainRespectsGravityEntityComponent : RespectsGravityEntityComponent {
		protected class TrainRespectsGravityEntityComponentFactory : CustomEntityComponentFactory<TrainRespectsGravityEntityComponent> {
			public TrainRespectsGravityEntityComponentFactory() { }

			protected override void InitializeComponent( TrainRespectsGravityEntityComponent data ) { }
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
}
