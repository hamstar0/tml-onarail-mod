﻿using System;
using System.Collections.Generic;
using System.Linq;
using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Components.CustomEntity.Components;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using Microsoft.Xna.Framework;
using OnARail.Entities.Train.Components;
using Terraria;


namespace OnARail.Entities.Train {
	public partial class TrainEntity : CustomEntity {
		public static int FindMyTrain( Player player ) {
			if( !SaveableEntityComponent.HaveAllEntitiesLoaded ) {
				LogHelpers.LogOnce( "!OnARail.TrainEntityHandler.FindMyTrain - Entities not loaded." );
				return -1;
			}

			ISet<TrainEntity> ents = CustomEntityManager.GetEntitiesForPlayer<TrainEntity>( player );

			return ents.FirstOrDefault()?.Core.WhoAmI ?? -1;
		}



		////////////////

		public override bool SyncFromClient => false;
		public override bool SyncFromServer => true;


		////////////////

		protected TrainEntity( PacketProtocolDataConstructorLock ctor_lock ) : base( ctor_lock ) { }


		////

		private IList<CustomEntityComponent> CreateComponents() {
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


		////////////////

		protected override IList<CustomEntityComponent> CreateComponents<T>( CustomEntityFactory<T> factory ) {
			return this.CreateComponents();
		}

		protected override CustomEntityCore CreateCore<T>( CustomEntityFactory<T> factory ) {
			Vector2 pos = factory.OwnerPlayer.Center;
			pos.Y -= 16;
			pos.X = MathHelper.Clamp( pos.X, 160, ( Main.maxTilesX - 10 ) * 16 );
			pos.Y = MathHelper.Clamp( pos.Y, 160, ( Main.maxTilesY - 10 ) * 16 );

			string name = factory.OwnerPlayer.name + "'s Train";

			var core = new CustomEntityCore( name, 64, 48, pos, 1 ) {
				Center = pos
			};

			return core;
		}


		public override IList<CustomEntityComponent> CreateComponentsTemplate() {
			return this.CreateComponents();
		}

		public override CustomEntityCore CreateCoreTemplate() {
			Vector2 pos = default( Vector2 );
			string name = "Unclaimed Train";
			var core = new CustomEntityCore( name, 64, 48, pos, 1 ) {
				Center = pos
			};

			return core;
		}
	}
}
