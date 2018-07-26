using HamstarHelpers.Services.Promises;
using Microsoft.Xna.Framework;
using OnARail.Entities;
using System;
using Terraria;
using Terraria.ModLoader;


namespace OnARail.Mounts {
	class TrainMount : ModMountData {
		public TrainMount() : base() {
			Promises.AddCustomPromiseForObject( DecentralizedPlayerUpdates.Instance, () => {
				this.RunUpdateForPlayer( DecentralizedPlayerUpdates.Instance.MyPlayer );
				return true;
			} );
		}


		public override void SetDefaults() {
			int total_frames = 4;

			var player_y_offsets = new int[ total_frames ];
			for( int i = 0; i < player_y_offsets.Length; i++ ) {
				player_y_offsets[i] = 8+(i%2);
			}

			this.mountData.Minecart = true;
			this.mountData.MinecartDirectional = true;
			this.mountData.MinecartDust = new Action<Vector2>( DelegateMethods.Minecart.Sparks );
			this.mountData.spawnDust = 213;
			this.mountData.buff = this.mod.BuffType( "TrainMountBuff" );
			//this.mountData.extraBuff = 185;
			this.mountData.flightTimeMax = 0;
			this.mountData.fallDamage = 1f;
			this.mountData.runSpeed = 10f;
			this.mountData.dashSpeed = 10f;
			this.mountData.acceleration = 0.03f;
			this.mountData.jumpHeight = 12;
			this.mountData.jumpSpeed = 5.15f;
			this.mountData.blockExtraJumps = true;

			this.mountData.totalFrames = total_frames;
			this.mountData.heightBoost = 10;
			this.mountData.playerYOffsets = player_y_offsets;
			this.mountData.xOffset = 1;
			this.mountData.yOffset = 16;
			this.mountData.bodyFrame = 3;
			this.mountData.playerHeadOffset = 14;

			this.mountData.standingFrameCount = 1;
			this.mountData.standingFrameDelay = 12;
			this.mountData.standingFrameStart = 0;
			this.mountData.runningFrameCount = 3;
			this.mountData.runningFrameDelay = 12;
			this.mountData.runningFrameStart = 0;
			this.mountData.flyingFrameCount = 0;
			this.mountData.flyingFrameDelay = 0;
			this.mountData.flyingFrameStart = 0;
			this.mountData.inAirFrameCount = 0;
			this.mountData.inAirFrameDelay = 0;
			this.mountData.inAirFrameStart = 0;
			this.mountData.idleFrameCount = 0;
			this.mountData.idleFrameDelay = 0;
			this.mountData.idleFrameStart = 0;
			this.mountData.idleFrameLoop = false;

			if( Main.netMode != 2 ) {
				this.mountData.textureWidth = this.mountData.backTexture.Width;
				this.mountData.textureHeight = this.mountData.backTexture.Height;
			}
		}


		////////////////

		public override void UpdateEffects( Player player ) {
			if( Math.Abs( player.velocity.X ) > 4f ) {
				//Rectangle rect = player.getRect();
				//Dust.NewDust( new Vector2( rect.X, rect.Y ), rect.Width, rect.Height, this.mod.DustType( "Smoke" ) );
			}
		}
		

		internal void RunUpdateForPlayer( Player player ) {
			if( player.mount.Active && player.mount.Type == this.Type ) {
				TrainEntityHandler.SetTrainEntityFollowing( player );
			} else {
				TrainEntityHandler.SetTrainEntityStanding( player );
			}
		}
	}
}
