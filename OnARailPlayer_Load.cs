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
		private void OnConnectHost() {
			Promises.AddPostWorldLoadOncePromise( () => {
				Promises.AddValidatedPromise( SaveableEntityComponent.LoadAllValidator, () => {
					if( TrainEntityHandler.FindMyTrain( this.player ) == -1 ) {
						this.SpawnMyTrain();
					}
					return false;
				} );
			} );
		}
		
		////////////////

		private void OnConnectSingle() {
			this.OnConnectHost();
		}

		private void OnConnectClient() {
			PacketProtocol.QuickRequestToServer<ModSettingsProtocol>();
		}

		private void OnConnectServer() {
			this.OnConnectHost();
		}


		////////////////

		public override void SetupStartInventory( IList<Item> items, bool mediumcore_death ) {
			if( mediumcore_death ) { return; }
			
			if( !this.IsInitializedwithTrain ) {
				this.IsInitializedwithTrain = true;

				var tracks_item = new Item();
				tracks_item.SetDefaults( ItemID.MinecartTrack );
				tracks_item.stack = 999;

				items.Add( tracks_item );
			}
		}
	}
}
