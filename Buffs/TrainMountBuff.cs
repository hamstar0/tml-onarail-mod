using HamstarHelpers.Services.Timers;
using OnARail.CustomEntities;
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
			if( player.buffType[buff_idx] != this.Type ) {
				return;
			}
			var myplayer = player.GetModPlayer<OnARailPlayer>();
			int who = player.whoAmI;

			myplayer.MountTrain();
			
			Timers.SetTimer( "TrainMountFor" + who, 3, () => {
				Player player2 = Main.player[ who ];

				if( player2.active || (player2.active && player2.dead) ) {
					TrainEntityHandler.ActivateTrainEntity( player2 );
				}
				return false;
			} );
		}
	}
}
