using System.Collections.Generic;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.PlayerHelpers;
using Microsoft.Xna.Framework;
using OnARail.Entities;
using OnARail.Mounts;
using OnARail.NetProtocols;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace OnARail {
	partial class OnARailPlayer : ModPlayer {
		public int MyTrainId { get; private set; }
		
		private bool IsInitialized = false;



		////////////////

		public override bool CloneNewInstances { get { return false; } }

		public override void Initialize() {
			this.MyTrainId = -1;
		}


		////////////////

		public override void Load( TagCompound tags ) {
			if( tags.ContainsKey("is_init") ) {
				this.IsInitialized = tags.GetBool( "is_init" );
			}
		}

		public override TagCompound Save() {
			return new TagCompound { {"is_init", this.IsInitialized} };
		}


		////////////////

		public override void SyncPlayer( int to_who, int from_who, bool new_player ) {
			if( Main.netMode == 2 ) {
				if( to_who == -1 && from_who == this.player.whoAmI ) {
					this.OnEnterWorldForServer();
				}
			}
		}

		public override void OnEnterWorld( Player player ) {
			if( Main.netMode == 0 ) {
				this.OnEnterWorldForSingle();
			} else if( Main.netMode == 1 ) {
				this.OnEnterWorldForClient();
			}
		}


		////////////////

		private void OnEnterWorldForSingle() {
			if( !this.IsInitialized ) {
				this.IsInitialized = true;
				
				this.FinishSetup();
			}
		}

		private void OnEnterWorldForClient() {
			PacketProtocol.QuickRequestToServer<ModSettingsProtocol>();
			if( !this.IsInitialized ) {
				this.IsInitialized = true;

				PacketProtocol.QuickRequestToServer<TrainSpawnProtocol>();
			}
		}

		private void OnEnterWorldForServer() {
			this.IsInitialized = true;
		}


		////////////////

		public override void SetupStartInventory( IList<Item> items, bool mediumcore_death ) {
			if( mediumcore_death ) { return; }
			
			if( !this.IsInitialized ) {
				var tracks_item = new Item();
				tracks_item.SetDefaults( ItemID.MinecartTrack );
				tracks_item.stack = 999;

				items.Add( tracks_item );
			}
		}


		internal void FinishSetup() {
			this.MyTrainId = TrainEntityHandler.SpawnTrain( this.player );
		}


		////////////////

		private Vector2 PrevPosition = default( Vector2 );

		public override void PreUpdate() {
			if( this.player.dead ) { return; }

			if( Vector2.Distance( this.player.position, PlayerHelpers.GetSpawnPoint(this.player) ) <= 8 ) {
				if( Vector2.Distance( this.player.position, this.PrevPosition ) > 16 * 4 ) {
					TrainEntityHandler.WarpPlayer( player );
				}
			}

			this.PrevPosition = this.player.position;
		}

		//public override void OnRespawn( Player player ) {
		//	TrainEntityHandler.WarpPlayer( player );
		//}


		////////////////

		public void MountTrainMount() {
			int mount_type = this.mod.MountType<TrainMount>();

			this.player.mount.SetMount( mount_type, this.player );
		}
	}
}
