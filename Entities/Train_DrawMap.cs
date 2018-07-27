using HamstarHelpers.Components.CustomEntity.Components;
using HamstarHelpers.Helpers.DebugHelpers;


namespace OnARail.Entities {
	class TrainDrawOnMapEntityComponent : DrawsOnMapEntityComponent {
		public TrainDrawOnMapEntityComponent() : base( "OnARail", "Entities/TrainIcon", 1, 0.25f ) { }
	}
}
