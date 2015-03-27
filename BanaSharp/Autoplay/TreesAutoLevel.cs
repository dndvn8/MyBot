/* Borrowed from Trees:
 * https://github.com/trees-software/LeagueSharp/blob/master/AutoLevelSpells/Program.cs 
 */
#region

using System;
using LeagueSharp;
using LeagueSharp.Common;

#endregion

namespace Support
{
    internal class TreesAutoLevel
    {
        public static int[] GetSequence()
        {
            var sequence = new int[18];
            switch (ObjectManager.Player.ChampionName)
            {
                case "Aatrox":
                    sequence = new[] { 0, 1, 2, 1, 1, 3, 1, 2, 1, 2, 3, 2, 2, 0, 0, 3, 0, 0 };
                    break;
                case "Ahri":
                    sequence = new[] { 0, 2, 0, 1, 0, 3, 0, 1, 0, 1, 3, 1, 1, 2, 2, 3, 1, 1 };
                    break;
                case "Akali":
                    sequence = new[] { 0, 1, 0, 2, 0, 3, 0, 2, 0, 2, 3, 2, 2, 1, 1, 3, 1, 1 };
                    break;
                case "Alistar":
                    sequence = new[] { 0, 2, 1, 0, 2, 3, 0, 2, 0, 2, 3, 0, 2, 1, 1, 3, 1, 1 };
                    break;
                case "Amumu":
                    sequence = new[] { 1, 2, 2, 0, 2, 3, 2, 0, 2, 0, 3, 0, 0, 1, 1, 3, 1, 1 };
                    break;
                case "Anivia":
                    sequence = new[] { 0, 2, 0, 2, 2, 3, 2, 1, 2, 1, 3, 0, 0, 0, 1, 3, 1, 1 };
                    break;
                case "Annie":
                    sequence = new[] { 0, 1, 0, 2, 0, 3, 0, 1, 0, 1, 3, 1, 1, 2, 2, 3, 2, 2 };
                    break;
                case "Ashe":
                    sequence = new[] { 1, 2, 1, 0, 1, 3, 1, 0, 1, 0, 3, 0, 0, 2, 2, 3, 2, 2 };
                    break;
                case "Azir":
                    sequence = new[] { 1, 0, 2, 0, 0, 3, 0, 1, 0, 2, 3, 2, 2, 1, 1, 3, 1, 1 };
                    break;
                case "Blitzcrank":
                    sequence = new[] { 0, 2, 1, 2, 1, 3, 2, 1, 2, 1, 3, 2, 1, 0, 0, 3, 0, 0 };
                    break;
                case "Brand":
                    sequence = new[] { 1, 2, 1, 0, 1, 3, 1, 2, 1, 2, 3, 2, 2, 0, 0, 3, 0, 0 };
                    break;
                case "Braum":
                    sequence = new[] { 0, 2, 0, 1, 0, 3, 0, 2, 0, 2, 3, 2, 2, 1, 1, 3, 1, 1 };
                    break;
                case "Caitlyn":
                    sequence = new[] { 1, 0, 0, 2, 0, 3, 0, 2, 0, 2, 3, 2, 2, 1, 1, 3, 1, 1 };
                    break;
                case "Cassiopeia":
                    sequence = new[] { 0, 2, 0, 1, 0, 3, 0, 2, 0, 2, 3, 2, 2, 1, 1, 3, 1, 1 };
                    break;
                case "Chogath":
                    sequence = new[] { 0, 2, 1, 1, 1, 3, 1, 2, 1, 2, 3, 2, 2, 0, 0, 3, 0, 0 };
                    break;
                case "Corki":
                    sequence = new[] { 0, 1, 2, 0, 0, 3, 0, 2, 0, 2, 3, 2, 2, 1, 1, 3, 1, 1 };
                    break;
                case "Darius":
                    sequence = new[] { 0, 2, 0, 1, 0, 3, 0, 1, 0, 1, 3, 1, 2, 1, 2, 3, 2, 2 };
                    break;
                case "Diana":
                    sequence = new[] { 1, 0, 1, 2, 0, 3, 0, 0, 0, 1, 3, 1, 1, 2, 2, 3, 2, 2 };
                    break;
                case "DrMundo":
                    sequence = new[] { 1, 0, 2, 1, 1, 3, 1, 2, 1, 2, 3, 2, 2, 0, 0, 3, 0, 0 };
                    break;
                case "Draven":
                    sequence = new[] { 0, 2, 1, 0, 0, 3, 0, 1, 0, 1, 3, 1, 1, 2, 2, 3, 2, 2 };
                    break;
                case "Elise":
                    sequence = new[] { 0, 2, 0, 1, 0, 3, 0, 1, 0, 1, 3, 1, 1, 2, 2, 3, 2, 2 };
                    break;
                case "Evelynn":
                    sequence = new[] { 0, 2, 0, 1, 0, 3, 0, 2, 0, 2, 3, 2, 2, 1, 1, 3, 1, 1 };
                    break;
                case "Ezreal":
                    sequence = new[] { 0, 2, 1, 0, 0, 3, 0, 2, 0, 2, 3, 2, 2, 1, 1, 3, 1, 1 };
                    break;
                case "FiddleSticks":
                    sequence = new[] { 2, 1, 1, 0, 1, 3, 1, 0, 1, 0, 3, 0, 0, 2, 2, 3, 2, 2 };
                    break;
                case "Fiora":
                    sequence = new[] { 1, 0, 2, 1, 1, 3, 1, 2, 1, 2, 3, 2, 2, 0, 0, 3, 0, 0 };
                    break;
                case "Fizz":
                    sequence = new[] { 2, 0, 1, 0, 1, 3, 0, 0, 0, 1, 3, 1, 1, 2, 2, 3, 2, 2 };
                    break;
                case "Galio":
                    sequence = new[] { 0, 1, 0, 2, 0, 3, 0, 1, 0, 1, 3, 2, 2, 1, 1, 3, 2, 2 };
                    break;
                case "Gangplank":
                    sequence = new[] { 0, 1, 0, 2, 0, 3, 0, 1, 0, 1, 3, 1, 1, 2, 2, 3, 2, 2 };
                    break;
                case "Garen":
                    sequence = new[] { 0, 1, 2, 2, 2, 3, 2, 0, 2, 0, 3, 0, 0, 1, 1, 3, 1, 1 };
                    break;
                case "Gnar":
                    sequence = new[] { 0, 1, 2, 0, 0, 3, 0, 1, 0, 1, 3, 1, 1, 2, 2, 3, 2, 2 };
                    break;
                case "Gragas":
                    sequence = new[] { 0, 2, 1, 0, 0, 3, 0, 1, 0, 1, 3, 1, 2, 1, 2, 3, 2, 2 };
                    break;
                case "Graves":
                    sequence = new[] { 0, 2, 1, 0, 0, 3, 0, 2, 0, 2, 3, 2, 2, 1, 1, 3, 1, 1 };
                    break;
                case "Hecarim":
                    sequence = new[] { 0, 1, 0, 2, 0, 3, 0, 1, 0, 1, 3, 1, 1, 2, 2, 3, 2, 2 };
                    break;
                case "Heimerdinger":
                    sequence = new[] { 0, 1, 1, 0, 0, 3, 2, 1, 1, 1, 3, 0, 0, 2, 2, 3, 0, 0 };
                    break;
                case "Irelia":
                    sequence = new[] { 2, 0, 1, 1, 1, 3, 1, 2, 1, 2, 3, 0, 0, 2, 0, 3, 2, 0 };
                    break;
                case "Janna":
                    sequence = new[] { 2, 0, 2, 1, 2, 3, 2, 1, 2, 1, 0, 1, 1, 0, 0, 0, 3, 3 };
                    break;
                case "JarvanIV":
                    sequence = new[] { 0, 2, 0, 1, 0, 3, 0, 2, 1, 0, 3, 2, 2, 2, 1, 3, 1, 1 };
                    break;
                case "Jax":
                    sequence = new[] { 2, 1, 0, 1, 1, 3, 1, 2, 1, 2, 3, 0, 2, 0, 0, 3, 2, 0 };
                    break;
                case "Jayce":
                    sequence = new[] { 0, 2, 0, 1, 0, 3, 0, 2, 0, 2, 3, 2, 2, 1, 1, 3, 1, 1 };
                    break;
                case "Jinx":
                    sequence = new[] { 0, 2, 1, 0, 0, 3, 0, 1, 0, 1, 3, 1, 1, 2, 2, 3, 2, 2 };
                    break;
                case "Karma":
                    sequence = new[] { 0, 2, 0, 1, 2, 0, 2, 0, 2, 0, 2, 0, 2, 1, 1, 1, 1, 1 };
                    break;
                case "Karthus":
                    sequence = new[] { 0, 2, 1, 0, 0, 3, 0, 0, 2, 2, 3, 2, 2, 1, 1, 3, 1, 1 };
                    break;
                case "Kassadin":
                    sequence = new[] { 0, 1, 0, 2, 0, 3, 0, 2, 0, 2, 3, 2, 2, 1, 1, 3, 1, 1 };
                    break;
                case "Katarina":
                    sequence = new[] { 0, 2, 1, 1, 1, 3, 1, 2, 1, 0, 3, 0, 0, 0, 2, 3, 2, 2 };
                    break;
                case "Kayle":
                    sequence = new[] { 2, 1, 2, 0, 2, 3, 2, 1, 2, 1, 3, 1, 1, 0, 0, 3, 0, 0 };
                    break;
                case "Kennen":
                    sequence = new[] { 0, 2, 1, 1, 1, 3, 1, 0, 1, 0, 3, 0, 0, 2, 2, 3, 2, 2 };
                    break;
                case "Khazix":
                    sequence = new[] { 0, 2, 0, 1, 0, 3, 0, 1, 0, 1, 3, 1, 1, 2, 2, 3, 2, 2 };
                    break;
                case "KogMaw":
                    sequence = new[] { 1, 2, 1, 0, 1, 3, 1, 0, 1, 0, 3, 0, 0, 2, 2, 3, 2, 2 };
                    break;
                case "Leblanc":
                    sequence = new[] { 0, 1, 2, 0, 0, 3, 0, 1, 0, 1, 3, 1, 2, 1, 2, 3, 2, 2 };
                    break;
                case "LeeSin":
                    sequence = new[] { 2, 0, 1, 0, 0, 3, 0, 1, 0, 1, 3, 1, 1, 2, 2, 3, 2, 2 };
                    break;
                case "Leona":
                    sequence = new[] { 0, 2, 1, 1, 1, 3, 1, 2, 1, 2, 3, 2, 2, 0, 0, 3, 0, 0 };
                    break;
                case "Lissandra":
                    sequence = new[] { 0, 2, 0, 1, 0, 3, 0, 1, 0, 1, 3, 1, 1, 2, 2, 3, 2, 2 };
                    break;
                case "Lucian":
                    sequence = new[] { 0, 2, 1, 0, 0, 3, 0, 1, 0, 1, 3, 1, 1, 2, 2, 3, 2, 2 };
                    break;
                case "Lulu":
                    sequence = new[] { 2, 1, 0, 2, 2, 3, 2, 1, 2, 1, 3, 1, 1, 0, 0, 3, 0, 0 };
                    break;
                case "Lux":
                    sequence = new[] { 2, 0, 2, 1, 2, 3, 2, 0, 2, 0, 3, 0, 0, 1, 1, 3, 1, 1 };
                    break;
                case "Malphite":
                    sequence = new[] { 0, 2, 0, 1, 0, 3, 0, 2, 0, 2, 3, 2, 1, 2, 1, 3, 1, 1 };
                    break;
                case "Malzahar":
                    sequence = new[] { 0, 2, 2, 1, 2, 3, 0, 2, 0, 2, 3, 1, 0, 1, 0, 3, 1, 1 };
                    break;
                case "Maokai":
                    sequence = new[] { 2, 0, 1, 2, 2, 3, 2, 1, 2, 1, 3, 1, 1, 0, 0, 3, 0, 0 };
                    break;
                case "MasterYi":
                    sequence = new[] { 2, 0, 2, 0, 2, 3, 2, 0, 2, 0, 3, 0, 1, 1, 1, 3, 1, 1 };
                    break;
                case "MissFortune":
                    sequence = new[] { 0, 2, 0, 1, 0, 3, 0, 1, 0, 1, 3, 1, 1, 2, 2, 3, 2, 2 };
                    break;
                case "MonkeyKing":
                    sequence = new[] { 2, 0, 1, 0, 0, 3, 2, 0, 2, 0, 3, 2, 2, 1, 1, 3, 1, 1 };
                    break;
                case "Mordekaiser":
                    sequence = new[] { 2, 0, 2, 1, 2, 3, 2, 0, 2, 0, 3, 0, 0, 1, 1, 3, 1, 1 };
                    break;
                case "Morgana":
                    sequence = new[] { 0, 1, 1, 2, 1, 3, 1, 0, 1, 0, 3, 0, 0, 2, 2, 3, 2, 2 };
                    break;
                case "Nami":
                    sequence = new[] { 0, 1, 2, 1, 1, 3, 1, 1, 2, 2, 3, 2, 2, 0, 0, 3, 0, 0 };
                    break;
                case "Nasus":
                    sequence = new[] { 0, 1, 0, 2, 0, 3, 0, 1, 0, 1, 3, 1, 2, 1, 2, 3, 2, 2 };
                    break;
                case "Nautilus":
                    sequence = new[] { 1, 2, 1, 0, 1, 3, 1, 2, 1, 2, 3, 2, 2, 0, 0, 3, 0, 0 };
                    break;
                case "Nidalee":
                    sequence = new[] { 1, 2, 0, 2, 0, 3, 2, 1, 2, 0, 3, 2, 0, 0, 1, 3, 1, 1 };
                    break;
                case "Nocturne":
                    sequence = new[] { 0, 1, 0, 2, 0, 3, 0, 2, 0, 2, 3, 2, 2, 1, 1, 3, 1, 1 };
                    break;
                case "Nunu":
                    sequence = new[] { 2, 0, 2, 1, 0, 3, 2, 0, 2, 0, 3, 0, 2, 1, 1, 3, 1, 1 };
                    break;
                case "Olaf":
                    sequence = new[] { 0, 2, 0, 1, 0, 3, 0, 2, 0, 2, 3, 2, 2, 1, 1, 3, 1, 1 };
                    break;
                case "Orianna":
                    sequence = new[] { 0, 2, 1, 0, 0, 3, 0, 1, 0, 1, 3, 1, 1, 2, 2, 3, 2, 2 };
                    break;
                case "Pantheon":
                    sequence = new[] { 0, 1, 2, 0, 0, 3, 0, 2, 0, 2, 3, 2, 1, 2, 1, 3, 1, 1 };
                    break;
                case "Poppy":
                    sequence = new[] { 2, 1, 0, 0, 0, 3, 0, 1, 0, 1, 1, 1, 2, 2, 2, 2, 3, 3 };
                    break;
                case "Quinn":
                    sequence = new[] { 2, 0, 0, 1, 0, 3, 0, 2, 0, 2, 3, 2, 2, 1, 1, 3, 1, 1 };
                    break;
                case "Rammus":
                    sequence = new[] { 0, 1, 2, 2, 2, 3, 2, 1, 2, 1, 3, 1, 1, 0, 0, 3, 0, 0 };
                    break;
                case "RekSai":
                    sequence = new[] { 0, 2, 1, 0, 0, 3, 0, 2, 0, 2, 3, 2, 2, 1, 1, 3, 1, 1 };
                    break;
                case "Renekton":
                    sequence = new[] { 1, 0, 2, 0, 0, 3, 0, 1, 0, 1, 3, 1, 1, 2, 2, 3, 2, 2 };
                    break;
                case "Rengar":
                    sequence = new[] { 0, 2, 1, 0, 0, 3, 1, 0, 0, 1, 3, 1, 1, 2, 2, 3, 2, 2 };
                    break;
                case "Riven":
                    sequence = new[] { 0, 2, 1, 0, 2, 3, 0, 0, 0, 2, 3, 2, 2, 1, 1, 3, 1, 1 };
                    break;
                case "Rumble":
                    sequence = new[] { 2, 0, 0, 1, 0, 3, 0, 2, 0, 2, 3, 2, 2, 1, 1, 3, 1, 1 };
                    break;
                case "Ryze":
                    sequence = new[] { 0, 1, 0, 2, 0, 3, 0, 1, 0, 1, 3, 1, 1, 2, 2, 3, 2, 2 };
                    break;
                case "Sejuani":
                    sequence = new[] { 1, 0, 2, 2, 1, 3, 2, 1, 2, 2, 3, 1, 0, 1, 0, 3, 0, 0 };
                    break;
                case "Shaco":
                    sequence = new[] { 1, 2, 0, 2, 2, 3, 2, 1, 2, 1, 3, 1, 1, 0, 0, 3, 0, 0 };
                    break;
                case "Shen":
                    sequence = new[] { 0, 1, 2, 0, 0, 3, 0, 1, 0, 1, 3, 1, 1, 2, 2, 3, 2, 2 };
                    break;
                case "Shyvana":
                    sequence = new[] { 1, 0, 1, 2, 1, 3, 1, 2, 1, 2, 3, 2, 0, 2, 0, 3, 0, 0 };
                    break;
                case "Singed":
                    sequence = new[] { 0, 2, 0, 2, 0, 3, 0, 1, 0, 1, 3, 2, 1, 2, 1, 3, 1, 2 };
                    break;
                case "Sion":
                    sequence = new[] { 0, 2, 2, 1, 2, 3, 2, 0, 2, 0, 3, 0, 0, 1, 1, 3, 1, 1 };
                    break;
                case "Sivir":
                    sequence = new[] { 1, 0, 2, 0, 0, 3, 0, 1, 0, 1, 3, 1, 1, 2, 2, 3, 2, 2 };
                    break;
                case "Skarner":
                    sequence = new[] { 0, 1, 0, 1, 0, 3, 0, 1, 0, 1, 3, 1, 2, 2, 2, 3, 2, 2 };
                    break;
                case "Sona":
                    sequence = new[] { 0, 1, 2, 0, 0, 3, 0, 1, 0, 1, 3, 1, 1, 2, 2, 3, 2, 2 };
                    break;
                case "Soraka":
                    sequence = new[] { 0, 1, 2, 0, 0, 3, 0, 1, 0, 2, 3, 1, 2, 1, 2, 3, 1, 2 };
                    break;
                case "Swain":
                    sequence = new[] { 1, 2, 2, 0, 2, 3, 2, 0, 2, 0, 3, 0, 0, 1, 1, 3, 1, 1 };
                    break;
                case "Syndra":
                    sequence = new[] { 0, 2, 0, 1, 0, 3, 0, 2, 0, 2, 3, 2, 2, 1, 1, 3, 1, 1 };
                    break;
                case "Talon":
                    sequence = new[] { 1, 2, 0, 1, 1, 3, 1, 0, 1, 0, 3, 0, 0, 2, 2, 3, 2, 2 };
                    break;
                case "Taric":
                    sequence = new[] { 2, 1, 0, 1, 1, 3, 0, 1, 1, 0, 3, 0, 0, 2, 2, 3, 2, 2 };
                    break;
                case "Teemo":
                    sequence = new[] { 0, 2, 1, 2, 0, 3, 2, 2, 2, 0, 3, 1, 1, 0, 1, 3, 1, 0 };
                    break;
                case "Thresh":
                    sequence = new[] { 0, 2, 1, 1, 1, 3, 1, 2, 1, 2, 3, 2, 2, 0, 0, 3, 0, 0 };
                    break;
                case "Tristana":
                    sequence = new[] { 2, 1, 1, 2, 1, 3, 1, 0, 1, 0, 3, 0, 0, 0, 2, 3, 2, 2 };
                    break;
                case "Trundle":
                    sequence = new[] { 0, 1, 0, 2, 0, 3, 0, 1, 0, 2, 3, 1, 2, 1, 2, 3, 1, 2 };
                    break;
                case "Tryndamere":
                    sequence = new[] { 2, 0, 1, 0, 0, 3, 0, 1, 0, 1, 3, 1, 1, 2, 2, 3, 2, 2 };
                    break;
                case "TwistedFate":
                    sequence = new[] { 1, 0, 0, 2, 0, 3, 0, 1, 0, 1, 3, 1, 1, 2, 2, 3, 2, 2 };
                    break;
                case "Twitch":
                    sequence = new[] { 2, 1, 0, 2, 2, 3, 2, 0, 2, 0, 3, 0, 0, 1, 1, 3, 1, 1 };
                    break;
                case "Udyr":
                    sequence = new[] { 3, 1, 2, 3, 3, 1, 3, 1, 3, 1, 1, 0, 2, 2, 2, 2, 0, 0 };
                    break;
                case "Urgot":
                    sequence = new[] { 2, 0, 0, 1, 0, 3, 0, 1, 0, 2, 3, 1, 2, 1, 2, 3, 1, 2 };
                    break;
                case "Varus":
                    sequence = new[] { 0, 1, 2, 0, 0, 3, 0, 1, 0, 1, 3, 1, 1, 2, 2, 3, 2, 2 };
                    break;
                case "Vayne":
                    sequence = new[] { 0, 2, 1, 0, 0, 3, 0, 1, 0, 1, 3, 1, 1, 2, 2, 3, 2, 2 };
                    break;
                case "Veigar":
                    sequence = new[] { 0, 2, 0, 1, 0, 3, 1, 1, 1, 1, 3, 2, 0, 0, 2, 3, 2, 2 };
                    break;
                case "VelKoz":
                    sequence = new[] { 0, 1, 2, 1, 1, 3, 1, 0, 1, 0, 3, 0, 0, 2, 2, 3, 2, 2 };
                    break;
                case "Vi":
                    sequence = new[] { 1, 2, 0, 0, 0, 3, 0, 1, 0, 0, 3, 1, 1, 2, 2, 3, 2, 2 };
                    break;
                case "Viktor":
                    sequence = new[] { 2, 1, 2, 0, 2, 3, 2, 0, 2, 0, 3, 0, 1, 0, 1, 3, 1, 1 };
                    break;
                case "Vladimir":
                    sequence = new[] { 0, 1, 0, 2, 0, 3, 0, 2, 0, 2, 3, 2, 2, 1, 1, 3, 1, 1 };
                    break;
                case "Volibear":
                    sequence = new[] { 1, 2, 1, 0, 1, 3, 2, 1, 0, 1, 3, 2, 0, 2, 0, 3, 2, 0 };
                    break;
                case "Warwick":
                    sequence = new[] { 1, 0, 0, 1, 0, 3, 0, 2, 0, 2, 3, 2, 2, 2, 1, 3, 1, 1 };
                    break;
                case "Xerath":
                    sequence = new[] { 0, 2, 0, 1, 0, 3, 0, 1, 0, 1, 3, 1, 1, 2, 2, 3, 2, 2 };
                    break;
                case "XinZhao":
                    sequence = new[] { 0, 2, 0, 1, 0, 3, 0, 1, 0, 1, 3, 1, 1, 2, 2, 3, 2, 2 };
                    break;
                case "Yasuo":
                    sequence = new[] { 0, 2, 0, 1, 0, 3, 0, 2, 0, 2, 3, 2, 2, 1, 1, 3, 1, 1 };
                    break;
                case "Yorick":
                    sequence = new[] { 1, 2, 0, 2, 2, 3, 2, 1, 2, 0, 3, 1, 0, 1, 0, 3, 1, 0 };
                    break;
                case "Zac":
                    sequence = new[] { 0, 1, 2, 0, 0, 3, 0, 2, 0, 2, 3, 2, 2, 1, 1, 3, 1, 1 };
                    break;
                case "Zed":
                    sequence = new[] { 0, 1, 2, 0, 0, 3, 0, 2, 0, 2, 3, 2, 2, 1, 1, 3, 1, 1 };
                    break;
                case "Ziggs":
                    sequence = new[] { 0, 1, 2, 0, 0, 3, 0, 2, 0, 2, 3, 2, 2, 1, 1, 3, 1, 1 };
                    break;
                case "Zilean":
                    sequence = new[] { 0, 1, 0, 2, 0, 3, 0, 1, 0, 1, 3, 1, 1, 2, 2, 3, 2, 2 };
                    break;
                case "Zyra":
                    sequence = new[] { 2, 1, 0, 0, 0, 3, 0, 2, 0, 2, 3, 2, 2, 1, 1, 3, 1, 1 };
                    break;
                default:
                    sequence = new[] { 0, 1, 2, 1, 1, 3, 1, 0, 1, 0, 3, 0, 0, 2, 2, 3, 2, 2 };
                    break;
            }
            return sequence;
        }
    }
}
