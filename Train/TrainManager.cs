using System.Collections.Generic;
using Terraria.ModLoader.IO;


namespace OnARail.Train {
	class TrainManager {
		private readonly IDictionary<string, TrainEntity> Trains = new Dictionary<string, TrainEntity>();


		////////////////

		public void Load( TagCompound tags ) {
			this.Trains.Clear();

			if( !tags.ContainsKey( "train_count" ) ) { return; }

			int train_count = tags.GetInt( "train_count" );

			for( int i = 0; i < train_count; i++ ) {
				string uid = tags.GetString( "train_uid_" + i );
				this.Trains[ uid ] = TrainEntity.LoadAs( tags, i );
			}
		}

		public void Save( TagCompound tags ) {
			tags[ "train_count" ] = this.Trains.Count;

			int i = 0;
			foreach( var kv in this.Trains ) {
				tags[ "train_uid_"+i ] = kv.Key;

				TrainEntity.SaveAs( kv.Value, tags, i );

				i++;
			}
		}
	}
}
