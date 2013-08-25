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
    class Player
    {
        private enum PlayerState
        {
            Idle,
            WalkNorth,
            WalkEast,
            WalkWest,
            WalkSouth
        }

        public const float speed = 3.0f;
        private Vector2 DrawOffset = new Vector2(-14, -63);

        public void Update(Room currentRoom)
        {

        }

        public void Render(SpriteBatch sb, Vector2 location)
        {
            sb.Draw(AssMan.Get(IAsset.Player_SouthWest), Util.AddOffset(DrawOffset, location), Color.White);
        }
    }
}
