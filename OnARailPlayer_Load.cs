using System.Collections.Generic;
using HamstarHelpers.Components.CustomEntity.Components;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Services.Promises;
using OnARail.Entities;
using OnARail.NetProtocols;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace OnARail {
	partial class OnARailPlayer : ModPlayer {
		private void ReclaimTrain() {
			int id = TrainEntityHandler.FindMyTrain( this.player );

			if( id == -1 ) {
				LogHelpers.Log( "OnARail.OnARailPlayer.ReclaimTrain - No " + this.player.name + " train found." );
				Main.NewText( "Your train is gone. Sorry." );
			} else {
				this.MyTrainID = id;
				TrainEntityHandler.WarpPlayerToTrain( player );
			}
		}

		////////////////

		private void OnConnectSingle() {
			if( !this.IsInitializedwithTrain ) {
				this.IsInitializedwithTrain = true;
				
				this.SpawnMyTrain();
			} else {
				this.ReclaimTrain();
			}
		}

		private void OnConnectClient() {
			PacketProtocol.QuickRequestToServer<ModSettingsProtocol>();

			if( !this.IsInitializedwithTrain ) {
				this.IsInitializedwithTrain = true;
				
				PacketProtocol.QuickRequestToServer<TrainSpawnProtocol>();
			} else {
				Promises.AddCustomPromiseForObject( SaveableEntityComponent.LoadHook, () => {
					this.ReclaimTrain();
					return false;
				} );
			}
		}

		private void OnConnectServer() {
			//this.IsInitializedwithTrain = true;
		}


		////////////////

		public override void SetupStartInventory( IList<Item> items, bool mediumcore_death ) {
			if( mediumcore_death ) { return; }
			
			if( !this.IsInitializedwithTrain ) {
				var tracks_item = new Item();
				tracks_item.SetDefaults( ItemID.MinecartTrack );
				tracks_item.stack = 999;

				items.Add( tracks_item );
			}
		}
	}
}
