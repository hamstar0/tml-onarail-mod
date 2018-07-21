using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Components.CustomEntity.Components;
using HamstarHelpers.Helpers.DebugHelpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using OnARail.Buffs;
using Terraria;


namespace OnARail.CustomEntities {
	class TrainDrawEntityComponent : DrawsEntityComponent {
		private float PulseScaleAnimation = 0f;


		////////////////

		public TrainDrawEntityComponent() : base( "Mounts/TrainMount_Back", 4 ) { }

		////////////////

		public override void PostDraw( SpriteBatch sb, CustomEntity ent ) {
			var mouse_comp = (TrainMouseInteractionEntityComponent)ent.GetComponentByType<TrainMouseInteractionEntityComponent>();

			if( mouse_comp.IsHovering ) {
				var pos = new Vector2( Main.mouseX - TrainEntityFactory.TrainIcon.Width, Main.mouseY - TrainEntityFactory.TrainIcon.Height );
				float scale = 1f + ( ( this.PulseScaleAnimation > 0 ? this.PulseScaleAnimation : -this.PulseScaleAnimation ) / 90f );

				if( this.PulseScaleAnimation >= 30 ) {
					this.PulseScaleAnimation = -this.PulseScaleAnimation;
				} else {
					this.PulseScaleAnimation++;
				}

				sb.Draw( TrainEntityFactory.TrainIcon, pos, null, Color.White, 0f, default( Vector2 ), scale, SpriteEffects.None, 1f );
			}
		}
	}



	class TrainMouseInteractionEntityComponent : IsClickableEntityComponent {
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
				this.IsHovering = player.Distance( ent.Center ) <= TrainEntityFactory.BoardingDistance;
			}
		}
	}
}
