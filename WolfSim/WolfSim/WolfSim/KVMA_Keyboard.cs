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
    class KVMA_Keyboard
    {
        private static KeyboardState current = Keyboard.GetState(),
                                     prev = Keyboard.GetState();

        public static bool KeyDown(Keys k)
        {
            return current.IsKeyDown(k);
        }

        public static bool KeyUp(Keys k)
        {
            return current.IsKeyUp(k);
        }

        public static bool SemiAuto(Keys k)
        {
            return current.IsKeyDown(k) && !prev.IsKeyDown(k);
        }

        public static void Flip()
        {
            prev = current;
            current = Keyboard.GetState();
        }
    }
}
