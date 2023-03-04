using System;
using StardewValley;
using StardewModdingAPI;
using Microsoft.Xna.Framework;
using StardewValley.Tools;

namespace ClayMap.Framework
{
	public class WinterRootMap : SObjectTileMap
	{
		private uint NumHoed;
        public override string Name => "WinterForageMap";

        public WinterRootMap() : base(412)
		{
			NumHoed = uint.MaxValue;
		}

        protected override bool EvalTile(Vector2 tile, int depth)
        {
            if (!TileInfo.IsTillable(Game1.currentLocation, tile)) return false;
            Random r = new Random(((int)tile.X) * 2000 + ((int)tile.Y) * 77 + (int)Game1.uniqueIDForThisGame / 2 + (int)Game1.stats.DaysPlayed + (int)Game1.stats.DirtHoed + depth);
            GameLocation loc = Game1.currentLocation;
            if (!loc.IsFarm && loc.IsOutdoors && Game1.GetSeasonForLocation(loc).Equals("winter") && r.NextDouble() < 0.08 && !(loc is StardewValley.Locations.Desert))
            {
                if (r.NextDouble() < 0.5) return true;
                bool generousEnchant = Game1.player != null && Game1.player.CurrentTool != null && Game1.player.CurrentTool is Hoe && Game1.player.CurrentTool.hasEnchantmentOfType<GenerousEnchantment>();
                float generousChance = 0.5f;
                return generousEnchant && r.NextDouble() < generousChance && r.NextDouble() < 0.5;
            }
            return false;
        }


        protected override bool ShouldReset()
        {
            return Game1.stats.DirtHoed != NumHoed;
        }

        public override void Reset(bool rollover = false)
        {
            NumHoed = Game1.stats.DirtHoed;
            base.Reset(rollover);
        }
    }

    public class SnowYamMap : SObjectTileMap
    {
        private uint NumHoed;
        public override string Name => "WinterForageMap";

        public SnowYamMap() : base(416)
        {
            NumHoed = uint.MaxValue;
        }

        protected override bool EvalTile(Vector2 tile, int depth)
        {
            if (!TileInfo.IsTillable(Game1.currentLocation, tile)) return false;
            Random r = new Random(((int)tile.X) * 2000 + ((int)tile.Y) * 77 + (int)Game1.uniqueIDForThisGame / 2 + (int)Game1.stats.DaysPlayed + (int)Game1.stats.DirtHoed + depth);
            GameLocation loc = Game1.currentLocation;
            if (!loc.IsFarm && loc.IsOutdoors && Game1.GetSeasonForLocation(loc).Equals("winter") && r.NextDouble() < 0.08 && !(loc is StardewValley.Locations.Desert))
            {
                if (r.NextDouble() >= 0.5) return true;
                bool generousEnchant = Game1.player != null && Game1.player.CurrentTool != null && Game1.player.CurrentTool is Hoe && Game1.player.CurrentTool.hasEnchantmentOfType<GenerousEnchantment>();
                float generousChance = 0.5f;
                return generousEnchant && r.NextDouble() < generousChance && r.NextDouble() >= 0.5;
            }
            return false;
        }


        protected override bool ShouldReset()
        {
            return Game1.stats.DirtHoed != NumHoed;
        }

        public override void Reset(bool rollover = false)
        {
            NumHoed = Game1.stats.DirtHoed;
            base.Reset(rollover);
        }
    }
}

