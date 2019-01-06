using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Components.CustomEntity.Components;
using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Services.Promises;
using OnARail.Entities.Train;
using Terraria;


namespace OnARail.NetProtocols {
	class TrainSpawnProtocol : PacketProtocolRequestToServer {
		protected TrainSpawnProtocol( PacketProtocolDataConstructorLock ctor_lock ) : base( ctor_lock ) { }

		////////////////

		protected override void InitializeServerSendData( int to_who ) { }
		
		////////////////

		protected override bool ReceiveRequestWithServer( int from_who ) {
			Player player = Main.player[ from_who ];

			Promises.AddValidatedPromise( SaveableEntityComponent.LoadAllValidator, () => {
				if( TrainEntity.FindMyTrain( player ) != -1 ) {
					throw new HamstarException( "Cannot spawn duplicate train for player "+player.name );
				}

				var ent = TrainEntity.CreateTrainEntity( player );
				CustomEntityManager.AddToWorld( ent );

				return false;
			} );

			return true;
		}

		protected override void ReceiveReply() {
			throw new System.NotImplementedException();
		}
	}
}
