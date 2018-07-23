using HamstarHelpers.Components.CustomEntity.Components;
using HamstarHelpers.Helpers.DebugHelpers;


namespace OnARail.CustomEntities {
	class TrainDrawOnMapEntityComponent : DrawsOnMapEntityComponent {
		public TrainDrawOnMapEntityComponent() : base( "OnARail/CustomEntities/TrainIcon", 1, 0.5f ) { }
	}
}
