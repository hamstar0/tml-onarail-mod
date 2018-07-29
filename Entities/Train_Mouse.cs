using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Components.CustomEntity.Components;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Helpers.DebugHelpers;
using Newtonsoft.Json;
using OnARail.Buffs;
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
				if( Main.mouseRight ) {
					if( !player.dead ) {
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

		public override void Update( CustomEntity ent ) {
			this.IsMouseHovering = false;

			base.Update( ent );
		}
	}
}
