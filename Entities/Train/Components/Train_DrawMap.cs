using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Components.CustomEntity.Components;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using Microsoft.Xna.Framework.Graphics;
using Terraria;


namespace OnARail.Entities.Train.Components {
	class TrainDrawOnMapEntityComponent : DrawsOnMapEntityComponent {
		private TrainDrawOnMapEntityComponent( PacketProtocolDataConstructorLock ctor_lock ) : this() { }

		public TrainDrawOnMapEntityComponent() : base( "OnARail", "Entities/Train/TrainIcon", 1, 1f, false ) { }


		////////////////

		public override bool PreDrawFullscreenMap( SpriteBatch sb, CustomEntity ent ) {
			return ent.OwnerPlayerWho == Main.myPlayer;
		}

		public override bool PreDrawMiniMap( SpriteBatch sb, CustomEntity ent ) {
			return ent.OwnerPlayerWho == Main.myPlayer;
		}

		public override bool PreDrawOverlayMap( SpriteBatch sb, CustomEntity ent ) {
			return ent.OwnerPlayerWho == Main.myPlayer;
		}
	}
}
