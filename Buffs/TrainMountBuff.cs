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
			this.Description.SetDefault( "Your home away from home." );

			Main.buffNoTimeDisplay[ this.Type ] = true;
			Main.buffNoSave[ this.Type ] = true;

			if( !this.HasPromise ) {
				this.HasPromise = true;

				Promises.AddValidatedPromise( PlayerPromiseValidator.RunAll, () => {
					this.RunUpdateForPlayer( PlayerPromiseValidator.RunAll.MyPlayer );
					return true;
				} );
			}
		}


		////////////////

		internal void RunUpdateForPlayer( Player player ) {
			bool has_buff = player.FindBuffIndex( this.Type ) != -1;
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
				if( player.mount.Active && player.mount.Type == mount_type ) {
					player.mount.Dismount( player );
				}
			}
		}
	}
}
