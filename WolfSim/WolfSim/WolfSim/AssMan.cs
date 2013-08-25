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
    enum IAsset
    {
        Player_WalkSouth1,
        Player_WalkSouth2,
        Player_WalkEast1,
        Player_WalkEast2,
        Player_WalkWest1,
        Player_WalkWest2,
        Player_WalkNorth1,
        Player_WalkNorth2,
        Room_Vertical,
        Room_Foyer,
        Room_Square,
        Room_Shed,
        Room_Horizontal,
        Mask,
        Door1,
        Foyer_Railing
    }

    class AssMan
    {
        public static Dictionary<IAsset, Texture2D> IDict = new Dictionary<IAsset, Texture2D>();

        public static void LoadStaticAssets(ContentManager c)
        {
            //Code here looks like this:
            // IDict.Add(IAsset.ASSET_NAME_HERE, c.Load<Texture2D>("ASSET_STRING_FROM_CONTENT_HERE"));

            IDict.Add(IAsset.Player_WalkSouth1, c.Load<Texture2D>("Images/WolfmanStillFront"));
            IDict.Add(IAsset.Room_Vertical, c.Load<Texture2D>("Images/Parlor"));
            IDict.Add(IAsset.Room_Foyer, c.Load<Texture2D>("Images/Foyer"));
            IDict.Add(IAsset.Room_Square, c.Load<Texture2D>("Images/SquareRoom"));
            IDict.Add(IAsset.Room_Horizontal, c.Load<Texture2D>("Images/Horizontal"));
            IDict.Add(IAsset.Room_Shed, c.Load<Texture2D>("Images/Shed"));
            IDict.Add(IAsset.Mask, c.Load<Texture2D>("Images/mask"));
            IDict.Add(IAsset.Door1, c.Load<Texture2D>("Images/door1"));
            IDict.Add(IAsset.Foyer_Railing, c.Load<Texture2D>("Images/railing"));
            IDict.Add(IAsset.Player_WalkEast1, c.Load<Texture2D>("Images/wolfManStillRight"));
            IDict.Add(IAsset.Player_WalkEast2, c.Load<Texture2D>("Images/wolfManWalkRight"));
            IDict.Add(IAsset.Player_WalkWest1, c.Load<Texture2D>("Images/wolfManStillLeft"));
            IDict.Add(IAsset.Player_WalkWest2, c.Load<Texture2D>("Images/wolfManWalkLeft"));
            IDict.Add(IAsset.Player_WalkNorth1, c.Load<Texture2D>("Images/wolfManStillRear"));
        }

        public static Texture2D Get(IAsset a)
        {
            return IDict[a];
        }
    }
}
