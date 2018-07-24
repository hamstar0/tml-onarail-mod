using HamstarHelpers.Components.Network;
using HamstarHelpers.Helpers.DebugHelpers;
using Terraria;


namespace OnARail.NetProtocols {
	class ModSettingsProtocol : PacketProtocol {
		public OnARailConfigData Data;


		////////////////

		public override void SetServerDefaults() {
			this.Data = OnARailMod.Instance.Config;
		}

		protected override void ReceiveWithClient() {
			var mymod = OnARailMod.Instance;

			mymod.ConfigJson.SetData( this.Data );

			if( mymod.Config.DebugModeInfo ) {
				LogHelpers.Log( "OnARailMod.NetProtocols.ModSettingsProtocol.ReceiveWithClient - " + this.Data.ToString() );
			}

			Player player = Main.LocalPlayer;
			var myplayer = player.GetModPlayer<OnARailPlayer>();

			//myplayer.FinishModSettingsSync();
		}
	}
}
