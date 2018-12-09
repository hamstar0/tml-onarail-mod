using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Components.CustomEntity.Components;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.HudHelpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Terraria;
using Terraria.ID;


namespace OnARail.Entities.Train.Components {
	class TrainDrawInGameEntityComponent : DrawsInGameEntityComponent {
		private class TrainDrawInGameEntityComponentFactory : DrawsInGameEntityComponentFactory<TrainDrawInGameEntityComponent> {
			public TrainDrawInGameEntityComponentFactory( string src_mod_name, string rel_texture_path, int frame_count )
				: base( src_mod_name, rel_texture_path, frame_count ) { }
		}



		////////////////

		public static TrainDrawInGameEntityComponent CreateTrainDrawInGameEntityComponent() {
			var factory = new TrainDrawInGameEntityComponentFactory( "OnARail", "Mounts/TrainMount_Back", 4 );
			return factory.Create();
		}



		////////////////

		[PacketProtocolIgnore]
		[JsonIgnore]
		public bool IsMinecartIconHovering { get; private set; }

		private Texture2D TrainIcon;
		private float PulseScaleAnimation = 0f;



		////////////////

		protected TrainDrawInGameEntityComponent( PacketProtocolDataConstructorLock ctor_lock ) : base( ctor_lock ) { }


		////////////////

		protected override void PostPostInitialize() {
			if( Main.netMode != 2 ) {
				this.TrainIcon = OnARailMod.Instance.GetTexture( "Entities/Train/TrainIcon" );
			}
		}


		////////////////

		public override void Draw( SpriteBatch sb, CustomEntity ent ) {
			var train_comp = ent.GetComponentByType<TrainBehaviorEntityComponent>();
			Player plr = Main.LocalPlayer;

			this.IsMinecartIconHovering = plr.showItemIcon && plr.showItemIcon2 == ItemID.Minecart;

			if( train_comp.IsMountedBy == -1 ) {
				base.Draw( sb, ent );
			}
		}


		public override void PostDraw( SpriteBatch sb, CustomEntity ent ) {
			var train_comp = ent.GetComponentByType<TrainBehaviorEntityComponent>();
			var mouse_comp = ent.GetComponentByType<TrainMouseInteractionEntityComponent>();

			if( this.IsMinecartIconHovering ) { return; }
			if( !mouse_comp.IsMouseHovering ) { return; }
			if( train_comp.IsMountedBy != -1 ) { return; }
			if( ent.MyOwnerPlayerWho != Main.myPlayer ) { return; }

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
			
			sb.Draw( this.TrainIcon, pos, null, Color.White, 0f, default(Vector2), scale, dir, 1f );

			HudHelpers.DrawGlowingString( ent.Core.DisplayName, text_pos, 1f );
		}
	}
}
