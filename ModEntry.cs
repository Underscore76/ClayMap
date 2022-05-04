using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClayMap.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;

namespace ClayMap
{
    class ModEntry : Mod
    {
        private ClayTileMap drawer;

        public override void Entry(IModHelper helper)
        {
            drawer = new ClayTileMap();
            helper.Events.GameLoop.UpdateTicked += GameLoop_UpdateTicked;
            helper.Events.Display.RenderedWorld += Display_RenderedWorld;
            helper.ConsoleCommands.Add(
                "claymap_setdepth",
                "Sets the number of future clay spots to draw.\n\nUsage:claymap_setdepth <value>\n-value:integer depth", this.SetDepth);
        }

        private void GameLoop_UpdateTicked(object sender, UpdateTickedEventArgs e)
        {
            // ignore if player hasn't loaded a save yet
            if (!Context.IsWorldReady)
                return;

            drawer?.Update(this.Monitor, this.Helper);
        }

        private void Display_RenderedWorld(object sender, RenderedWorldEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            drawer?.Draw(e.SpriteBatch);
        }

        private void SetDepth(string command, string[] args)
        {
            if (drawer == null)
            {
                this.Monitor.Log($"clay map depth could not be set...", LogLevel.Info);
                return;
            }

            int depth = Math.Max(1, int.Parse(args[0]));
            drawer.Depth = depth;
            this.Monitor.Log($"clay map depth set to {depth}.", LogLevel.Info);
            drawer.Reset();
        }
    }
}
