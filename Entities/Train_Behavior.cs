using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Helpers.DebugHelpers;
using Newtonsoft.Json;
using OnARail.Mounts;
using Terraria;


namespace OnARail.Entities {
	class TrainBehaviorEntityComponent : CustomEntityComponent {
		[JsonIgnore]
		public int IsMountedBy = -1;

		public string OwnerUID;



		////////////////

		public override void Update( CustomEntity ent ) {
			if( this.IsMountedBy != -1 ) {
				Player player = Main.player[ this.IsMountedBy ];

				this.UpdateMounted( ent, player );
			}
		}
		
		private void UpdateMounted( CustomEntity ent, Player player ) {
			if( player == null || !player.active || player.dead ) {
				if( this.IsMountedBy == player.whoAmI ) {
					this.SetTrainEntityStanding_NoSync( ent, player );
				}
				return;
			} else {
				ent.Center = player.Center;
			}
		}


		////////////////

		public bool SetTrainEntityFollowing_NoSync( CustomEntity ent, Player player ) {
			if( this.IsMountedBy != -1 ) {
				return false;
			}

			var mymod = OnARailMod.Instance;

			this.IsMountedBy = player.whoAmI;

			player.Center = ent.Center;
			player.position.Y -= 2;
			//player.MountedCenter = ent.Center;
			//player.position.Y -= mymod.GetMount<TrainMount>().mountData.heightBoost;

			return true;
		}


		public bool SetTrainEntityStanding_NoSync( CustomEntity ent, Player player ) {
			if( this.IsMountedBy == -1 ) {
				return false;
			}

			this.IsMountedBy = -1;

			ent.Center = player.Center;

			return true;
		}
	}
}
