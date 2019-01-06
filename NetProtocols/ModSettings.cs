using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;


namespace OnARail.NetProtocols {
	class ModSettingsProtocol : PacketProtocolRequestToServer {
		public OnARailConfigData Data;


		////////////////

		protected ModSettingsProtocol( PacketProtocolDataConstructorLock ctor_lock ) : base( ctor_lock ) { }

		////////////////

		protected override void InitializeServerSendData( int to_who ) {
			this.Data = (OnARailConfigData)OnARailMod.Instance.Config.Clone();
		}

		////////////////
		
		protected override void ReceiveReply() {
			var mymod = OnARailMod.Instance;

			mymod.ConfigJson.SetData( this.Data );
		}
	}
}
