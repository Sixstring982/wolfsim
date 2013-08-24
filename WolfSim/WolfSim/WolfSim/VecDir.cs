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
    enum Direction
    {
        N, E, S, W
    }

    class VecDir
    {
        public static Direction OppositeDir(Direction d)
        {
            return (Direction)((int)(d + 2) % 4);
        }

        public Vector2 vec;
        public Direction dir;

        public VecDir(Vector2 vec, Direction dir)
        {
            this.vec = vec;
            this.dir = dir;
        }
    }
}
