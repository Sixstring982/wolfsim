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
    class SplashScreen : Screen
    {
        public static int PeopleEaten = 0;
        public override void Render(SpriteBatch sb)
        {
            sb.Draw(AssMan.Get(IAsset.SplashScreen), Vector2.Zero, Color.White);
            sb.DrawString(AssMan.victorianFont, PeopleEaten + " Successes...", new Vector2(21, 21), Color.Black);
            sb.DrawString(AssMan.victorianFont, PeopleEaten + " Successes...", new Vector2(20, 20), Color.Red);
        }

        public override void Update()
        {
            if (KVMA_Keyboard.SemiAuto(Keys.Enter))
            {
                HouseScreen.bodyAsset = IAsset.Sun;
                Game1.PushScreen(new HouseScreen());
            }
        }

    }
}
