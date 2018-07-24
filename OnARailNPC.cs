using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace OnARail {
	class OnARailNPC : GlobalNPC {
		public override void SetupShop( int npc_type, Chest shop, ref int next_slot ) {
			var mymod = (OnARailMod)this.mod;

			if( mymod.Config.MerchantSellsRails ) {
				if( npc_type == NPCID.Merchant ) {
					Item item = new Item();
					item.SetDefaults( ItemID.MinecartTrack, true );

					shop.item[next_slot++] = item;
				}
			}
		}
	}
}
