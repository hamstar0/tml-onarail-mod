using HamstarHelpers.Services.Promises;
using OnARail.Mounts;
using Terraria;
using Terraria.ModLoader;


namespace OnARail.Buffs {
	public class TrainMountBuff : ModBuff {
		private bool HasPromise = false;


		////////////////

		public override void SetDefaults() {
			this.DisplayName.SetDefault( "Train" );
			this.Description.SetDefault( "Choo Choo" );

			Main.buffNoTimeDisplay[ this.Type ] = true;
			Main.buffNoSave[ this.Type ] = true;

			if( !this.HasPromise ) {
				this.HasPromise = true;

				Promises.AddCustomPromiseForObject( DecentralizedPlayerUpdates.Instance, () => {
					this.RunUpdateForPlayer( DecentralizedPlayerUpdates.Instance.MyPlayer );
					return true;
				} );
			}
		}


		////////////////

		internal void RunUpdateForPlayer( Player player ) {
			bool has_buff = player.FindBuffIndex( this.Type ) != -1;

			var myplayer = player.GetModPlayer<OnARailPlayer>();
			int who = player.whoAmI;
			int mount_type = this.mod.MountType<TrainMount>();
			
			if( has_buff ) {
				if( !player.mount.Active ) {
					player.mount.SetMount( mount_type, player );
				} else {
					if( player.mount.Type != mount_type ) {
						player.ClearBuff( this.Type );
					}
				}
			} else {
				if( player.mount.Type == mount_type ) {
					player.mount.Dismount( player );
				}
			}
		}
	}
}
