using System.Collections.Generic;
using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Components.CustomEntity.Components;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using Microsoft.Xna.Framework;
using OnARail.Entities.Train.Components;
using Terraria;


namespace OnARail.Entities.Train {
	public partial class TrainEntity : CustomEntity {
		protected class TrainEntityFactory<T> : CustomEntityFactory<T> where T : TrainEntity {
			private readonly Player OwnerPlayer;


			////////////////

			public TrainEntityFactory( Player owner_plr ) {
				this.OwnerPlayer = owner_plr;
			}

			////

			protected override IList<CustomEntityComponent> InitializeComponents() {
				return new List<CustomEntityComponent> {
					TrainBehaviorEntityComponent.CreateTrainBehaviorEntityComponent(),
					TrainDrawInGameEntityComponent.CreateTrainDrawInGameEntityComponent(),
					TrainDrawOnMapEntityComponent.CreateTrainDrawOnMapEntityComponent(),
					TrainMouseInteractionEntityComponent.CreateTrainMouseInteractionEntityComponent(),
					TrainRespectsTerrainEntityComponent.CreateTrainRespectsTerrainEntityComponent(),
					TrainRespectsGravityEntityComponent.CreateTrainRespectsGravityEntityComponent(),
					TrainRailBoundEntityComponent.CreateTrainRailBoundEntityComponent(),
					TrainPeriodicSyncEntityComponent.CreateTrainPeriodicSyncEntityComponent(),
					TrainSaveableEntityComponent.CreateTrainSaveableEntityComponent( OnARailMod.Instance.Config.SaveTrainDataAsJson )
				};
			}

			protected override CustomEntityCore InitializeCore() {
				Vector2 pos = this.OwnerPlayer.Center;
				pos.Y -= 16;
				pos.X = MathHelper.Clamp( pos.X, 160, (Main.maxTilesX - 10) * 16 );
				pos.Y = MathHelper.Clamp( pos.Y, 160, (Main.maxTilesY - 10) * 16 );

				var core = new CustomEntityCore( this.OwnerPlayer.name + "'s Train", 64, 48, pos, 1 ) {
					Center = pos
				};

				return core;
			}
			
			protected override void PostInitialize( T ent ) {
				if( Main.netMode == 2 ) {
					ent.SyncTo();
				}
			}
		}



		////////////////

		public static TrainEntity CreateTrainEntity( Player owner_plr ) {
			var factory = new TrainEntityFactory<TrainEntity>( owner_plr );
			return factory.Create();
		}



		////////////////

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
		
		protected TrainEntity( PacketProtocolDataConstructorLock ctor_lock ) : base( ctor_lock ) { }
	}
}
