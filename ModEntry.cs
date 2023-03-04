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
        private ClayTileMap clayTileMap;
        private WinterRootMap winterRootMap;
        private SnowYamMap snowYamMap;

        public override void Entry(IModHelper helper)
        {
            clayTileMap = new ClayTileMap();
            winterRootMap = new WinterRootMap();
            snowYamMap = new SnowYamMap();

            helper.Events.GameLoop.UpdateTicked += GameLoop_UpdateTicked;
            helper.Events.Display.RenderedWorld += Display_RenderedWorld;
            helper.ConsoleCommands.Add(
                "claymap_setdepth",
                "Sets the number of future spots to draw.\n\nUsage:claymap_setdepth <value>\n-value:integer depth",
                this.SetDepthAll);

            helper.ConsoleCommands.Add(
                "claymap_toggle",
                "toggles the clay map on/off",
                this.ToggleAll);
        }

        private void GameLoop_UpdateTicked(object sender, UpdateTickedEventArgs e)
        {
            // ignore if player hasn't loaded a save yet
            if (!Context.IsWorldReady)
                return;

            clayTileMap?.Update(this.Monitor, this.Helper);
            winterRootMap?.Update(this.Monitor, this.Helper);
            snowYamMap?.Update(this.Monitor, this.Helper);
        }

        private void Display_RenderedWorld(object sender, RenderedWorldEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            clayTileMap?.Draw(e.SpriteBatch);
            winterRootMap?.Draw(e.SpriteBatch);
            snowYamMap?.Draw(e.SpriteBatch);
        }

        private void SetDepthAll(string command, string[] args)
        {
            SetDepth(clayTileMap, command, args);
            SetDepth(winterRootMap, command, args);
            SetDepth(snowYamMap, command, args);
        }
        private void ToggleAll(string command, string[] args)
        {
            Toggle(clayTileMap, command, args);
            Toggle(winterRootMap, command, args);
            Toggle(snowYamMap, command, args);
        }

        #region base funcs
        private void SetDepth(SObjectTileMap tileMap, string command, string[] args)
        {
            if (tileMap == null)
            {
                this.Monitor.Log($"clay map depth could not be set...", LogLevel.Info);
                return;
            }

            int depth = Math.Max(1, int.Parse(args[0]));
            tileMap.Depth = depth;
            this.Monitor.Log($"{tileMap.Name} depth set to {depth}.", LogLevel.Info);
            tileMap.Reset();
        }

        private void Toggle(SObjectTileMap tileMap, string command, string[] args)
        {
            if (tileMap == null)
            {
                this.Monitor.Log($"clay map not active", LogLevel.Info);
                return;
            }
            tileMap.Toggle();
        }
        #endregion
    }
}
