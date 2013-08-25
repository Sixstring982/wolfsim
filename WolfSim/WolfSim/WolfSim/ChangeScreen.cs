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
    class ChangeScreen : Screen
    {
        private string txt;
        private int liveTicks = 0;
        private int maxLiveTicks = 100;
        public ChangeScreen(string txt)
        {
            this.txt = txt;
            AssMan.Get(SAsset.Mystery).Play();
        }

        public override void Update()
        {
            liveTicks++;
            if (liveTicks > maxLiveTicks)
            {
                Game1.PopScreen();
            }
        }

        public override void Render(SpriteBatch sb)
        {
            sb.DrawString(AssMan.victorianFont, txt, new Vector2((Game1.SCREENW >> 1) - 100, (Game1.SCREENH >> 1) - 20), Color.White);
        }
    }
}
