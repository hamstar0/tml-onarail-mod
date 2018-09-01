using HamstarHelpers.Components.Config;
using System;


namespace OnARail {
	public class OnARailConfigData : ConfigurationDataBase {
		public readonly static string ConfigFileName = "On A Rail Config.json";


		////////////////

		public string _ModVersionSinceUpdate = "";

		public bool DebugModeInfo = false;
		public bool DebugModeReset = false;
		public bool DebugModeEnableTrainRespawnCommand = true;
		public bool DebugModeTunnelTester = false;

		public bool NewPlayerStarterKit = true;

		public bool SaveTrainDataAsJson = true;
		public bool MerchantSellsRails = true;

		public int TrainAddedDefenseBase = 20;

		public bool CraftableTrainTunnel = true;

		public int TrainTunnelMinTileRange = 50;
		public int TrainTunnelMaxTileRange = 125;

		public bool ExtensibleInventoryDefaultRestrictedToTrain = true;



		////////////////

		private static int _1_2_0_TrainTunnelMinTileRange = 75;
		private static int _1_2_0_TrainTunnelMaxTileRange = 200;



		////////////////

		public bool UpdateToLatestVersion( OnARailMod mymod ) {
			var new_config = new OnARailConfigData();
			var vers_since = this._ModVersionSinceUpdate != "" ?
				new Version( this._ModVersionSinceUpdate ) :
				new Version();

			if( vers_since >= mymod.Version ) {
				return false;
			}

			if( vers_since < new Version(1,2,1) ) {
				if( this.TrainTunnelMinTileRange == OnARailConfigData._1_2_0_TrainTunnelMinTileRange ) {
					this.TrainTunnelMinTileRange = new_config.TrainTunnelMinTileRange;
				}
				if( this.TrainTunnelMaxTileRange == OnARailConfigData._1_2_0_TrainTunnelMaxTileRange ) {
					this.TrainTunnelMaxTileRange = new_config.TrainTunnelMaxTileRange;
				}
			}

			this._ModVersionSinceUpdate = mymod.Version.ToString();

			return true;
		}
	}
}
