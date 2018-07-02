using OnARail.Mounts;
using Terraria;
using Terraria.ModLoader;


namespace OnARail.Buffs {
	public class TrainMountBuff : ModBuff {
		public override void SetDefaults() {
			this.DisplayName.SetDefault("Train");
			this.Description.SetDefault("Choo Choo");
			Main.buffNoTimeDisplay[ this.Type ] = true;
			Main.buffNoSave[ this.Type ] = true;
		}

		public override void Update( Player player, ref int buff_idx ) {
			player.mount.SetMount( this.mod.MountType<TrainMount>(), player );
			player.buffTime[ buff_idx ] = 10;
		}
	}
}
