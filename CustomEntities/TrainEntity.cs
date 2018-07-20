using System.Collections.Generic;
using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Components.CustomEntity.Components;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Services.Promises;
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
				var pos = new Vector2( Main.mouseX - TrainEntity.TrainIcon.Width, Main.mouseY - TrainEntity.TrainIcon.Height );
				float scale = 1f + ( ( this.PulseScaleAnimation > 0 ? this.PulseScaleAnimation : -this.PulseScaleAnimation ) / 90f );

				if( this.PulseScaleAnimation >= 30 ) {
					this.PulseScaleAnimation = -this.PulseScaleAnimation;
				} else {
					this.PulseScaleAnimation++;
				}

				sb.Draw( TrainEntity.TrainIcon, pos, null, Color.White, 0f, default( Vector2 ), scale, SpriteEffects.None, 1f );
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

					CustomEntityManager.Entities.Remove( ent );
				}
			} else {
				this.IsHovering = player.Distance( ent.Center ) <= TrainEntity.BoardingDistance;
			}
		}
	}




	public class TrainEntity {
		public const float BoardingDistance = 96;

		internal static Texture2D TrainTexture;
		internal static Texture2D TrainIcon;
		private static IList<CustomEntityComponent> Components;


		////////////////

		static TrainEntity() {
			if( Main.netMode != 2 ) {
				Promises.AddPostModLoadPromise( () => {
					TrainEntity.TrainTexture = OnARailMod.Instance.GetTexture( "Mounts/TrainMount_Back" );
					TrainEntity.TrainIcon = OnARailMod.Instance.GetTexture( "CustomEntities/TrainIcon" );
				} );
			}

			Promises.AddPostModLoadPromise( () => {
				TrainEntity.Components = new List<CustomEntityComponent> {
					new TrainDrawEntityComponent(),
					new TrainMouseInteractionEntityComponent(),
					new RespectsTerrainEntityComponent(),
					new RespectsGravityEntityComponent(),
					new RailBoundEntityComponent(),
					new PeriodicSyncEntityComponent(),
					new PerWorldSaveEntityComponent( false )
				};
			} );
		}

		////////////////

		public static int CreateTrain( Vector2 pos ) {
			var ent = new CustomEntity( TrainEntity.Components );
			var draw_comp = (DrawsEntityComponent)ent.GetComponentByType<DrawsEntityComponent>();

			pos.X = MathHelper.Clamp( pos.X, 160, ( Main.maxTilesX - 10 ) * 16 );
			pos.Y = MathHelper.Clamp( pos.Y, 160, ( Main.maxTilesY - 10 ) * 16 );

			ent.position = pos;
			ent.width = TrainEntity.TrainTexture.Width;
			ent.height = ( TrainEntity.TrainTexture.Height / draw_comp.FrameCount ) - 12;

			return CustomEntityManager.Entities.Add( ent );
		}
	}
}
