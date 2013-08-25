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
    enum PlayerState
    {
        Idle,
        WalkNorth,
        WalkEast,
        WalkWest,
        WalkSouth
    }

    class Player
    {

        public const float speed = 3.0f;
        private const int animSpeed = 20;
        private Vector2 DrawOffset = new Vector2(-14, -63);
        private PlayerState state;
        Dictionary<PlayerState, Animation> anims;

        public Player()
        {
            anims = new Dictionary<PlayerState, Animation>();
            anims.Add(PlayerState.Idle, new Animation(
                new IAsset[] { IAsset.Player_WalkSouth1 }, 5));
            anims.Add(PlayerState.WalkNorth, new Animation(
                new IAsset[]
                {
                    IAsset.Player_WalkNorth1,
                    IAsset.Player_WalkNorth2
                }, animSpeed));
            anims.Add(PlayerState.WalkSouth, new Animation(
                new IAsset[]
                {
                    IAsset.Player_WalkSouth1,
                    IAsset.Player_WalkSouth2
                }, animSpeed));
            anims.Add(PlayerState.WalkEast, new Animation(
                new IAsset[]
                {
                    IAsset.Player_WalkEast1,
                    IAsset.Player_WalkEast2
                }, animSpeed));
            anims.Add(PlayerState.WalkWest, new Animation(
                new IAsset[]
                {
                    IAsset.Player_WalkWest1,
                    IAsset.Player_WalkWest2
                }, animSpeed));
        }

        public void SetState(PlayerState state)
        {
            this.state = state;
        }

        public void Update(Room currentRoom)
        {

        }

        public void Render(SpriteBatch sb, Vector2 location)
        {
            anims[state].Render(sb, Util.AddOffset(DrawOffset, location));
        }
    }
}
