using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;
using StardewModdingAPI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using static System.Net.Mime.MediaTypeNames;

namespace ClayMap.Framework
{
    public class ClayTileMap
    {
        // stashed state so we don't have to recompute each time
        public int Depth = 1;
        private uint NumHoed;
        private string LocationName;
        private Rectangle SourceRect;

        private List<List<Vector2>> Tiles;
        public ClayTileMap()
        {
            NumHoed = uint.MaxValue;
            LocationName = "";
            Tiles = new List<List<Vector2>>();
            SourceRect = Rectangle.Empty;
        }

        public void Reset(bool rollover = false)
        {
            NumHoed = Game1.stats.DirtHoed;
            LocationName = Game1.currentLocation.Name;
            if (rollover)
            {
                // drop the current and append a new max depth set of tiles
                if (Tiles.Count > 0)
                    Tiles.RemoveAt(0);
                Tiles.Add(BuildTiles(Depth - 1));
            }
            else
            {
                // rebuild all
                Tiles = new List<List<Vector2>>();
                for (int i = 0; i < Depth; ++i)
                {
                    Tiles.Add(BuildTiles(i));
                }
            }

        }

        public void Update(IMonitor monitor, IModHelper helper)
        {
            if (SourceRect == Rectangle.Empty && Game1.objectSpriteSheet != null)
            {
                SourceRect = GameLocation.getSourceRectForObject(330);
            }
            if (Game1.currentLocation == null || Game1.stats == null) return;

            if (Game1.stats.DirtHoed != NumHoed)
            {
                Reset(true);
            }
            else if (Game1.currentLocation.Name != LocationName)
            {
                Reset(false);
            }
        }

        public void DrawClayAtTile(SpriteBatch spriteBatch, Vector2 tile, int depth)
        {
            Rectangle destRect = DrawHelpers.TransformToLocal(DrawHelpers.TileToRect(tile));
            spriteBatch.Draw(Game1.objectSpriteSheet, destRect, SourceRect, Color.White);
            DrawHelpers.DrawCenteredTextInRect(spriteBatch, destRect, depth.ToString(), Color.White);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Tiles.Count == 0) return;

            for (int i = 0; i < Depth; ++i)
            {
                foreach (Vector2 tile in Tiles[i])
                {
                    DrawClayAtTile(spriteBatch, tile, i);
                }
            }
        }


        // handle the actual tile generation
        private bool EvalTile(Vector2 tile, int depth)
        {
            if (!TileInfo.IsTillable(Game1.currentLocation, tile)) return false;
            Random r = new Random(((int)tile.X) * 2000 + ((int)tile.Y) * 77 + (int)Game1.uniqueIDForThisGame / 2 + (int)Game1.stats.DaysPlayed + (int)Game1.stats.DirtHoed + depth);
            return r.NextDouble() < 0.03;
        }

        private List<Vector2> BuildTiles(int depth)
        {
            List<Vector2> tiles = new List<Vector2>();
            if (Game1.currentLocation == null || Game1.stats == null) return tiles;

            int layerHeight = Game1.currentLocation.map.Layers[0].LayerHeight;
            int layerWidth = Game1.currentLocation.map.Layers[0].LayerWidth;
            for (int x = 0; x < layerWidth; x++)
            {
                for (int y = 0; y < layerHeight; y++)
                {
                    Vector2 tile = new Vector2(x, y);
                    if (EvalTile(tile, depth))
                    {
                        tiles.Add(tile);
                    }

                }
            }
            return tiles;
        }

    }
}
