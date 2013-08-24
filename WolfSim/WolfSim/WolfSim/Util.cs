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
    class Util
    {
        public static int[] ShuffledIota(int n)
        {
            int[] iota = new int[n];
            for (int i = 0; i < n; i++)
            {
                iota[i] = i;
            }

            int a, b, t;
            for (int i = 0; i < n * 5; i++)
            {
                a = Game1.rand.Next() % n;
                b = Game1.rand.Next() % n;
                if (a != b)
                {
                    t = iota[a];
                    iota[a] = iota[b];
                    iota[b] = t;
                }
            }
            return iota;
        }

        public static Rectangle AddOffset(Rectangle r, Vector2 offset)
        {
            return new Rectangle((int)(r.X + offset.X), (int)(r.Y + offset.Y), r.Width, r.Height);
        }

        public static Vector2 AddOffset(Vector2 v, Vector2 offset)
        {
            return new Vector2(v.X + offset.X, v.Y + offset.Y);
        }

        public static Vector2 SubOffset(Vector2 v, Vector2 offset)
        {
            return new Vector2(v.X - offset.X, v.Y - offset.Y);
        }

        public static double Distance(Vector2 a, Vector2 b)
        {
            return Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));
        }
    }
}
