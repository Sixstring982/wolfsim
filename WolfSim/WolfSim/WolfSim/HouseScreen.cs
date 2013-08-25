using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace WolfSim
{
    class HouseScreen : Screen
    {
        private Map m = new Map();
        private Player p = new Player();

        public static IAsset bodyAsset = IAsset.Sun;
        Vector2 bodyStart = new Vector2(20, 20);
        Vector2 bodyDest = new Vector2(1260, 20);

        int dayTickLength = 3500;
        int dayTicks = 0;
        int deathPlayTicks = 0;
        int deathPlays = 0;

        public override void Update()
        {
            dayTicks++;
            if (dayTicks > dayTickLength)
            {
                bodyAsset = (bodyAsset == IAsset.Sun) ? IAsset.Moon : IAsset.Sun;
                dayTicks = 0;
                if (bodyAsset == IAsset.Moon)
                {
                    Game1.PushScreen(new ChangeScreen("Night"));
                }
                else
                {
                    Game1.PopScreen();
                    Game1.PushScreen(new ChangeScreen("Failure."));
                }
                m.GoToStart();
            }

            if (!p.alive)
            {
                deathPlayTicks++;
                if (deathPlayTicks > 30)
                {
                    deathPlays++;
                    deathPlayTicks = 0;
                    if (deathPlays == 5)
                    {
                        Game1.PopScreen();
                    }
                }
            }

            m.Update(this, p);
            p.Update(m.GetCurrentRoom());
            if (p.GetState() == PlayerState.Eating)
            {
                deathPlayTicks++;
                if (deathPlayTicks > 75)
                {
                    Game1.PopScreen();
                    Game1.PushScreen(new ChangeScreen("Success."));
                }
            }
        }

        public override void Render(SpriteBatch sb)
        {
            m.Render(sb, p);
            sb.Draw(AssMan.Get(bodyAsset), new Rectangle((int)(bodyStart.X + ((dayTicks * (bodyDest.X - bodyStart.X)) / (float)dayTickLength)), (int)bodyStart.Y, 64, 64),
                null, Color.White, (float)(dayTicks * (Math.PI / 16)), new Vector2(32, 32), SpriteEffects.None, 0.0f);
        }
    }
}
