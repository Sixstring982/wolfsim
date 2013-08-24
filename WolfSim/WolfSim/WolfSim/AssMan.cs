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

    }

    class AssMan
    {
        public static Dictionary<IAsset, Texture2D> IDict = new Dictionary<IAsset, Texture2D>();

        public static void LoadStaticAssets(ContentManager c)
        {
            //Code here looks like this:
            // IDict.Add(IAsset.ASSET_NAME_HERE, c.Load<Texture2D>("ASSET_STRING_FROM_CONTENT_HERE"));
        }

        public static Texture2D Get(IAsset a)
        {
            return IDict[a];
        }
    }
}
