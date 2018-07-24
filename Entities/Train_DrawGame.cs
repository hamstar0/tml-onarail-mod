using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Components.CustomEntity.Components;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Services.Promises;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;


namespace OnARail.Entities {
	class TrainDrawInGameEntityComponent : DrawsInGameEntityComponent {
		internal static Texture2D TrainIcon;


		////////////////

		protected class MyStaticInitializer : StaticInitializer {
			protected override void StaticInitialize() {
				if( Main.netMode != 2 ) {
					Promises.AddPostModLoadPromise( () => {
						TrainDrawInGameEntityComponent.TrainIcon = OnARailMod.Instance.GetTexture( "Entities/TrainIcon" );
					} );
				}
			}
		}


		////////////////

		private float PulseScaleAnimation = 0f;


		////////////////

		public TrainDrawInGameEntityComponent() : base( "OnARail/Mounts/TrainMount_Back", 4 ) { }

		////////////////

		public override bool PreDraw( SpriteBatch sb, CustomEntity ent ) {
			var train_comp = ent.GetComponentByType<TrainBehaviorEntityComponent>();

			if( train_comp.IsMountedBy != -1 ) {
				return false;
			}

			return base.PreDraw( sb, ent );
		}

		public override void PostDraw( SpriteBatch sb, CustomEntity ent ) {
			var train_comp = ent.GetComponentByType<TrainBehaviorEntityComponent>();
			var mouse_comp = ent.GetComponentByType<TrainMouseInteractionEntityComponent>();
			
			if( mouse_comp.IsMouseHovering && train_comp.IsMountedBy == -1 ) {
				var pos = new Vector2(
					Main.mouseX - TrainDrawInGameEntityComponent.TrainIcon.Width,
					Main.mouseY - TrainDrawInGameEntityComponent.TrainIcon.Height
				);
				float scale = 1f + ( ( this.PulseScaleAnimation > 0 ? this.PulseScaleAnimation : -this.PulseScaleAnimation ) / 150f );

				if( this.PulseScaleAnimation >= 36 ) {
					this.PulseScaleAnimation = -this.PulseScaleAnimation;
				} else {
					this.PulseScaleAnimation++;
				}

				sb.Draw( TrainDrawInGameEntityComponent.TrainIcon, pos, null, Color.White, 0f, default( Vector2 ), scale, SpriteEffects.None, 1f );
			}
		}
	}
}
