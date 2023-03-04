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
                this.SetClayDepth);

            helper.ConsoleCommands.Add(
                "claymap_toggle",
                "toggles the clay map on/off",
                this.ToggleClay);

            helper.ConsoleCommands.Add(
                "wintermap_setdepth",
                "Sets the number of future spots to draw.\n\nUsage:wintermap_setdepth <value>\n-value:integer depth",
                this.SetWinterDepth);

            helper.ConsoleCommands.Add(
                "wintermap_toggle",
                "toggles the winter forage map on/off",
                this.ToggleWinter);
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



        private void SetClayDepth(string command, string[] args)
        {
            if (clayTileMap == null)
            {
                this.Monitor.Log($"{clayTileMap.Name} depth could not be set...", LogLevel.Info);
                return;
            }
            SetDepth(clayTileMap, command, args);
        }

        private void ToggleClay(string command, string[] args)
        {
            if (clayTileMap == null)
            {
                this.Monitor.Log($"{clayTileMap.Name} not active", LogLevel.Info);
                return;
            }
            Toggle(clayTileMap, command, args);
        }

        private void SetWinterDepth(string command, string[] args)
        {
            if (winterRootMap == null)
            {
                this.Monitor.Log($"{winterRootMap.Name} depth could not be set...", LogLevel.Info);
                return;
            }
            SetDepth(winterRootMap, command, args);

            if (snowYamMap == null)
            {
                this.Monitor.Log($"{snowYamMap.Name} depth could not be set...", LogLevel.Info);
                return;
            }
            SetDepth(snowYamMap, command, args);
        }

        private void ToggleWinter(string command, string[] args)
        {
            if (winterRootMap == null)
            {
                this.Monitor.Log($"{winterRootMap.Name} not active", LogLevel.Info);
                return;
            }
            Toggle(winterRootMap, command, args);

            if (snowYamMap == null)
            {
                this.Monitor.Log($"{snowYamMap.Name} not active", LogLevel.Info);
                return;
            }
            Toggle(snowYamMap, command, args);
        }

        #region base funcs
        private void SetDepth(SObjectTileMap tileMap, string command, string[] args)
        {
            int depth = Math.Max(1, int.Parse(args[0]));
            tileMap.Depth = depth;
            this.Monitor.Log($"{tileMap.Name} depth set to {depth}.", LogLevel.Info);
            tileMap.Reset();
        }

        private void Toggle(SObjectTileMap tileMap, string command, string[] args)
        {
            tileMap.Toggle();
        }
        #endregion
    }
}
