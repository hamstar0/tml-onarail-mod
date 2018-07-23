using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Components.CustomEntity.Components;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Helpers.DebugHelpers;
using Newtonsoft.Json;
using Terraria;


namespace OnARail.CustomEntities {
	class TrainMouseInteractionEntityComponent : IsClickableEntityComponent {
		public const float BoardingDistance = 96;



		////////////////

		[PacketProtocolIgnore]
		[JsonIgnore]
		public bool IsMouseHovering = false;



		////////////////
		
		protected override void OnMouseHover( CustomEntity ent ) {
			Player player = Main.LocalPlayer;

			if( Main.mouseRight ) {
				var train_comp = ent.GetComponentByType<TrainBehaviorEntityComponent>();

				if( this.IsMouseHovering ) {
					train_comp.MountTrain_NoSync( ent, player );

					if( Main.netMode != 0 ) {
						ent.Sync();
					}
				}
			} else {
				this.IsMouseHovering = player.Distance( ent.Center ) <= TrainMouseInteractionEntityComponent.BoardingDistance;
			}
		}
	}
}
