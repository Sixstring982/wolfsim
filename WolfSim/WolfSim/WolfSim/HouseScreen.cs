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

        int deathPlayTicks = 0;
        int deathPlays = 0;

        public override void Update()
        {
            if (KVMA_Keyboard.SemiAuto(Keys.R))
            {
                m = new Map();
            }

            if (KVMA_Mouse.SemiAuto(0))
            {
                m.ChangeRoom(Util.SubOffset(KVMA_Mouse.Location(), m.GetCurrentRoom().backgroundOrigin));
            }

            if (!p.alive)
            {
                deathPlayTicks++;
                if (deathPlayTicks > 30)
                {
                    deathPlays++;
                    deathPlayTicks = 0;
                    AssMan.Get(SAsset.Death).Play();
                    if (deathPlays == 5)
                    {
                        Game1.PopScreen();
                    }
                }
            }

            m.Update(this, p);
            p.Update(m.GetCurrentRoom());
        }

        public override void Render(SpriteBatch sb)
        {
            m.Render(sb, p);
        }
    }
}
