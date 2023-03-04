using System;
using StardewValley;
using StardewModdingAPI;
using Microsoft.Xna.Framework;

namespace ClayMap.Framework
{
    public class ClayTileMap : SObjectTileMap
    {
        private uint NumHoed;
        public override string Name => "ClayMap";
        public ClayTileMap() : base(330)
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
                return false;
            }
            return r.NextDouble() < 0.03;
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