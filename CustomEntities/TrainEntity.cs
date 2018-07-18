using System.Collections.Generic;
using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Components.CustomEntity.Components;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Services.Promises;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OnARail.Buffs;
using Terraria;
using Terraria.ModLoader.IO;


namespace OnARail.CustomEntities {
	class TrainEntity : CustomEntity {
		private static IList<CustomEntityComponent> MyProperties = new List<CustomEntityComponent> {
			new RespectsTerrainEntityComponent(),
			new RespectsGravityEntityComponent(),
			new RailBoundEntityComponent()
		};

		public const float BoardingDistance = 96;

		private static Texture2D Tex;


		////////////////

		static TrainEntity() {
			if( Main.netMode != 2 ) {
				Promises.AddPostModLoadPromise( () => {
					TrainEntity.Tex = OnARailMod.Instance.GetTexture( "Mounts/TrainMount_Back" );
				} );
			}
		}


		////////////////

		internal static TrainEntity LoadAs( TagCompound tags, int which ) {
			if( !tags.ContainsKey("train_pos_x_" + which) ) { return null; }

			float x = tags.GetFloat( "train_pos_x_" + which );
			float y = tags.GetFloat( "train_pos_y_" + which );
			var pos = new Vector2( x, y );

			return new TrainEntity( pos );
		}

		internal static void SaveAs( TrainEntity train_ent, TagCompound tags, int which ) {
			tags[ "train_pos_x_"+which ] = train_ent.position.X;
			tags[ "train_pos_y_"+which ] = train_ent.position.Y;
		}


		////////////////

		public static void Spawn( Vector2 pos ) {
			var ent = new TrainEntity( pos );

			CustomEntityManager.Entities.Add( ent );
		}



		////////////////

		public override string DisplayName { get { return "Clockwork Train"; } }
		public override Texture2D Texture { get { return TrainEntity.Tex; } }
		public override int FrameCount { get { return 4; } }

		protected override IList<CustomEntityComponent> _OrderedComponents { get { return TrainEntity.MyProperties; } }

		private bool IsHovering = false;


		////////////////

		public TrainEntity( Vector2 pos ) : base(true) {
			pos.X = MathHelper.Clamp( pos.X, 160, ( Main.maxTilesX - 10 ) * 16 );
			pos.Y = MathHelper.Clamp( pos.Y, 160, ( Main.maxTilesY - 10 ) * 16 );

			this.position = pos;
			this.width = TrainEntity.Tex.Width;
			this.height = (TrainEntity.Tex.Height / this.FrameCount) - 12;
		}


		////////////////

		public override void OnMouseHover() {
			Player player = Main.LocalPlayer;
			
			if( Main.mouseLeft ) {
				if( this.IsHovering ) {
					player.position = this.position;
					player.AddBuff( OnARailMod.Instance.BuffType<TrainMountBuff>(), 60 );

					CustomEntityManager.Entities.Remove( this );
				}
			} else {
				this.IsHovering = player.Distance( this.Center ) <= TrainEntity.BoardingDistance;
			}
		}


		////////////////

		public override void PostDraw( SpriteBatch sb ) {
			//if( this.IsHovering ) { }
		}
	}
}
