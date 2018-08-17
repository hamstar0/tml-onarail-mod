using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Components.CustomEntity.Components;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.HudHelpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;


namespace OnARail.Entities.Components {
	class TrainDrawInGameEntityComponent : DrawsInGameEntityComponent {
		public bool IsMinecartIconHovering { get; private set; }

		private readonly Texture2D TrainIcon;
		private float PulseScaleAnimation = 0f;


		////////////////

		private TrainDrawInGameEntityComponent( PacketProtocolDataConstructorLock ctor_lock ) : this() { }

		public TrainDrawInGameEntityComponent() : base( "OnARail", "Mounts/TrainMount_Back", 4 ) {
			if( Main.netMode != 2 ) {
				this.TrainIcon = OnARailMod.Instance.GetTexture( "Entities/TrainIcon" );
			}
		}


		////////////////

		public override bool PreDraw( SpriteBatch sb, CustomEntity ent ) {
			var train_comp = ent.GetComponentByType<TrainBehaviorEntityComponent>();
			Player plr = Main.LocalPlayer;

			this.IsMinecartIconHovering = plr.showItemIcon && plr.showItemIcon2 == ItemID.Minecart;

			if( train_comp.IsMountedBy != -1 ) {
				return false;
			}

			return base.PreDraw( sb, ent );
		}

		public override void PostDraw( SpriteBatch sb, CustomEntity ent ) {
			var train_comp = ent.GetComponentByType<TrainBehaviorEntityComponent>();
			var mouse_comp = ent.GetComponentByType<TrainMouseInteractionEntityComponent>();

			if( this.IsMinecartIconHovering ) { return; }
			if( !mouse_comp.IsMouseHovering ) { return; }
			if( train_comp.IsMountedBy != -1 ) { return; }
			if( !train_comp.OwnsMe( Main.LocalPlayer ) ) { return; }

			var text_pos = new Vector2(
				Main.mouseX + 16,
				Main.mouseY + 16
			);
			var pos = new Vector2(
				Main.mouseX - this.TrainIcon.Width,
				Main.mouseY - this.TrainIcon.Height
			);
			float scale = 1.5f + ( ( this.PulseScaleAnimation > 0 ? this.PulseScaleAnimation : -this.PulseScaleAnimation ) / 240f );
			SpriteEffects dir = DrawsInGameEntityComponent.GetOrientation( ent.Core );

			if( this.PulseScaleAnimation >= 26 ) {
				this.PulseScaleAnimation = -this.PulseScaleAnimation;
			} else {
				this.PulseScaleAnimation++;
			}
			
			sb.Draw( this.TrainIcon, pos, null, Color.White, 0f, default( Vector2 ), scale, dir, 1f );

			HudHelpers.DrawGlowingString( ent.Core.DisplayName, text_pos, 1f );
		}
	}
}
