using System;
using System.Collections.Generic;
using StardewValley;
using StardewModdingAPI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ClayMap.Framework
{
	public abstract class SObjectTileMap
	{
        private bool Active;
        private string LocationName;
        public int Depth = 1;
        public int ParentSheetIndex;
        private List<List<Vector2>> Tiles;
        protected Rectangle SourceRect;

        public SObjectTileMap(int parentSheetIndex)
        {
            ParentSheetIndex = parentSheetIndex;
            LocationName = "";
            Tiles = new List<List<Vector2>>();
            SourceRect = Rectangle.Empty;
            Active = true;
        }

        #region override methods
        public abstract string Name { get; }
        protected abstract bool EvalTile(Vector2 tile, int depth);
        protected abstract bool ShouldReset();

        public virtual void Reset(bool rollover = false)
        {
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
        #endregion

        #region public methods
        public void Update(IMonitor monitor, IModHelper helper)
        {
            if (SourceRect == Rectangle.Empty && Game1.objectSpriteSheet != null)
            {
                SourceRect = GameLocation.getSourceRectForObject(ParentSheetIndex);
            }
            if (Game1.currentLocation == null || Game1.stats == null || !Active) return;

            if (ShouldReset())
            {
                Reset(true);
            }
            else if (Game1.currentLocation.Name != LocationName)
            {
                Reset(false);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Tiles.Count == 0 || !Active) return;

            for (int i = 0; i < Depth; ++i)
            {
                foreach (Vector2 tile in Tiles[i])
                {
                    DrawObjectText(spriteBatch, tile, i > 0 ? i.ToString() : "");
                }
            }
        }

        public void Toggle()
        {
            Active = !Active;
            if (Active)
            {
                Reset();
            }
        }
        #endregion

        #region private methods
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

        private void DrawObjectText(SpriteBatch spriteBatch, Vector2 tile, string text)
        {
            Rectangle destRect = DrawHelpers.TransformToLocal(DrawHelpers.TileToRect(tile));
            spriteBatch.Draw(Game1.objectSpriteSheet, destRect, SourceRect, Color.White);
            DrawHelpers.DrawCenteredTextInRect(spriteBatch, destRect, text, Color.White);
        }
        #endregion
    }
}

