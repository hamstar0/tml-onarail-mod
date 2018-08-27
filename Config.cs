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

		public int TrainTunnelMinTileRange = 75;
		public int TrainTunnelMaxTileRange = 200;

		public bool ExtensibleInventoryDefaultRestrictedToTrain = true;



		////////////////

		public bool UpdateToLatestVersion( OnARailMod mymod ) {
			var new_config = new OnARailConfigData();
			var vers_since = this._ModVersionSinceUpdate != "" ?
				new Version( this._ModVersionSinceUpdate ) :
				new Version();

			if( vers_since >= mymod.Version ) {
				return false;
			}
			
			this._ModVersionSinceUpdate = mymod.Version.ToString();

			return true;
		}
	}
}
