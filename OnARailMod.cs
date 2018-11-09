using HamstarHelpers.Components.Config;
using HamstarHelpers.Services.Messages;
using HamstarHelpers.Services.Promises;
using OnARail.Entities.Train;
using OnARail.Tiles;
using System;
using System.Linq;
using Terraria.ModLoader;


namespace OnARail {
    public partial class OnARailMod : Mod {
		public static OnARailMod Instance { get; private set; }



		////////////////

		public JsonConfig<OnARailConfigData> ConfigJson { get; private set; }
		public OnARailConfigData Config { get { return this.ConfigJson.Data; } }
		
		internal bool HasBuffPromise = false;
		internal bool HasMountPromise = false;
		


		////////////////

		public OnARailMod() {
			this.ConfigJson = new JsonConfig<OnARailConfigData>( OnARailConfigData.ConfigFileName,
					ConfigurationDataBase.RelativePath, new OnARailConfigData() );
		}

		////////////////

		public override void Load() {
			OnARailMod.Instance = this;

			this.LoadConfig();
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

		public override void PostAddRecipes() {
			var mymod = OnARailMod.Instance;
			if( mymod.Config.ExtensibleInventoryDefaultRestrictedToTrain ) {
				var ei_mod = ModLoader.GetMod( "ExtensibleInventory" );

				if( ei_mod != null ) {
					InboxMessages.SetMessage( "OnARailExtensibleInventoryAlert", "Extensible Inventory is now available from your train. See config settings for options.", false );
				}
			}

			if( mymod.Config.DebugModeReset ) {
				Promises.AddSafeWorldLoadEachPromise( () => {
					foreach( ModTileEntity ent in ModTileEntity.ByID.Values.ToArray() ) {
						if( ent is TrainTunnelTileEntity ) {
							ModTileEntity.ByID.Remove( ent.ID );
							ModTileEntity.ByPosition.Remove( ent.Position );
						}
					}
				} );
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
	}
}
