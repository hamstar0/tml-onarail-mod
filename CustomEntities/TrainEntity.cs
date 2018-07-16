using System.Collections.Generic;
using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Components.CustomEntity.Properties;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Services.Promises;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader.IO;


namespace OnARail.CustomEntities {
	class TrainEntity : CustomEntity {
		private static IList<CustomEntityProperty> MyProperties = new List<CustomEntityProperty> {
			new RespectsTerrainEntityProperty(),
			new RespectsGravityEntityProperty(),
			new RailBoundEntityProperty()
		};

		private static Texture2D Tex;
		private static int FrameCount = 3;
		private static int Width = 64;
		private static int Height = 64;


		////////////////

		static TrainEntity() {
			if( Main.netMode != 2 ) {
				Promises.AddPostModLoadPromise( () => {
					TrainEntity.Tex = OnARailMod.Instance.GetTexture( "Mounts/TrainMount_Back" );
					TrainEntity.Width = TrainEntity.Tex.Width;
					TrainEntity.Height = TrainEntity.Tex.Height / TrainEntity.FrameCount;
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

		public override string DisplayName {
			get {
				return "Clockwork Train";
			}
		}

		public override Texture2D Texture {
			get {
				return TrainEntity.Tex;
			}
		}

		protected override IList<CustomEntityProperty> _OrderedProperties {
			get {
				return TrainEntity.MyProperties;
			}
		}


		////////////////

		public TrainEntity( Vector2 pos ) : base(true) {
			pos.X = MathHelper.Clamp( pos.X, 160, ( Main.maxTilesX - 10 ) * 16 );
			pos.Y = MathHelper.Clamp( pos.Y, 160, ( Main.maxTilesY - 10 ) * 16 );

			this.position = pos;
			this.width = TrainEntity.Width;
			this.height = TrainEntity.Height;
		}


		////////////////

		public override void PostDraw( SpriteBatch sb ) {
			Dust.NewDust( this.position, this.width, this.height, 15, 0, 0, 150, Color.White, 1f );
		}
	}
}
