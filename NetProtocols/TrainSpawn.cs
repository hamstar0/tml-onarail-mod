using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Services.Promises;
using Terraria;


namespace OnARail.NetProtocols {
	class TrainSpawnProtocol : PacketProtocol {
		private TrainSpawnProtocol( PacketProtocolDataConstructorLock ctor_lock ) { }

		////////////////

		protected override void SetServerDefaults() { }
		
		////////////////

		protected override bool ReceiveRequestWithServer( int from_who ) {
			Player player = Main.player[ from_who ];
			var myplayer = player.GetModPlayer<OnARailPlayer>();

			Promises.AddPostWorldLoadOncePromise( () => {
				myplayer.SpawnMyTrain();
			} );

			return true;
		}
	}
}
