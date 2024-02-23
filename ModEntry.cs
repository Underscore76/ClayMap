using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClayMap.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using GenericModConfigMenu;

namespace ClayMap
{
    class ModEntry : Mod
    {
        private ModConfig Config;
        private ClayTileMap clayTileMap;
        private WinterRootMap winterRootMap;
        private SnowYamMap snowYamMap;

        public override void Entry(IModHelper helper)
        {
            this.Config = this.Helper.ReadConfig<ModConfig>();
            clayTileMap = new ClayTileMap(){
                Active = this.Config.ClayMap_Visible,
                Depth = this.Config.ClayMap_Depth
            };
            winterRootMap = new WinterRootMap(){
                Active = this.Config.ClayMap_Visible,
                Depth = this.Config.ClayMap_Depth
            };
            snowYamMap = new SnowYamMap(){
                Active = this.Config.ClayMap_Visible,
                Depth = this.Config.ClayMap_Depth
            };

            helper.Events.GameLoop.GameLaunched += GameLoop_GameLaunched;
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

        private void GameLoop_GameLaunched(object sender, GameLaunchedEventArgs e)
        {
            // get Generic Mod Config Menu's API (if it's installed)
            var configMenu = this.Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
            if (configMenu is null)
                return;

            // register mod
            configMenu.Register(
                mod: this.ModManifest,
                reset: () => this.Config = new ModConfig(),
                save: () => this.Helper.WriteConfig(this.Config)
            );

            // add some config options
            configMenu.AddBoolOption(
                mod: this.ModManifest,
                name: () => "Active",
                tooltip: () => "Draw clay map overlay",
                getValue: () => this.Config.ClayMap_Visible,
                setValue: (value) => {
                    SetVisibleAll(value);
                }
            );

            // add some config options
            configMenu.AddNumberOption(
                mod: this.ModManifest,
                name: () => "Lookahead",
                tooltip: () => "Number of future spots to draw",
                getValue: () => this.Config.ClayMap_Depth,
                setValue: (value) => {
                    SetDepthAll(value);
                },
                min: 1,
                max: 10,
                interval: 1
            );
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
            if (int.TryParse(args[0], out int depth))
            {
                SetDepthAll(depth);
                return;
            }
            this.Monitor.Log($"clay map depth could not be set...", LogLevel.Info);
        }
        private void SetDepthAll(int depth)
        {
            depth = Math.Max(1, depth);
            clayTileMap?.SetDepth(depth);
            winterRootMap?.SetDepth(depth);
            snowYamMap?.SetDepth(depth);
            this.Config.ClayMap_Depth = depth;
        }

        private void ToggleAll(string command, string[] args)
        {
            clayTileMap?.Toggle();
            winterRootMap?.Toggle();
            snowYamMap?.Toggle();
            this.Config.ClayMap_Visible = !this.Config.ClayMap_Visible;
        }

        private void SetVisibleAll(bool visibility)
        {
            clayTileMap?.SetVisible(visibility);
            winterRootMap?.SetVisible(visibility);
            snowYamMap?.SetVisible(visibility);
            this.Config.ClayMap_Visible = visibility;
        }
    }
}
