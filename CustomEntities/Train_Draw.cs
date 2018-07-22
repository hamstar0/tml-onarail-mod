using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Components.CustomEntity.Components;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Services.Promises;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;


namespace OnARail.CustomEntities {
	class TrainDrawEntityComponent : DrawsEntityComponent {
		internal static Texture2D TrainTexture;
		internal static Texture2D TrainIcon;


		////////////////

		protected class MyStaticInitializer : StaticInitializer {
			protected override void StaticInitialize() {
				if( Main.netMode != 2 ) {
					Promises.AddPostModLoadPromise( () => {
						TrainDrawEntityComponent.TrainTexture = OnARailMod.Instance.GetTexture( "Mounts/TrainMount_Back" );
						TrainDrawEntityComponent.TrainIcon = OnARailMod.Instance.GetTexture( "Logic/TrainIcon" );
					} );
				}
			}
		}


		////////////////

		private float PulseScaleAnimation = 0f;


		////////////////

		public TrainDrawEntityComponent() : base( "OnARail/Mounts/TrainMount_Back", 4 ) { }

		////////////////

		public override void PostDraw( SpriteBatch sb, CustomEntity ent ) {
			var mouse_comp = ent.GetComponentByType<TrainMouseInteractionEntityComponent>();

			if( mouse_comp.IsMouseHovering && !mouse_comp.IsMounted ) {
				var pos = new Vector2( Main.mouseX - TrainDrawEntityComponent.TrainIcon.Width, Main.mouseY - TrainDrawEntityComponent.TrainIcon.Height );
				float scale = 1f + ( ( this.PulseScaleAnimation > 0 ? this.PulseScaleAnimation : -this.PulseScaleAnimation ) / 90f );

				if( this.PulseScaleAnimation >= 30 ) {
					this.PulseScaleAnimation = -this.PulseScaleAnimation;
				} else {
					this.PulseScaleAnimation++;
				}

				sb.Draw( TrainDrawEntityComponent.TrainIcon, pos, null, Color.White, 0f, default( Vector2 ), scale, SpriteEffects.None, 1f );
			}
		}
	}
}
