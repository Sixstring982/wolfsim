﻿using System;
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

            m.Update(p);
            p.Update(m.GetCurrentRoom());
        }

        public override void Render(SpriteBatch sb)
        {
            m.Render(sb, p);
        }
    }
}
