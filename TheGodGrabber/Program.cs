using System;
using LeagueSharp;
using LeagueSharp.SDK.Core;
using LeagueSharp.SDK.Core.Enumerations;
using LeagueSharp.SDK.Core.Extensions;
using LeagueSharp.SDK.Core.UI.IMenu.Values;
using LeagueSharp.SDK.Core.Wrappers;
using LeagueSharp.SDK.Core.Events;
using LeagueSharp.SDK.Core.Utils;

namespace GodOfgrabs
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args != null)
            {
                try
                {
                    Load.OnLoad += Blitzcrank.OnLoad;
                    Console.WriteLine("Blitzcrank loaded.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }
    }
}
