using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.PlayerHelpers;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using OnARail.Mounts;
using Terraria;


namespace OnARail.Entities.Components {
	class TrainBehaviorEntityComponent : CustomEntityComponent {
		[JsonIgnore]
		[PacketProtocolIgnore]
		internal int IsMountedBy = -1;

		[JsonIgnore]
		[PacketProtocolIgnore]
		private int OwnerWho = -1;

		public string OwnerUID = "";



		////////////////

		private TrainBehaviorEntityComponent( PacketProtocolDataConstructorLock ctor_lock ) : this() { }

		public TrainBehaviorEntityComponent() {
			this.ConfirmLoad();
		}


		////////////////

		public override void UpdateSingle( CustomEntity myent ) {
			if( this.OwnerWho == Main.myPlayer ) {
				this.UpdateMe( myent, Main.LocalPlayer );
			}
		}

		public override void UpdateClient( CustomEntity myent ) {
			if( this.OwnerWho != -1 ) {
				Player plr = Main.player[this.OwnerWho];

				if( plr != null && plr.active ) {
					this.UpdateMe( myent, plr );
				}
			}
		}

		public override void UpdateServer( CustomEntity myent ) {
			if( this.OwnerWho != -1 ) {
				Player plr = Main.player[this.OwnerWho];

				if( plr != null && plr.active ) {
					this.UpdateMe( myent, plr );
				}
			}
		}

		////////////////

		private void UpdateMe( CustomEntity myent, Player player ) {
			if( player.mount.Active && player.mount.Type == OnARailMod.Instance.MountType<TrainMount>() ) {
				if( this.IsMountedBy == -1 ) {
					this.SetTrainEntityFollowing_NoSync( myent, player );
				}
			} else {
				if( this.IsMountedBy != -1 ) {
					this.SetTrainEntityStanding_NoSync( myent, player );
				}
			}

			if( this.IsMountedBy != -1 ) {
				if( this.IsMountedBy == player.whoAmI ) {
					if( !player.active || player.dead ) {
						this.SetTrainEntityStanding_NoSync( myent, player );  // failsafe
					} else {
						myent.Core.Center = player.MountedCenter + new Vector2(0, 22);	// Follows dumbly while inactive
					}
				}
			}
		}


		////////////////

		public bool SetTrainEntityFollowing_NoSync( CustomEntity ent, Player player ) {
			var mymod = OnARailMod.Instance;

			this.IsMountedBy = player.whoAmI;
			
			player.MountedCenter = ent.Core.Center;
			player.position.Y -= 22f;

			return true;
		}


		public bool SetTrainEntityStanding_NoSync( CustomEntity ent, Player player ) {
			var mymod = OnARailMod.Instance;

			this.IsMountedBy = -1;

			player.position.Y -= 12;

			ent.Core.Center = player.Center;
			ent.Core.position.Y -= 16;
			ent.Core.direction = player.direction;

			if( player.whoAmI == Main.myPlayer && Main.netMode == 1 ) {   // needed because player mounts aren't synced to server
				ent.SyncTo();
			}

			return true;
		}


		////////////////

		public bool OwnsMe( Player player ) {
			if( this.OwnerWho != -1 ) {
				Player whoplr = Main.player[ this.OwnerWho ];
				
				if( whoplr == null || !whoplr.active ) {
					this.OwnerWho = -1;
				} else {
					return player.whoAmI == this.OwnerWho;
				}
			}

			bool success;

			if( string.IsNullOrEmpty( this.OwnerUID ) ) {
				return true;
			}

			string uid = PlayerIdentityHelpers.GetUniqueId( player, out success );
			if( !success ) {
				return false;
			}

			if( this.OwnerUID == uid ) {
				this.OwnerWho = player.whoAmI;

				return true;
			}

			return false;
		}
	}
}
