using System.Collections.Generic;
using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Components.CustomEntity.Components;
using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using Microsoft.Xna.Framework;
using OnARail.Entities.Train.Components;
using Terraria;


namespace OnARail.Entities.Train {
	public partial class TrainEntity : CustomEntity {
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

		private TrainEntity( PacketProtocolDataConstructorLock ctor_lock ) : base( Main.LocalPlayer ) { }
		
		public TrainEntity( Player owner_plr ) : base( owner_plr ) {
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

			this.Components = comps;
			this.Core = new CustomEntityCore( owner_plr.name+"'s Train", 64, 48, default( Vector2 ), 1 );
			
			Vector2 pos = owner_plr.Center;
			pos.Y -= 16;
			pos.X = MathHelper.Clamp( pos.X, 160, ( Main.maxTilesX - 10 ) * 16 );
			pos.Y = MathHelper.Clamp( pos.Y, 160, ( Main.maxTilesY - 10 ) * 16 );

			this.Core.Center = pos;

			if( Main.netMode == 2 ) {
				this.SyncTo();
			}
		}
	}
}
