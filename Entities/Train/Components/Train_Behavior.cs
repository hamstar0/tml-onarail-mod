using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.PlayerHelpers;
using HamstarHelpers.Services.Timers;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using OnARail.Mounts;
using OnARail.Tiles;
using Terraria;


namespace OnARail.Entities.Train.Components {
	class TrainBehaviorEntityComponent : CustomEntityComponent {
		[JsonIgnore]
		[PacketProtocolIgnore]
		internal int IsMountedBy = -1;



		////////////////

		private TrainBehaviorEntityComponent( PacketProtocolDataConstructorLock ctor_lock ) : this() { }

		public TrainBehaviorEntityComponent() {
			this.ConfirmLoad();
		}


		////////////////

		public override void UpdateSingle( CustomEntity myent ) {
			if( myent.OwnerPlayerWho == Main.myPlayer ) {
				this.UpdateMe( myent, Main.LocalPlayer );
			}
		}

		public override void UpdateClient( CustomEntity myent ) {
			if( myent.OwnerPlayerWho != -1 && myent.OwnerPlayerWho < Main.player.Length ) {
				Player plr = Main.player[ myent.OwnerPlayerWho ];

				if( plr != null && plr.active ) {
					this.UpdateMe( myent, plr );
				}
			}
		}

		public override void UpdateServer( CustomEntity myent ) {
			if( myent.OwnerPlayerWho != -1 && myent.OwnerPlayerWho < Main.player.Length ) {
				Player plr = Main.player[ myent.OwnerPlayerWho ];

				if( plr != null && plr.active ) {
					this.UpdateMe( myent, plr );
				}
			}
		}

		////////////////

		private void UpdateMe( CustomEntity myent, Player owner ) {
			if( owner.mount.Active && owner.mount.Type == OnARailMod.Instance.MountType<TrainMount>() ) {
				if( this.IsMountedBy == -1 ) {
					this.SetTrainEntityFollowing_NoSync( myent, owner );
				}
			} else {
				if( this.IsMountedBy != -1 ) {
					this.SetTrainEntityStanding_NoSync( myent, owner );
				}
			}

			if( this.IsMountedBy != -1 ) {
				if( this.IsMountedBy == owner.whoAmI ) {
					if( !owner.active || owner.dead ) {
						this.SetTrainEntityStanding_NoSync( myent, owner );  // failsafe
					} else {
						myent.Core.Center = owner.MountedCenter + new Vector2(0, 22);	// Follows dumbly while inactive
					}
				}
			}
		}


		////////////////

		public bool SetTrainEntityFollowing_NoSync( CustomEntity ent, Player owner ) {
			var mymod = OnARailMod.Instance;

			this.IsMountedBy = owner.whoAmI;
			
			owner.MountedCenter = ent.Core.Center;
			owner.position.Y -= 22f;

			return true;
		}


		public bool SetTrainEntityStanding_NoSync( CustomEntity ent, Player owner ) {
			var mymod = OnARailMod.Instance;

			this.IsMountedBy = -1;

			owner.position.Y -= 12;

			ent.Core.Center = owner.Center;
			ent.Core.position.Y -= 16;
			ent.Core.direction = owner.direction;

			if( owner.whoAmI == Main.myPlayer && Main.netMode == 1 ) {   // needed because player mounts aren't synced to server...?
				ent.SyncTo();
			}

			return true;
		}


		////////////////

		public void CheckTunnel( CustomEntity train_ent, TrainTunnelTileEntity from_tunnel, TrainTunnelTileEntity to_tunnel ) {
			string timer_name = "TrainTunnelCheck" + train_ent.Core.whoAmI;
			bool is_on_tunnel = from_tunnel.GetWorldRectangle().Intersects( train_ent.Core.Hitbox );

			if( is_on_tunnel ) {
				if( Timers.GetTimerTickDuration( timer_name ) == 0 ) {
					this.TraverseToTunnel( train_ent, to_tunnel );
				}
				Timers.SetTimer( timer_name, 15, () => false );
			}
		}

		private void TraverseToTunnel( CustomEntity train_ent, TrainTunnelTileEntity to_tunnel ) {
			if( this.IsMountedBy == -1 || train_ent.OwnerPlayerWho == -1 ) {
				// TODO train-only traversal
			} else {
				Vector2 to_tunnel_pos = to_tunnel.GetWorldRectangle().Center.ToVector2();
				to_tunnel_pos.Y -= 32;

				Player plr = Main.player[train_ent.OwnerPlayerWho];
				Vector2 vel = plr.velocity;

				PlayerHelpers.Teleport( plr, to_tunnel_pos );
				plr.velocity = vel;
			}
		}
	}
}
