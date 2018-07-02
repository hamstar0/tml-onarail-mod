using OnARail.Buffs;
using Terraria.ModLoader;


namespace OnARail.Commands {
	class DebugCommand : ModCommand {
		public override string Command {
			get {
				return "railcheat";
			}
		}
		public override CommandType Type {
			get {
				return CommandType.Chat;
			}
		}
		public override string Usage {
			get {
				return "/" + this.Command;
			}
		}
		public override string Description {
			get {
				return "blah";
			}
		}


		////////////////

		public override void Action( CommandCaller caller, string input, string[] args ) {
			caller.Player.AddBuff( this.mod.BuffType<TrainMountBuff>(), 60*5 );
		}
	}
}
