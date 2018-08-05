using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Components.CustomEntity.Components;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Helpers.DebugHelpers;
using Newtonsoft.Json;
using OnARail.Buffs;
using Terraria;
using Terraria.ID;


namespace OnARail.Entities.Components {
	class TrainMouseInteractionEntityComponent : IsClickableEntityComponent {
		public const float BoardingDistance = 96;



		////////////////
		
		[PacketProtocolIgnore]
		[JsonIgnore]
		public bool IsMouseHovering = false;



		////////////////

		public TrainMouseInteractionEntityComponent() {
			this.ConfirmLoad();
		}

		////////////////

		protected override void OnMouseHover( CustomEntity ent ) {
			Player player = Main.LocalPlayer;

			this.IsMouseHovering = player.Distance( ent.Core.Center ) <= TrainMouseInteractionEntityComponent.BoardingDistance;
			
			if( this.IsMouseHovering ) {
				if( Main.mouseRight ) {
					if( !player.dead && !(player.showItemIcon && player.showItemIcon2 == ItemID.Minecart) ) {
						var train_comp = ent.GetComponentByType<TrainBehaviorEntityComponent>();

						if( train_comp.IsLocallyOwned( ent ) ) {
							int train_buff_id = OnARailMod.Instance.BuffType<TrainMountBuff>();

							if( player.FindBuffIndex( train_buff_id ) == -1 ) {
								player.AddBuff( train_buff_id, 3 );
							}
						}
					}
				}
			}
		}
		
		public override void UpdateSingle( CustomEntity ent ) {
			this.IsMouseHovering = false;
			base.UpdateSingle( ent );
		}
		public override void UpdateClient( CustomEntity ent ) {
			this.IsMouseHovering = false;
			base.UpdateClient( ent );
		}
	}
}
