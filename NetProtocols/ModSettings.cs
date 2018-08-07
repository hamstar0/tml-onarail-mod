using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;


namespace OnARail.NetProtocols {
	class ModSettingsProtocol : PacketProtocol {
		public OnARailConfigData Data;


		////////////////

		private ModSettingsProtocol( PacketProtocolDataConstructorLock ctor_lock ) { }

		////////////////

		protected override void SetServerDefaults() {
			this.Data = (OnARailConfigData)OnARailMod.Instance.Config.Clone();
		}

		////////////////

		protected override void ReceiveWithClient() {
			var mymod = OnARailMod.Instance;

			mymod.ConfigJson.SetData( this.Data );

			if( mymod.Config.DebugModeInfo ) {
				LogHelpers.Log( "OnARailMod.NetProtocols.ModSettingsProtocol.ReceiveWithClient - " + this.Data.ToString() );
			}
		}
	}
}
