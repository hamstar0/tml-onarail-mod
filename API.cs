using HamstarHelpers.Components.Errors;
using Terraria;


namespace OnARail {
	public static partial class OnARailAPI {
		public static OnARailConfigData GetModSettings() {
			return OnARailMod.Instance.Config;
		}

		public static void SaveModSettingsChanges() {
			if( Main.netMode != 0 ) {
				throw new HamstarException( "Mod settings may only be saved in single player." );
			}

			OnARailMod.Instance.ConfigJson.SaveFile();
		}
	}
}
