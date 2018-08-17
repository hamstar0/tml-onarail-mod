using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Services.Promises;
using OnARail.Entities;
using Terraria;


namespace OnARail.NetProtocols {
	class TrainSpawnProtocol : PacketProtocol {
		private TrainSpawnProtocol( PacketProtocolDataConstructorLock ctor_lock ) { }

		////////////////

		protected override void SetServerDefaults( int to_who ) { }
		
		////////////////

		protected override bool ReceiveRequestWithServer( int from_who ) {
			Player player = Main.player[ from_who ];
			var myplayer = player.GetModPlayer<OnARailPlayer>();

			Promises.AddPostWorldLoadOncePromise( () => {
				if( TrainEntityHandler.FindMyTrain( player ) == -1 ) {
					throw new HamstarException( "Cannot spawn duplicate train for player "+player.name );
				}

				TrainEntityHandler.SpawnMyTrain( player );
			} );

			return true;
		}
	}
}
