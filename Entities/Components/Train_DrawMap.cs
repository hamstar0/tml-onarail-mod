using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Components.CustomEntity.Components;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using Microsoft.Xna.Framework.Graphics;
using Terraria;


namespace OnARail.Entities.Components {
	class TrainDrawOnMapEntityComponent : DrawsOnMapEntityComponent {
		private TrainDrawOnMapEntityComponent( PacketProtocolDataConstructorLock ctor_lock ) : this() { }

		public TrainDrawOnMapEntityComponent() : base( "OnARail", "Entities/TrainIcon", 1, 0.25f, false ) { }


		////////////////

		public override bool PreDrawFullscreenMap( SpriteBatch sb, CustomEntity ent ) {
			var train_comp = ent.GetComponentByType<TrainBehaviorEntityComponent>();

			return train_comp.OwnsMe( Main.LocalPlayer );
		}

		public override bool PreDrawMiniMap( SpriteBatch sb, CustomEntity ent ) {
			var train_comp = ent.GetComponentByType<TrainBehaviorEntityComponent>();

			return train_comp.OwnsMe( Main.LocalPlayer );
		}

		public override bool PreDrawOverlayMap( SpriteBatch sb, CustomEntity ent ) {
			var train_comp = ent.GetComponentByType<TrainBehaviorEntityComponent>();

			return train_comp.OwnsMe( Main.LocalPlayer );
		}
	}
}
