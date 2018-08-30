using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.PlayerHelpers;
using HamstarHelpers.Helpers.TmlHelpers;
using HamstarHelpers.Services.Promises;
using Microsoft.Xna.Framework;
using OnARail.Entities.Train;
using Terraria;
using Terraria.ModLoader;


namespace OnARail {
	partial class OnARailPlayer : ModPlayer {
		public override void PreUpdate() {
			if( this.MyTrainWho == -1 ) {
				this.MyTrainWho = TrainEntityHandler.FindMyTrain( this.player );
			}

			if( this.MyTrainWho != -1 ) {
				this.UpdatePostTrainLoad();
			}

			this.UpdatePlayerState();
		}


		private void UpdatePostTrainLoad() {
			if( this.player.dead ) { return; }

			if( LoadHelpers.IsWorldSafelyBeingPlayed() ) {
				if( Main.netMode != 2 ) {
					this.UpdateTrainWarp();
				}
			}
		}


		private void UpdateTrainWarp() {
			if( Vector2.Distance( this.player.position, PlayerHelpers.GetSpawnPoint( this.player ) ) <= 8 ) {   // is at spawn
				if( Vector2.Distance( this.player.position, this.PrevPosition ) > 16 * 4 ) {    // is 4+ blocks away since prev tick
					this.HandleRecall();
				}
			}

			this.PrevPosition = this.player.position;
		}


		private void UpdatePlayerState() {
			if( this.player.dead ) { return; }

			this.IsInInitLockdown = !LoadHelpers.IsWorldSafelyBeingPlayed();

			if( this.IsInInitLockdown ) {
				PlayerHelpers.LockdownPlayerPerTick( this.player );
			}

			var args = new PlayerPromiseArguments { Who = this.player.whoAmI };

			Promises.TriggerValidatedPromise( OnARailPlayer.PlayerFuncsValidator, OnARailPlayer.MyValidatorKey, args );
		}
	}
}
