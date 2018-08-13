using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Components.CustomEntity.Components;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.Network.Data;
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

		private TrainMouseInteractionEntityComponent( PacketProtocolDataConstructorLock ctor_lock ) : this() { }

		public TrainMouseInteractionEntityComponent() {
			this.ConfirmLoad();
		}

		////////////////

		protected override void OnMouseHover( CustomEntity ent ) {
			Player player = Main.LocalPlayer;

			this.IsMouseHovering = player.Distance( ent.Core.Center ) <= TrainMouseInteractionEntityComponent.BoardingDistance;
			
			if( this.IsMouseHovering ) {
				if( Main.mouseRight ) {
					bool is_minecart_hovering = player.showItemIcon && player.showItemIcon2 == ItemID.Minecart;
					
					if( !player.dead && !is_minecart_hovering ) {
						var train_comp = ent.GetComponentByType<TrainBehaviorEntityComponent>();

						if( train_comp.OwnsMe( player ) ) {
							int train_buff_id = OnARailMod.Instance.BuffType<TrainMountBuff>();

							if( player.FindBuffIndex( train_buff_id ) == -1 ) {
								player.AddBuff( train_buff_id, 3 );	// Board train
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
