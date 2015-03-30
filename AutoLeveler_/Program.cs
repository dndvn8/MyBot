#region

using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;

#endregion

namespace AutoLeveler
{
    internal class Program
    {
        public static Menu Menu;
        

        private static void Main(string[] args)
        {
            Utils.ClearConsole();
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        private static void Game_OnGameLoad(EventArgs args)
        {
            Menu = new Menu("AutoLevelSpells", "AutoLevelSpells", true);
            Menu.AddItem(new MenuItem("Enabled", "Enabled", true).SetValue(true));
            Menu.AddToMainMenu();
            Game.OnUpdate += Game_OnGameUpdate;
        }

        private static void Game_OnGameUpdate(EventArgs args)
        {
            if (!Menu.Item("Enabled", true).IsActive())
            {
                return;
            }
            new AutoLevel(TreesAutoLevel.GetSequence().Select(l => l - 1));
            AutoLevel.Enable();
        }
    }
}