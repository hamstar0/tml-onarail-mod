using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Helpers.HudHelpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OnARail.CustomEntities;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace OnARail {
    public partial class OnARailMod : Mod {
		private void DrawMiniMapForAll( SpriteBatch sb ) {
			ISet<CustomEntity> trains = CustomEntityManager.Entities.GetByComponentType<TrainDrawEntityComponent>();

			foreach( var train in trains ) {
				if( Main.mapStyle == 1 ) {
					this.DrawMiniMap( sb, train );
				} else {
					this.DrawOverlayMap( sb, train );
				}
			}
		}


		private void DrawFullMapForAll( SpriteBatch sb ) {
			ISet<CustomEntity> trains = CustomEntityManager.Entities.GetByComponentType<TrainDrawEntityComponent>();

			foreach( var train in trains ) {
				this.DrawFullscreenMap( sb, train );
			}
		}


		////////////////

		public void DrawMiniMap( SpriteBatch sb, CustomEntity train_ent ) {
			float scale = Main.mapMinimapScale;
			Texture2D tex = TrainEntity.TrainIcon;

			var rect = new Rectangle( (int)train_ent.position.X, (int)train_ent.position.Y, tex.Width, tex.Height );

			Vector2? mini_map_pos = HudMapHelpers.GetMiniMapPosition( rect );
			if( mini_map_pos != null ) {
				sb.Draw( tex, (Vector2)mini_map_pos, null, Color.White, 0f, new Vector2(), scale, SpriteEffects.None, 1f );
			}
		}

		public void DrawOverlayMap( SpriteBatch sb, CustomEntity train_ent ) {
			float scale = Main.mapOverlayScale;
			Texture2D tex = TrainEntity.TrainIcon;

			var rect = new Rectangle( (int)train_ent.position.X, (int)train_ent.position.Y, tex.Width, tex.Height );

			Vector2? over_map_pos = HudMapHelpers.GetOverlayMapPosition( rect );
			if( over_map_pos != null ) {
				sb.Draw( tex, (Vector2)over_map_pos, null, Color.White, 0f, new Vector2(), scale, SpriteEffects.None, 1f );
			}
		}

		public void DrawFullscreenMap( SpriteBatch sb, CustomEntity train_ent ) {
			float scale = Main.mapFullscreenScale;
			Texture2D tex = TrainEntity.TrainIcon;

			var rect = new Rectangle( (int)train_ent.position.X, (int)train_ent.position.Y, tex.Width, tex.Height );
			Vector2 pos = HudMapHelpers.GetFullMapPosition( rect );
			sb.Draw( tex, pos, null, Color.White, 0f, default( Vector2 ), scale, SpriteEffects.None, 1f );
		}
	}
}
