using HamstarHelpers.Components.Config;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;


namespace OnARail {
    public partial class OnARailMod : Mod {
		public static OnARailMod Instance { get; private set; }


		public static string GithubUserName { get { return "hamstar0"; } }
		public static string GithubProjectName { get { return "tml-onarail-mod"; } }


		public static string ConfigFileRelativePath {
			get { return ConfigurationDataBase.RelativePath + Path.DirectorySeparatorChar + OnARailConfigData.ConfigFileName; }
		}

		public static void ReloadConfigFromFile() {
			if( Main.netMode != 0 ) {
				throw new Exception( "Cannot reload configs outside of single player." );
			}
			if( !OnARailMod.Instance.ConfigJson.LoadFile() ) {
				OnARailMod.Instance.ConfigJson.SaveFile();
			}
		}

		public static void ResetConfigFromDefaults() {
			if( Main.netMode != 0 ) {
				throw new Exception( "Cannot reset to default configs outside of single player." );
			}

			var config_data = new OnARailConfigData();
			//config_data.SetDefaults();

			OnARailMod.Instance.ConfigJson.SetData( config_data );
			OnARailMod.Instance.ConfigJson.SaveFile();
		}
	}
}
