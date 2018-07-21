using HamstarHelpers.Components.Config;
using System;


namespace OnARail {
	public class OnARailConfigData : ConfigurationDataBase {
		public readonly static string ConfigFileName = "On A Rail Config.json";


		////////////////

		public string _ModVersionSinceUpdate = "";

		public bool DebugModeInfo = false;

		public bool SaveTrainDataAsJson = true;



		//public string _OLD_SETTINGS_BELOW_ = "";



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
