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
    class DialogScreen : Screen
    {
        Screen parent;
        string[] text;
        int drawTicks = 0;
        int maxDrawTicks = 3;
        int charsToDraw = 0;

        public DialogScreen(Screen parent, string[] text)
        {
            this.parent = parent;
            this.text = text;
        }

        public override void Update()
        {
            
        }

        private int totalChars()
        {
            int chrs = 0;
            for (int i = 0; i < text.Length; i++)
            {
                chrs += text.Length;
            }
            return chrs;
        }

        public override void Render(SpriteBatch sb)
        {
            drawTicks++;
            if(drawTicks > maxDrawTicks)
            {
                drawTicks = 0;
                charsToDraw++;
            }
            parent.Render(sb);

            int charsleft = charsToDraw;
            for(int i = 0; i < text.Length; i++)
            {
                RenderString(sb, i, charsleft);
                charsleft -= text[i].Length;
            }

            if (charsToDraw > totalChars())
            {
                sb.DrawString(AssMan.victorianSmall, "Press enter to continue...", new Vector2(1000, 0), Color.White);
                if (KVMA_Keyboard.SemiAuto(Keys.Enter))
                {
                    Game1.PopScreen();
                }
            }

        }

        private void RenderString(SpriteBatch sb, int strNum, int charsLeft)
        {
            if (charsLeft > 0)
            {
                Vector2 location = new Vector2(0, 600 + 24 * strNum);
                sb.DrawString(AssMan.victorianSmall, text[strNum].Substring(0, Math.Min(charsLeft, text[strNum].Length)), location, Color.White);
            }
        }
    }
}
