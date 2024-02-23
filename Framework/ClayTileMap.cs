using System;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Tools;
using StardewValley.Locations;
using StardewValley.Enchantments;
using StardewValley.Extensions;
using StardewModdingAPI;
using StardewValley.GameData.Locations;

namespace ClayMap.Framework
{
    public class ClayTileMap : SObjectTileMap
    {
        private uint NumHoed;
        public override string Name => "ClayMap";
        public ClayTileMap() : base("(O)330")
        {
            NumHoed = uint.MaxValue;
        }

        protected override bool EvalTile(Vector2 tile, int depth)
        {
            if (!TileInfo.IsTillable(Game1.currentLocation, tile)) return false;
            Random r = Utility.CreateDaySaveRandom(tile.X * 2000, tile.Y * 77, Game1.stats.DirtHoed + depth);
            GameLocation loc = Game1.currentLocation;
            if (!loc.IsFarm && loc.IsOutdoors && Game1.GetSeasonForLocation(loc) == Season.Winter && r.NextDouble() < 0.08 && !(loc is Desert))
            {
                return false;
            }
            LocationData data = Game1.currentLocation.GetData();
            if (loc.IsOutdoors && r.NextBool(data?.ChanceForClay ?? 0.03))
			{
				return true;
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