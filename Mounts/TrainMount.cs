using HamstarHelpers.Components.CustomEntity.Components;
using HamstarHelpers.Services.Promises;
using Microsoft.Xna.Framework;
using OnARail.Entities;
using System;
using Terraria;
using Terraria.ModLoader;


namespace OnARail.Mounts {
	class TrainMount : ModMountData {
		private bool HasPromise = false;


		////////////////

		public override void SetDefaults() {
			int total_frames = 4;

			var player_y_offsets = new int[ total_frames ];
			for( int i = 0; i < player_y_offsets.Length; i++ ) {
				player_y_offsets[i] = 32;
			}

			this.mountData.Minecart = true;
			this.mountData.MinecartDirectional = true;
			this.mountData.MinecartDust = new Action<Vector2>( DelegateMethods.Minecart.Sparks );
			this.mountData.spawnDust = 213;
			this.mountData.buff = this.mod.BuffType( "TrainMountBuff" );
			//this.mountData.extraBuff = 185;
			this.mountData.flightTimeMax = 0;
			this.mountData.fallDamage = 1f;
			this.mountData.runSpeed = 20f;	//10f;
			this.mountData.dashSpeed = 10f;
			this.mountData.acceleration = 0.05f;	//0.03f;
			this.mountData.jumpHeight = 6;	//12;
			this.mountData.jumpSpeed = 5.15f;
			this.mountData.blockExtraJumps = true;

			this.mountData.totalFrames = total_frames;
			this.mountData.heightBoost = 30;
			this.mountData.playerYOffsets = player_y_offsets;
			this.mountData.xOffset = 8;
			this.mountData.yOffset = 2;
			this.mountData.bodyFrame = 3;
			this.mountData.playerHeadOffset = 14;

			this.mountData.standingFrameCount = 1;
			this.mountData.standingFrameDelay = 12;
			this.mountData.standingFrameStart = 0;
			this.mountData.runningFrameCount = 4;
			this.mountData.runningFrameDelay = 12;
			this.mountData.runningFrameStart = 0;
			this.mountData.flyingFrameCount = 0;
			this.mountData.flyingFrameDelay = 0;
			this.mountData.flyingFrameStart = 0;
			this.mountData.inAirFrameCount = 0;
			this.mountData.inAirFrameDelay = 0;
			this.mountData.inAirFrameStart = 0;
			this.mountData.idleFrameCount = 4;
			this.mountData.idleFrameDelay = 12;
			this.mountData.idleFrameStart = 0;
			this.mountData.idleFrameLoop = false;

			if( Main.netMode != 2 ) {
				this.mountData.textureWidth = this.mountData.backTexture.Width;
				this.mountData.textureHeight = this.mountData.backTexture.Height;
			}

			if( !this.HasPromise ) {
				this.HasPromise = true;

				Promises.AddCustomPromiseForObject( DecentralizedPlayerUpdates.Instance, () => {
					this.RunUpdateForPlayer( DecentralizedPlayerUpdates.Instance.MyPlayer );
					return true;
				} );
			}
		}


		////////////////
		
		internal void RunUpdateForPlayer( Player player ) {
			if( !PerWorldSaveEntityComponent.IsLoaded ) { return; }

			var myplayer = player.GetModPlayer<OnARailPlayer>();
			if( !myplayer.IsLoaded ) { return; }

			if( player.mount.Active && player.mount.Type == this.Type ) {
				TrainEntityHandler.SetTrainEntityFollowing( player );
				
				if( player.direction > 0 ) {
					this.mountData.xOffset = 8;
				} else {
					this.mountData.xOffset = -8;
				}
			} else {
				TrainEntityHandler.SetTrainEntityStanding( player );
			}
		}
	}
}
