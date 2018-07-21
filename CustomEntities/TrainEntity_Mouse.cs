using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Components.CustomEntity.Components;
using HamstarHelpers.Helpers.DebugHelpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using OnARail.Buffs;
using Terraria;


namespace OnARail.CustomEntities {
	class TrainMouseInteractionEntityComponent : IsClickableEntityComponent {
		public const float BoardingDistance = 96;


		[JsonIgnore]
		public bool IsHovering = false;


		protected override void OnMouseHover( CustomEntity ent ) {
			Player player = Main.LocalPlayer;

			if( Main.mouseRight ) {
				if( this.IsHovering ) {
					player.position = ent.position;
					player.AddBuff( OnARailMod.Instance.BuffType<TrainMountBuff>(), 60 );
					
					CustomEntityManager.Instance.Remove( ent );
				}
			} else {
				this.IsHovering = player.Distance( ent.Center ) <= TrainMouseInteractionEntityComponent.BoardingDistance;
			}
		}
	}
}
