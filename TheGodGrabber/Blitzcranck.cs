using System;
using System.Collections.Generic;
using System.Linq;
using LeagueSharp.SDK.Core.Extensions.SharpDX;
using LeagueSharp;
using LeagueSharp.SDK.Core;
using LeagueSharp.SDK.Core.Enumerations;
using LeagueSharp.SDK.Core.Extensions;
using LeagueSharp.SDK.Core.UI.IMenu.Values;
using LeagueSharp.SDK.Core.Wrappers;
using LeagueSharp.SDK.Core.Events;
using LeagueSharp.SDK.Core.Utils;
using SharpDX;
using Menu = LeagueSharp.SDK.Core.UI.IMenu.Menu;
using Color = System.Drawing.Color;
namespace GodOfgrabs
{
    class Blitzcrank
    {

        static Spell Q, E, R;
        static Menu menu;
        SpellDataInst spell;
       static int TotalGrabs=0;
       static int GoodGrabs=0;
        public static Obj_AI_Hero Player
        {
            get
            {
                return ObjectManager.Player;
            }
        }
        internal static void OnLoad(object sender, EventArgs e)
        {               

            if (Player.CharData.BaseSkinName.ToLower()  != "blitzcrank")
            {
                Console.WriteLine("Champion is not supported.");
                return;
            }
            try
            {
                Q = new Spell(SpellSlot.Q, 1050);
                E = new Spell(SpellSlot.E, 0);
                R = new Spell(SpellSlot.R, 600);
                Q.SetSkillshot(0.25f, 70f, 1800f, true, SkillshotType.SkillshotLine);
              //Q.SetSkillshot(Q.Instance.SData.SpellCastTime, Q.Instance.SData.LineWidth, Q.Instance.SData.MissileSpeed, true, SkillshotType.SkillshotLine);
                CreateMenu();
                Game.OnUpdate += OnUpdate;  
               Drawing.OnDraw += OnDraw;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static void CreateMenu()
        {

            menu = new Menu("Blitzcranck", "Bliztcranck", true);
            Bootstrap.Init(new string[] { }); //blabla

            var comboMenu = new Menu("Combo", "Combo Menu");
            {
                comboMenu.Add(new MenuList<string>("combohit", "Q hitchance ", new[] { "Medium", "Hight", "Very Hight" }));
                comboMenu.Add(new MenuBool("comboQ", "Use Q", true));
                comboMenu.Add(new MenuBool("comboE", "Use E", true));
                comboMenu.Add(new MenuBool("comboR", "Auto R", true));
        //       comboMenu.Add(new MenuBool("Min Enemys R", "Min Enemys R", true));
                menu.Add(comboMenu);
            }
            var drawingMenu = new Menu("Draws", "Draw Menu");
            {
                drawingMenu.Add(new MenuBool("drawQ", "Draw Q", true));
                drawingMenu.Add(new MenuBool("drawR", "Draw R", true));
                drawingMenu.Add(new MenuBool("drawGrabs", "Draw Grabs", true));
                //        comboMenu.Add(new MenuBool("Min Enemys R", "Min Enemys R", true));
                menu.Add(drawingMenu);
            }
            menu.Attach();
        }

        private static void OnDraw(System.EventArgs args)
        {
            var GrabsDraw = menu["Draws"]["drawGrabs"].GetValue<MenuBool>().Value;
            if (GrabsDraw)
            {
                Drawing.DrawText(100, 100, Color.Yellow, "Total grabs : " + TotalGrabs);
                Drawing.DrawText(100, 125, Color.Yellow, "Good grabs : " + GoodGrabs);
            }
            if (Player.IsDead) return;
            var QDraw = menu["Draws"]["drawQ"].GetValue<MenuBool>().Value;
            var RDraw = menu["Draws"]["drawR"].GetValue<MenuBool>().Value;
               if(QDraw)
            Drawing.DrawCircle(Player.Position,Q.Range,Color.Green);
               if (RDraw)
            Drawing.DrawCircle(Player.Position, R.Range, Color.Green);

        }

        private static void OnUpdate(System.EventArgs args)
        {
            switch (Orbwalker.ActiveMode)
            {
                case OrbwalkerMode.Orbwalk:
                    Combo();
                    break;
            }
        }
        static bool count=true;
        static bool countgood = true;
        public static bool HaveQ(Obj_AI_Base target)
        {
           return target.HasBuff("rocketgrab2"); 
        }
        public static HitChance getHit(MenuList combohit)
        {
            switch(combohit.Index)
            {
                case 1:
                    return HitChance.Medium;
                case 2:
                    return HitChance.High;
                case 3:
                    return HitChance.VeryHigh;
            }
            return HitChance.Medium;
        }
        public static void Combo()
        {
            var combohit = menu["Combo"]["combohit"].GetValue<MenuList>();
            var QCombo = menu["Combo"]["comboQ"].GetValue<MenuBool>().Value;
            var ECombo = menu["Combo"]["comboE"].GetValue<MenuBool>().Value;
            var RCombo = menu["Combo"]["comboR"].GetValue<MenuBool>().Value;
            var target = TargetSelector.GetTarget(Q.Range);
            
            if (QCombo)
            {

                if(!Q.IsReady())
                {
                    count=true;

                }
          //      Console.WriteLine(getHit(combohit));
                if (Q.CastIfHitchanceEquals(target, getHit(combohit)) == CastStates.SuccessfullyCasted)
                {
                    if (count == true)
                    {
                        countgood = true;
                        TotalGrabs++;
                        count = false;
                    }
                }
            }
            if (HaveQ(target))
            {
                if (countgood == true)
                {
                    GoodGrabs++;
                    countgood = false;
                }
            }
            if(E.IsInRange(target.Position,50))
            {
                E.Cast();

            }
            if(RCombo)
            {
             if(   R.CanCast(target))
                {
                    R.Cast();
                }
            }
        }
    }
}
