﻿using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Components.CustomEntity.Components;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using Newtonsoft.Json;
using OnARail.Buffs;
using Terraria;


namespace OnARail.Entities.Train.Components {
	class TrainMouseInteractionEntityComponent : IsClickableEntityComponent {
		private class TrainMouseInteractionEntityComponentFactory : CustomEntityComponentFactory<TrainMouseInteractionEntityComponent> {
			public TrainMouseInteractionEntityComponentFactory() { }

			protected override void InitializeComponent( TrainMouseInteractionEntityComponent data ) { }
		}


		////////////////

		public static TrainMouseInteractionEntityComponent CreateTrainMouseInteractionEntityComponent() {
			var factory = new TrainMouseInteractionEntityComponentFactory();
			return factory.Create();
		}


		////////////////

		public const float BoardingDistance = 96;



		////////////////
		
		[PacketProtocolIgnore]
		[JsonIgnore]
		public bool IsMouseHovering = false;



		////////////////

		protected TrainMouseInteractionEntityComponent( PacketProtocolDataConstructorLock ctor_lock ) : base( ctor_lock ) { }


		////////////////

		protected override void OnMouseHover( CustomEntity ent ) {
			Player player = Main.LocalPlayer;

			this.IsMouseHovering = player.Distance( ent.Core.Center ) <= TrainMouseInteractionEntityComponent.BoardingDistance;
			
			if( this.IsMouseHovering ) {
				if( Main.mouseRight ) {
					var draw_comp = ent.GetComponentByType<TrainDrawInGameEntityComponent>();
					
					if( !player.dead && !draw_comp.IsMinecartIconHovering ) {
						var myplayer = player.GetModPlayer<OnARailPlayer>();

						if( !myplayer.IsInInitLockdown ) {
							if( ent.MyOwnerPlayerWho == player.whoAmI ) {
								int train_buff_id = OnARailMod.Instance.BuffType<TrainMountBuff>();

								if( player.FindBuffIndex( train_buff_id ) == -1 ) {
									player.AddBuff( train_buff_id, 30 );    // Board train
								}
							}
						}
					}
				}
			}
		}
		
		public override void UpdateSingle( CustomEntity ent ) {
			this.IsMouseHovering = false;
			base.UpdateSingle( ent );
		}
		public override void UpdateClient( CustomEntity ent ) {
			this.IsMouseHovering = false;
			base.UpdateClient( ent );
		}
	}
}
