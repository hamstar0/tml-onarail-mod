using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.PlayerHelpers;
using Newtonsoft.Json;
using Terraria;


namespace OnARail.Entities {
	class TrainBehaviorEntityComponent : CustomEntityComponent {
		[JsonIgnore]
		public int IsMountedBy = -1;

		public string OwnerUID;



		////////////////

		public override CustomEntityComponent Clone() {
			return (TrainBehaviorEntityComponent)this.MemberwiseClone();
		}

		////////////////

		private void UpdateMe( CustomEntity ent ) {
			if( this.IsMountedBy != -1 ) {
				Player player = Main.player[ this.IsMountedBy ];

				this.UpdateMounted( ent, player );
			}
		}

		public override void UpdateSingle( CustomEntity ent ) {
			this.UpdateMe( ent );
		}
		public override void UpdateClient( CustomEntity ent ) {
			this.UpdateMe( ent );
		}
		public override void UpdateServer( CustomEntity ent ) {
			this.UpdateMe( ent );
		}

		private void UpdateMounted( CustomEntity ent, Player player ) {
			if( player == null || !player.active || player.dead ) {
				if( this.IsMountedBy == player.whoAmI ) {
					this.SetTrainEntityStanding_NoSync( ent, player );
				}
				return;
			} else {
				ent.Core.Center = player.Center;
			}
		}


		////////////////

		public bool SetTrainEntityFollowing_NoSync( CustomEntity ent, Player player ) {
			if( this.IsMountedBy != -1 ) {
				return false;
			}

			var mymod = OnARailMod.Instance;

			this.IsMountedBy = player.whoAmI;

			//player.Center = ent.Center;
			player.MountedCenter = ent.Core.Center;
			player.position.Y -= 22f;

			return true;
		}


		public bool SetTrainEntityStanding_NoSync( CustomEntity ent, Player player ) {
			if( this.IsMountedBy == -1 ) {
				return false;
			}
			
			var mymod = OnARailMod.Instance;

			this.IsMountedBy = -1;

			ent.Core.Center = player.Center;
			ent.Core.position.Y -= 16;
			ent.Core.direction = player.direction;

			player.position.Y -= 12;

			return true;
		}


		////////////////

		public bool IsLocallyOwned( CustomEntity ent ) {
			bool success;

			if( string.IsNullOrEmpty( this.OwnerUID ) ) {
				return true;
			}

			string uid = PlayerIdentityHelpers.GetUniqueId( Main.LocalPlayer, out success );
			if( !success ) {
				return false;
			}

			return this.OwnerUID == uid;
		}
	}
}
