using HamstarHelpers.Components.Config;
using Microsoft.Xna.Framework.Graphics;
using OnARail.CustomEntities;
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



		////////////////

		public JsonConfig<OnARailConfigData> ConfigJson { get; private set; }
		public OnARailConfigData Config { get { return this.ConfigJson.Data; } }

		private TrainEntityHandler TrainFactory;


		////////////////

		public OnARailMod() {
			this.Properties = new ModProperties() {
				Autoload = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};

			this.ConfigJson = new JsonConfig<OnARailConfigData>( OnARailConfigData.ConfigFileName,
					ConfigurationDataBase.RelativePath, new OnARailConfigData() );
		}

		////////////////

		public override void Load() {
			OnARailMod.Instance = this;

			this.LoadConfig();

			this.TrainFactory = new TrainEntityHandler();
		}

		private void LoadConfig() {
			if( !this.ConfigJson.LoadFile() ) {
				this.ConfigJson.SaveFile();
				ErrorLogger.Log( "On A Rail config for version " + this.Version.ToString() + " created." );
			}

			if( this.Config.UpdateToLatestVersion( this ) ) {
				ErrorLogger.Log( "On A Rail config updated to " + this.Version.ToString() );
				this.ConfigJson.SaveFile();
			}
		}

		public override void Unload() {
			OnARailMod.Instance = null;
		}


		////////////////

		public override object Call( params object[] args ) {
			if( args.Length == 0 ) { throw new Exception( "Undefined call type." ); }

			string call_type = args[0] as string;
			if( args == null ) { throw new Exception( "Invalid call type." ); }

			var new_args = new object[args.Length - 1];
			Array.Copy( args, 1, new_args, 0, args.Length - 1 );

			return OnARailAPI.Call( call_type, new_args );
		}


		////////////////

		public override void PostDrawInterface( SpriteBatch sb ) {
			// Clients and single only (redundant?)
			if( Main.netMode == 2 ) { return; }

			try {
				if( !Main.mapFullscreen && ( Main.mapStyle == 1 || Main.mapStyle == 2 ) ) {
					this.DrawMiniMapForAll( sb );
				}
			} catch( Exception e ) {
				ErrorLogger.Log( "OnARailMod.PostDrawInterface - " + e.ToString() );
				throw e;
			}
		}

		public override void PostDrawFullscreenMap( ref string mouseText ) {
			// Clients and single only (redundant?)
			if( Main.netMode == 2 ) { return; }

			try {
				this.DrawFullMapForAll( Main.spriteBatch );
			} catch( Exception e ) {
				ErrorLogger.Log( "OnARailMod.PostDrawFullscreenMap: " + e.ToString() );
				throw e;
			}
		}
	}
}
