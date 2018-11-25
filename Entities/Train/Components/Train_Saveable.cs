using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Components.CustomEntity.Components;
using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;


namespace OnARail.Entities.Train.Components {
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
