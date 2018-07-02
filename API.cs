using Terraria;


namespace OnARail {
	public static partial class OnARailAPI {
		public static OnARailConfigData GetModSettings() {
			return OnARailMod.Instance.Config;
		}

		public static void SaveModSettingsChanges() {
			OnARailMod.Instance.ConfigJson.SaveFile();
		}
	}
}
