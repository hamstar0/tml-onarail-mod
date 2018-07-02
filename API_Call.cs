using System;


namespace OnARail {
	public static partial class OnARailAPI {
		internal static object Call( string call_type, params object[] args ) {
			switch( call_type ) {
			case "GetModSettings":
				return OnARailAPI.GetModSettings();
			case "SaveModSettingsChanges":
				OnARailAPI.SaveModSettingsChanges();
				return null;
			}

			throw new Exception( "No such api call " + call_type );
		}
	}
}
