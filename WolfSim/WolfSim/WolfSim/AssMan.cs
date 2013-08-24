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
        Player_SouthWest,
        Room_Vertical,
        Room_Foyer,
        Room_Square,
        Room_Shed,
        Mask,
        Door1,
    }

    class AssMan
    {
        public static Dictionary<IAsset, Texture2D> IDict = new Dictionary<IAsset, Texture2D>();

        public static void LoadStaticAssets(ContentManager c)
        {
            //Code here looks like this:
            // IDict.Add(IAsset.ASSET_NAME_HERE, c.Load<Texture2D>("ASSET_STRING_FROM_CONTENT_HERE"));

            IDict.Add(IAsset.Player_SouthWest, c.Load<Texture2D>("Images/DRSHMO"));
            IDict.Add(IAsset.Room_Vertical, c.Load<Texture2D>("Images/Parlor"));
            IDict.Add(IAsset.Room_Foyer, c.Load<Texture2D>("Images/Foyer"));
            IDict.Add(IAsset.Room_Square, c.Load<Texture2D>("Images/SquareRoom"));
            IDict.Add(IAsset.Room_Shed, c.Load<Texture2D>("Images/Shed"));
            IDict.Add(IAsset.Mask, c.Load<Texture2D>("Images/mask"));
            IDict.Add(IAsset.Door1, c.Load<Texture2D>("Images/door1"));
        }

        public static Texture2D Get(IAsset a)
        {
            return IDict[a];
        }
    }
}
