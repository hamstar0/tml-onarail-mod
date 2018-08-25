using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Helpers.UserHelpers;
using Microsoft.Xna.Framework;
using OnARail.Entities.Train;
using OnARail.NetProtocols;
using Terraria;
using Terraria.ModLoader;


namespace OnARail.Commands {
	class RespawnTrainCommand : ModCommand {
		public override CommandType Type {
			get {
				return CommandType.Chat;
			}
		}
		public override string Command { get { return "oar-train-respawn"; } }
		public override string Usage { get { return "/" + this.Command; } }
		public override string Description { get { return "Respawns current player's train."; } }


		////////////////

		public override void Action( CommandCaller caller, string input, string[] args ) {
			if( Main.netMode == 2 ) {
				throw new HamstarException( "No server." );
			}

			var mymod = (OnARailMod)this.mod;

			if( !mymod.Config.DebugModeTrainRespawn ) {
				bool _;
				if( !UserHelpers.HasBasicServerPrivilege( caller.Player, out _ ) ) {
					caller.Reply( "Train respawning disabled by config.", Color.Red );
					return;
				} else {
					Main.NewText( "Priviledged player " + caller.Player.name + " respawns train.", Color.LightSteelBlue );
				}
			}

			int who = TrainEntityHandler.FindMyTrain( caller.Player );
			if( who != -1 ) {
				CustomEntityManager.RemoveEntityByWho( who );
			}

			PacketProtocol.QuickRequestToServer<TrainSpawnProtocol>();
			
			caller.Reply( "Spawning train...", Color.LimeGreen );
		}
	}
}
