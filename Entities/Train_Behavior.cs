using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Helpers.DebugHelpers;
using Newtonsoft.Json;
using OnARail.Buffs;
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
					this.IsMountedBy = -1;
				}
				return;
			} else {
				ent.position = player.position;
			}
		}


		////////////////

		public bool SetTrainEntityFollowing_NoSync( CustomEntity ent, Player player ) {
			if( this.IsMountedBy != -1 ) {
				return false;
			}

			var mymod = OnARailMod.Instance;

			this.IsMountedBy = player.whoAmI;

			player.position = ent.position;
			player.position.Y -= mymod.GetMount<TrainMount>().mountData.heightBoost;

			return true;
		}


		public bool SetTrainEntityStanding_NoSync( CustomEntity ent ) {
			if( this.IsMountedBy == -1 ) {
				return false;
			}

			this.IsMountedBy = -1;

			return true;
		}
	}
}
