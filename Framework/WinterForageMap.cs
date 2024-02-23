using System;
using StardewValley;
using StardewModdingAPI;
using Microsoft.Xna.Framework;
using StardewValley.Tools;
using StardewValley.Locations;
using StardewValley.Enchantments;
using StardewValley.Extensions;
using xTile.Dimensions;

namespace ClayMap.Framework
{
	public class WinterRootMap : SObjectTileMap
	{
		private uint NumHoed;
        public override string Name => "WinterRootMap";

        public WinterRootMap() : base("(O)412")
		{
			NumHoed = uint.MaxValue;
		}

        protected override bool EvalTile(Vector2 tile, int depth)
        {
            if (!TileInfo.IsTillable(Game1.currentLocation, tile)) return false;
            Random r = Utility.CreateDaySaveRandom(tile.X * 2000, tile.Y * 77, Game1.stats.DirtHoed + depth);
            bool generousEnchant = Game1.player?.CurrentTool is Hoe && Game1.player.CurrentTool.hasEnchantmentOfType<GenerousEnchantment>();
            float generousChance = 0.5f;
            GameLocation loc = Game1.currentLocation;
            if (!loc.IsFarm && loc.IsOutdoors && Game1.GetSeasonForLocation(loc) == Season.Winter && r.NextDouble() < 0.08 && !(loc is Desert))
            {
                return r.Choose("(O)412", "(O)416") == "(O)412" || (generousEnchant && r.NextDouble() < generousChance && r.Choose("(O)412", "(O)416") == "(O)412");
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
        public override string Name => "SnowYamMap";

        public SnowYamMap() : base("(O)416")
        {
            NumHoed = uint.MaxValue;
        }

        protected override bool EvalTile(Vector2 tile, int depth)
        {
            if (!TileInfo.IsTillable(Game1.currentLocation, tile)) return false;
            Random r = Utility.CreateDaySaveRandom(tile.X * 2000, tile.Y * 77, Game1.stats.DirtHoed + depth);
            bool generousEnchant = Game1.player?.CurrentTool is Hoe && Game1.player.CurrentTool.hasEnchantmentOfType<GenerousEnchantment>();
            float generousChance = 0.5f;
            GameLocation loc = Game1.currentLocation;
            if (!loc.IsFarm && loc.IsOutdoors && Game1.GetSeasonForLocation(loc) == Season.Winter && r.NextDouble() < 0.08 && !(loc is Desert))
            {
                return r.Choose("(O)412", "(O)416") == "(O)416" || (generousEnchant && r.NextDouble() < generousChance && r.Choose("(O)412", "(O)416") == "(O)416");
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

