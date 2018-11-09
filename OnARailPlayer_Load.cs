using System.Collections.Generic;
using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Components.CustomEntity.Components;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Services.Promises;
using OnARail.Entities.Train;
using OnARail.Items;
using OnARail.NetProtocols;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace OnARail {
	partial class OnARailPlayer : ModPlayer {
		private void OnConnectHost() {
			Promises.AddPostWorldLoadOncePromise( () => {
				Promises.AddValidatedPromise( SaveableEntityComponent.LoadAllValidator, () => {
					if( TrainEntity.FindMyTrain( this.player ) == -1 ) {
						CustomEntityManager.AddToWorld( new TrainEntity( this.player ) );
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

			var mymod = (OnARailMod)this.mod;
			int tunnel_type = mymod.ItemType<TrainTunnelItem>();
			
			if( !this.IsInitializedwithTrain ) {
				this.IsInitializedwithTrain = true;

				if( mymod.Config.NewPlayerStarterKit ) {
					var tracks_item = new Item();
					var tunnel_item1 = new Item();
					var tunnel_item2 = new Item();
					var tunnel_item3 = new Item();

					tracks_item.SetDefaults( ItemID.MinecartTrack );
					tracks_item.stack = 999;
					tunnel_item1.SetDefaults( tunnel_type );
					tunnel_item2.SetDefaults( tunnel_type );
					tunnel_item3.SetDefaults( tunnel_type );
					
					items.Add( tracks_item );
					items.Add( tunnel_item1 );
					items.Add( tunnel_item2 );
					items.Add( tunnel_item3 );
				}
			}
		}
	}
}
