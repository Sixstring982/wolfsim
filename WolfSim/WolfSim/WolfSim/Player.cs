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
        WalkSouth,
        Dead,
        Eating
    }

    enum Item
    {
        None,
        Plunger,
        Mop,
        Goat,
        Matchbox //MUST BE THE END OF THE ENUM
    }

    class Player
    {
        public bool alive = true;
        private int soundTicks = 0;
        public const float speed = 3.0f;
        private const int animSpeed = 20;
        public Vector2 DrawOffset = new Vector2(-14, -63);
        private PlayerState state;
        Dictionary<PlayerState, Animation> anims;
        private Item heldItem = Item.None;
        private string killString = "";

        public Item GetItem()
        {
            return heldItem;
        }

        public Item PickUpItem(Item i)
        {
            Item ret = heldItem;
            heldItem = i;
            return ret;
        }

        public Player()
        {
            anims = new Dictionary<PlayerState, Animation>();
            anims.Add(PlayerState.Idle, new Animation(
                new IAsset[] { IAsset.Player_Idle }, 5));
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
            anims.Add(PlayerState.Dead, new Animation(
                new IAsset[]
                {
                    IAsset.Player_Dead
                }, animSpeed));
            anims.Add(PlayerState.Eating, new Animation(
                new IAsset[]
                {
                    IAsset.Player_Eat1,
                    IAsset.Player_Eat2,
                    IAsset.Player_Eat3,
                    IAsset.Player_Eat4,
                    IAsset.Player_Eat5,
                    IAsset.Player_Eat6
                }, 2));
        }

        public Rectangle GetRect()
        {
            return new Rectangle(0, 0, AssMan.Get(IAsset.Player_Idle).Width, AssMan.Get(IAsset.Player_Idle).Height);
        }

        public void Kill(string killObject)
        {
            killString = "Killed by " + killObject;
            alive = false;
            this.state = PlayerState.Dead;
            AssMan.Get(SAsset.Death).Play();
        }

        public void Eat()
        {
            state = PlayerState.Eating;
            AssMan.Get(SAsset.Crunch).Play();
            AssMan.Get(SAsset.Crunch).Play();
            SplashScreen.PeopleEaten++;
        }

        public void SetState(PlayerState state)
        {
            this.state = state;
        }

        public PlayerState GetState()
        {
            return state;
        }

        public void Update(Room currentRoom)
        {

        }

        public void Render(SpriteBatch sb, Vector2 location)
        {
            if (state == PlayerState.WalkEast ||
                state == PlayerState.WalkWest ||
                state == PlayerState.WalkNorth ||
                state == PlayerState.WalkSouth)
            {
                soundTicks++;
                if (soundTicks == animSpeed)
                {
                    AssMan.Get(Game1.rand.Next() % 2 == 0 ? SAsset.Step1 : SAsset.Step2).Play();
                    soundTicks = 0;
                }
            }
            anims[state].Render(sb, Util.AddOffset(DrawOffset, location));
            //sb.Draw(AssMan.Get(IAsset.Mask), Util.AddOffset(Util.AddOffset(GetRect(), location), DrawOffset), Color.Blue);
            sb.Draw(AssMan.Get(ItemIcon(heldItem)), new Vector2(100, 100), Color.White);
            if (!alive)
            {
                sb.DrawString(AssMan.victorianFont, killString, new Vector2(700, 400), Color.Red);
            }
        }

        public static IAsset ItemIcon(Item i)
        {
            switch (i)
            {
                case Item.Plunger: return IAsset.Plunger;
                case Item.Matchbox: return IAsset.Matchbox;
                case Item.Mop: return IAsset.Mop;
                case Item.Goat: return IAsset.Goat;
                default: return IAsset.Mask;
            }
        }
    }
}
