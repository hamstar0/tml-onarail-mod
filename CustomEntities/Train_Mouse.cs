using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Components.CustomEntity.Components;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Helpers.DebugHelpers;
using Newtonsoft.Json;
using OnARail.Buffs;
using Terraria;


namespace OnARail.CustomEntities {
	class TrainMouseInteractionEntityComponent : IsClickableEntityComponent {
		public const float BoardingDistance = 96;



		////////////////

		[JsonIgnore]
		public bool IsMounted = false;

		[PacketProtocolIgnore]
		[JsonIgnore]
		public bool IsMouseHovering = false;



		////////////////

		protected override void OnMouseHover( CustomEntity ent ) {
			Player player = Main.LocalPlayer;

			if( Main.mouseRight ) {
				if( !this.IsMounted && this.IsMouseHovering ) {
					player.position = ent.position;
					player.AddBuff( OnARailMod.Instance.BuffType<TrainMountBuff>(), 60 );

					this.IsMounted = true;
					ent.Sync();
				}
			} else {
				this.IsMouseHovering = player.Distance( ent.Center ) <= TrainMouseInteractionEntityComponent.BoardingDistance;
			}
		}
	}
}
