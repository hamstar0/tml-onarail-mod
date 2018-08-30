using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Components.CustomEntity.Components;
using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.PlayerHelpers;
using Microsoft.Xna.Framework;
using OnARail.Buffs;
using Terraria;


namespace OnARail.Entities.Train {
	public partial class TrainEntityHandler {
		public static void WarpPlayerToTrain( Player player ) {
			if( !SaveableEntityComponent.IsLoaded ) {
				throw new HamstarException( "OnARail.TrainEntityHandler.WarpPlayerToTrain - Entities not loaded." );
			}

			var myplayer = player.GetModPlayer<OnARailPlayer>();
			if( myplayer.MyTrainWho == -1 ) {
				throw new HamstarException( "OnARail.TrainEntityHandler.WarpPlayerToTrain - Player " + player.name + " (" + player.whoAmI + ") has no train." );
			}

			CustomEntity train_ent = CustomEntityManager.GetEntityByWho( myplayer.MyTrainWho );
			if( train_ent == null ) {
				throw new HamstarException( "OnARail.TrainEntityHandler.WarpPlayerToTrain - Player " + player.name + " (" + player.whoAmI + ") has no train entity." );
			}

			PlayerHelpers.Teleport( player, train_ent.Core.Center - new Vector2( player.width / 2, ( player.height / 2 ) + 16 ) );

			int train_buff_id = OnARailMod.Instance.BuffType<TrainMountBuff>();
			player.AddBuff( train_buff_id, 30 );
		}
	}
}
