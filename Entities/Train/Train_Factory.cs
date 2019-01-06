using System;
using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Helpers.DebugHelpers;
using Terraria;


namespace OnARail.Entities.Train {
	public partial class TrainEntity : CustomEntity {
		protected class TrainEntityFactory<T> : CustomEntityFactory<T> where T : TrainEntity {
			public TrainEntityFactory( Player owner_plr ) : base( owner_plr ) {
				if( owner_plr == null ) {
					throw new NotImplementedException( "Trains must have an owner." );
				}
			}
		}
		
		//protected sealed class MyFactory : TrainEntityFactory<TrainEntity> {
		//	public MyFactory( Player owner_plr ) : base( owner_plr ) { }
		//}



		////////////////

		public static TrainEntity CreateTrainEntity( Player owner_plr ) {
			if( OnARailMod.Instance.Config.DebugModeInfo ) {
				LogHelpers.Log( "Creating new train for player "+owner_plr.name+" ("+owner_plr.whoAmI+")" );
			}

			var factory = new TrainEntityFactory<TrainEntity>( owner_plr );
			return factory.Create();
		}
	}
}
