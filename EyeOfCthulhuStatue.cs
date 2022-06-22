using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace EyeOfCthulhuStatueMod
{
	public class EyeOfCthulhuStatue : ModTile
	{
        public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileObsidianKill[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Statue");
			AddMapEntry(new Color(144, 148, 144), name);
			DustType = DustID.Stone;
			NPCID.Sets.StatueSpawnedDropRarity[NPCID.EyeofCthulhu] = 0.02f;
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 48, 64, ModContent.ItemType<EyeOfCthulhuStatueItem>());
		}

		public override void HitWire(int i, int j)
		{
			// Find the coordinates of top left tile square through math
			int y = j - (Main.tile[i, j].TileFrameY / 18);
			int x = i - (Main.tile[i, j].TileFrameX / 18);

			Wiring.SkipWire(x, y);
			Wiring.SkipWire(x, y + 1);
			Wiring.SkipWire(x, y + 2);
			Wiring.SkipWire(x, y + 3);
			Wiring.SkipWire(x + 1, y);
			Wiring.SkipWire(x + 1, y + 1);
			Wiring.SkipWire(x + 1, y + 2);
			Wiring.SkipWire(x + 1, y + 3);
			Wiring.SkipWire(x + 2, y + 1);
			Wiring.SkipWire(x + 2, y + 2);
			Wiring.SkipWire(x + 2, y + 3);

			int spawnX = x * 16 + 16 + 8;
			int spawnY = (y + 4) * 16;
			int npcIndex = -1;
			if (Wiring.CheckMech(x, y, 30) && NPC.MechSpawn((float)spawnX, (float)spawnY, NPCID.EyeofCthulhu))
			{
				npcIndex = NPC.NewNPC(new EntitySource_Wiring(spawnX, spawnY), spawnX, spawnY - 12, NPCID.EyeofCthulhu);
			}
			if (npcIndex >= 0)
			{
				Main.npc[npcIndex].value = 0f;
				Main.npc[npcIndex].npcSlots = 0f;
				Main.npc[npcIndex].SpawnedFromStatue = true;
			}
		}
	}

	public class EyeOfCthulhuStatueItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Eye of Cthulhu Statue");
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.ArmorStatue);
			Item.createTile = ModContent.TileType<EyeOfCthulhuStatue>();
			Item.placeStyle = 0;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.StoneBlock, 50)
				.AddIngredient(ItemID.SuspiciousLookingEye)
				.AddTile(TileID.DemonAltar)
				.Register();
		}
	}
}