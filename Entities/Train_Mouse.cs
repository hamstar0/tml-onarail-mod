using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Components.CustomEntity.Components;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Helpers.DebugHelpers;
using Newtonsoft.Json;
using Terraria;


namespace OnARail.Entities {
	class TrainMouseInteractionEntityComponent : IsClickableEntityComponent {
		public const float BoardingDistance = 96;



		////////////////
		
		[PacketProtocolIgnore]
		[JsonIgnore]
		public bool IsMouseHovering = false;



		////////////////
		
		protected override void OnMouseHover( CustomEntity ent ) {
			Player player = Main.LocalPlayer;

			this.IsMouseHovering = player.Distance( ent.Center ) <= TrainMouseInteractionEntityComponent.BoardingDistance;

			if( this.IsMouseHovering ) {
				if( !player.dead ) {
					if( Main.mouseRight ) {
						var train_comp = ent.GetComponentByType<TrainBehaviorEntityComponent>();

						if( train_comp.MountTrain_NoSync( ent, player ) ) {
							if( Main.netMode != 0 ) {
								ent.Sync();
							}
						}
					}
				}
			}
		}

		public override void Update( CustomEntity ent ) {
			this.IsMouseHovering = false;

			base.Update( ent );
		}
	}
}
