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

        public static bool UpKey()
        {
            return KeyDown(Keys.W) || KeyDown(Keys.Up);
        }

        public static bool DownKey()
        {
            return KeyDown(Keys.S) || KeyDown(Keys.Down);
        }

        public static bool LeftKey()
        {
            return KeyDown(Keys.A) || KeyDown(Keys.Left);
        }

        public static bool RightKey()
        {
            return KeyDown(Keys.D) || KeyDown(Keys.Right);
        }

        public static bool ActionKey()
        {
            return SemiAuto(Keys.E) || SemiAuto(Keys.Space);
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
