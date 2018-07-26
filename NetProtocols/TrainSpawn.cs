using HamstarHelpers.Components.Network;
using Terraria;


namespace OnARail.NetProtocols {
	class TrainSpawnProtocol : PacketProtocol {
		public override void SetServerDefaults() { }


		////////////////

		protected override bool ReceiveRequestWithServer( int from_who ) {
			Player player = Main.player[ from_who ];
			var myplayer = player.GetModPlayer<OnARailPlayer>();

			myplayer.SpawnMyTrain();

			return true;
		}
	}
}
