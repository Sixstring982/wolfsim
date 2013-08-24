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
    class KVMA_Mouse
    {
        private static MouseState current, prev;

        public static void Flip()
        {
            prev = current;
            current = Mouse.GetState();
        }

        //0 - Left, 1 - mid, 2 - right
        public static bool SemiAuto(int button)
        {
            switch (button)
            {
                case 0:
                    return (current.LeftButton == ButtonState.Pressed) && (prev.LeftButton == ButtonState.Released);
                case 1:
                    return (current.MiddleButton == ButtonState.Pressed) && (prev.MiddleButton == ButtonState.Released);
                case 2:
                    return (current.RightButton == ButtonState.Pressed) && (prev.RightButton == ButtonState.Released);
            }
            return false;
        }

        public static Vector2 Location()
        {
            return new Vector2(current.X, current.Y);
        }
    }
}
