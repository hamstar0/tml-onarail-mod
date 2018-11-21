using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Components.CustomEntity.Components;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using Microsoft.Xna.Framework.Graphics;
using Terraria;


namespace OnARail.Entities.Train.Components {
	class TrainDrawOnMapEntityComponent : DrawsOnMapEntityComponent {
		private class TrainDrawInGameEntityComponentFactory : DrawsOnMapEntityComponentFactory<TrainDrawOnMapEntityComponent> {
			public TrainDrawInGameEntityComponentFactory( string src_mod_name, string rel_texture_path, int frame_count, float scale, bool zooms )
				: base( src_mod_name, rel_texture_path, frame_count, scale, zooms ) { }
		}



		////////////////

		public static TrainDrawOnMapEntityComponent CreateTrainDrawOnMapEntityComponent() {
			var factory = new TrainDrawInGameEntityComponentFactory( "OnARail", "Entities/Train/TrainIcon", 1, 1f, false );
			return factory.Create();
		}



		////////////////

		protected TrainDrawOnMapEntityComponent( PacketProtocolDataConstructorLock ctor_lock ) : base( ctor_lock ) { }


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
