using System.Collections.Generic;
using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Components.CustomEntity.Components;
using HamstarHelpers.Components.CustomEntity.Templates;
using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.PlayerHelpers;
using HamstarHelpers.Services.Promises;
using Microsoft.Xna.Framework;
using OnARail.Entities.Train.Components;
using Terraria;


namespace OnARail.Entities.Train {
	public partial class TrainEntityHandler {
		public static int TrainEntityID { get; private set; }



		////////////////

		public static int SpawnMyTrain( Player owner ) {
			if( Main.netMode == 1 ) {
				throw new HamstarException( "!OnARail.TrainEntityHandler.SpawnTrain - Cannot spawn on client." );
			}

			var ent = CustomEntityTemplateManager.CreateEntityByID( TrainEntityHandler.TrainEntityID, owner );
			ent.Core.DisplayName = owner.name + "'s Train";

			Vector2 pos = owner.Center;
			pos.Y -= 16;
			pos.X = MathHelper.Clamp( pos.X, 160, ( Main.maxTilesX - 10 ) * 16 );
			pos.Y = MathHelper.Clamp( pos.Y, 160, ( Main.maxTilesY - 10 ) * 16 );

			ent.Core.Center = pos;

			int who = CustomEntityManager.AddEntity( ent );
			
			if( Main.netMode == 2 ) {
				ent.SyncTo();
			}

			if( OnARailMod.Instance.Config.DebugModeInfo ) {
				LogHelpers.Log( "Train entity ("+ent.ToString()+") spawned for "+owner.name );
			}

			return who;
		}


		public static int FindMyTrain( Player player ) {
			if( !SaveableEntityComponent.IsLoaded ) {
				LogHelpers.LogOnce( "OnARail.TrainEntityHandler.FindMyTrain - Entities not loaded." );
				return -1;
			}

			ISet<CustomEntity> ents = CustomEntityManager.GetEntitiesByComponent<TrainBehaviorEntityComponent>();

			foreach( var ent in ents ) {
				if( ent.OwnerPlayerWho == player.whoAmI ) {
					return ent.Core.whoAmI;
				}
			}

			return -1;
		}



		////////////////

		internal TrainEntityHandler() {
			Promises.AddPostModLoadPromise( () => {
				var comps = new List<CustomEntityComponent> {
					new TrainBehaviorEntityComponent(),
					new TrainDrawInGameEntityComponent(),
					new TrainDrawOnMapEntityComponent(),
					new TrainMouseInteractionEntityComponent(),
					new TrainRespectsTerrainEntityComponent(),
					new TrainRespectsGravityEntityComponent(),
					new TrainRailBoundEntityComponent(),
					new TrainPeriodicSyncEntityComponent(),
					new TrainSaveableEntityComponent( OnARailMod.Instance.Config.SaveTrainDataAsJson )
				};

				TrainEntityHandler.TrainEntityID = CustomEntityTemplateManager.Add( "Unnamed Train", 64, 48, comps );
			} );
		}
	}
}
