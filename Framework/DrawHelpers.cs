using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;

namespace ClayMap.Framework
{
    public static class DrawHelpers
    {
        public static Rectangle TileToRect(Vector2 tile)
        {
            return new Rectangle(
                (int)tile.X * Game1.tileSize,
                (int)tile.Y * Game1.tileSize,
                Game1.tileSize,
                Game1.tileSize
                );
        }
        public static Rectangle TransformToLocal(Rectangle global)
        {
            return Game1.GlobalToLocal(Game1.viewport, global);
        }

        public static Vector2 MeasureString(string text, float scale = 1)
        {
            return Game1.dialogueFont.MeasureString(text) * scale;
        }

        public static void DrawCenteredTextInRect(SpriteBatch spriteBatch, Rectangle rect, string text, Color color, float fontScale = 1, int shadowOffset = 1)
        {
            // measure font and offset the text in the rect
            float localFontScale = fontScale * Game1.options.zoomLevel;
            Vector2 textSize = MeasureString(text, localFontScale);
            Vector2 pos = (new Vector2(rect.Width - textSize.X, rect.Height - textSize.Y) / 2) + new Vector2(rect.X, rect.Y);
            spriteBatch.DrawString(Game1.dialogueFont, text, new Vector2(pos.X + shadowOffset, pos.Y + shadowOffset), Color.Black, 0f, Vector2.Zero, localFontScale, SpriteEffects.None, 1f);
            spriteBatch.DrawString(Game1.dialogueFont, text, pos, color, 0f, Vector2.Zero, localFontScale, SpriteEffects.None, 1f);
        }
    }
}
