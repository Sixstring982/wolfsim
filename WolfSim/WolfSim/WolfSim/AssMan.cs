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
        Player_Idle,
        Player_WalkSouth1,
        Player_WalkSouth2,
        Player_WalkEast1,
        Player_WalkEast2,
        Player_WalkWest1,
        Player_WalkWest2,
        Player_WalkNorth1,
        Player_WalkNorth2,
        Player_Dead,
        Player_Eat1,
        Player_Eat2,
        Player_Eat3,
        Player_Eat4,
        Player_Eat5,
        Player_Eat6,
        Room_Vertical,
        Room_Foyer,
        Room_Square,
        Room_Shed,
        Room_Horizontal,
        Room_Backyard,
        Mask,
        Door1,
        Foyer_Railing,
        Portrait1,
        Bookshelf,
        Bookshelf_Burned,
        Column_Unarmed,
        Column_East,
        Column_West,
        Column_South,
        Book_Blue_Closed,
        Book_Blue_Open,
        Book_Green_Closed,
        Book_Green_Open,
        Book_Red_Closed,
        Book_Red_Open,
        Laser_North,
        Laser_East,
        Laser_South,
        Laser_West,
        SplashScreen,
        Barrel,
        Plunger,
        Toilet,
        Toilet_Clogged,
        Fireball,
        Matchbox,
        Mop,
        Desk,
        Confuscious,
        Couch,
        Sun,
        Moon,
        Father,
        Bed,
        Bed_Sleep,
        Bathroom,
        Mother,
        PoolTable,
        PoolPanel,
        BlackBall,
        BlueBallStripe,
        BlueBall,
        YellowBallStripe,
        YellowBall,
        RedBallStripe,
        RedBall,
        DefenestratedPoolTable,
        Goat,
        Plant,
        Chair,
        TableSuck,
        Fountain1,
        Fountain2,
        Fountain3
    }

    enum SAsset
    {
        Laser,
        Death,
        Step1,
        Step2,
        Mystery,
        Flap1,
        Flap2,
        Flap3,
        Crunch,
    }

    class AssMan
    {
        private static Dictionary<IAsset, Texture2D> IDict = new Dictionary<IAsset, Texture2D>();
        private static Dictionary<SAsset, SoundEffect> SDict = new Dictionary<SAsset, SoundEffect>();

        public static SpriteFont victorianFont;
        public static SpriteFont victorianSmall;

        public static void LoadStaticAssets(ContentManager c)
        {
            //Code here looks like this:
            // IDict.Add(IAsset.ASSET_NAME_HERE, c.Load<Texture2D>("ASSET_STRING_FROM_CONTENT_HERE"));

            IDict.Add(IAsset.Player_Idle, c.Load<Texture2D>("Images/WolfmanStillFront"));
            IDict.Add(IAsset.Room_Vertical, c.Load<Texture2D>("Images/Parlor"));
            IDict.Add(IAsset.Room_Foyer, c.Load<Texture2D>("Images/Foyer2"));
            IDict.Add(IAsset.Room_Square, c.Load<Texture2D>("Images/SquareRoom"));
            IDict.Add(IAsset.Room_Horizontal, c.Load<Texture2D>("Images/Horizontal"));
            IDict.Add(IAsset.Room_Shed, c.Load<Texture2D>("Images/Shed"));
            IDict.Add(IAsset.Room_Backyard, c.Load<Texture2D>("Images/BackYard2"));
            IDict.Add(IAsset.Mask, c.Load<Texture2D>("Images/mask"));
            IDict.Add(IAsset.Door1, c.Load<Texture2D>("Images/door1"));
            IDict.Add(IAsset.Foyer_Railing, c.Load<Texture2D>("Images/railing"));
            IDict.Add(IAsset.Player_WalkEast1, c.Load<Texture2D>("Images/wolfManStillRight"));
            IDict.Add(IAsset.Player_WalkEast2, c.Load<Texture2D>("Images/wolfManWalkRight"));
            IDict.Add(IAsset.Player_WalkWest1, c.Load<Texture2D>("Images/wolfManStillLeft"));
            IDict.Add(IAsset.Player_WalkWest2, c.Load<Texture2D>("Images/wolfManWalkLeft"));
            IDict.Add(IAsset.Player_WalkSouth1, c.Load<Texture2D>("Images/wolfManWalkFront1"));
            IDict.Add(IAsset.Player_WalkSouth2, c.Load<Texture2D>("Images/wolfManWalkFront2"));
            IDict.Add(IAsset.Player_WalkNorth1, c.Load<Texture2D>("Images/wolfManWalkRear1"));
            IDict.Add(IAsset.Player_WalkNorth2, c.Load<Texture2D>("Images/wolfManWalkRear2"));
            IDict.Add(IAsset.Portrait1, c.Load<Texture2D>("Images/portrait1"));
            IDict.Add(IAsset.Bookshelf, c.Load<Texture2D>("Images/bookshelf"));
            IDict.Add(IAsset.Bookshelf_Burned, c.Load<Texture2D>("Images/bookshelf_burned"));
            IDict.Add(IAsset.Column_Unarmed, c.Load<Texture2D>("Images/column"));
            IDict.Add(IAsset.Book_Blue_Open, c.Load<Texture2D>("Images/BlueFB_OPEN"));
            IDict.Add(IAsset.Book_Blue_Closed, c.Load<Texture2D>("Images/BlueFB_CLOSED"));
            IDict.Add(IAsset.Book_Green_Open, c.Load<Texture2D>("Images/GreenFB_OPEN"));
            IDict.Add(IAsset.Book_Green_Closed, c.Load<Texture2D>("Images/GreenFB_CLOSED"));
            IDict.Add(IAsset.Book_Red_Open, c.Load<Texture2D>("Images/RedFB_OPEN"));
            IDict.Add(IAsset.Book_Red_Closed, c.Load<Texture2D>("Images/RedFB_CLOSED"));
            IDict.Add(IAsset.Player_Dead, c.Load<Texture2D>("Images/WolfmanDead"));
            IDict.Add(IAsset.Column_East, c.Load<Texture2D>("Images/ARMED_columnEast"));
            IDict.Add(IAsset.Column_West, c.Load<Texture2D>("Images/ARMED_columnWest"));
            IDict.Add(IAsset.Column_South, c.Load<Texture2D>("Images/ARMED_column"));
            IDict.Add(IAsset.Laser_North, c.Load<Texture2D>("Images/LAZR_NORTH_1"));
            IDict.Add(IAsset.Laser_East, c.Load<Texture2D>("Images/LAZR_EAST_1"));
            IDict.Add(IAsset.Laser_South, c.Load<Texture2D>("Images/LAZR_SOUTH_1"));
            IDict.Add(IAsset.Laser_West, c.Load<Texture2D>("Images/LAZR_WEST_1"));
            IDict.Add(IAsset.SplashScreen, c.Load<Texture2D>("Images/SPLASHFINAL"));
            IDict.Add(IAsset.Barrel, c.Load<Texture2D>("Images/Barrel"));
            IDict.Add(IAsset.Plunger, c.Load<Texture2D>("Images/Plunger"));
            IDict.Add(IAsset.Toilet, c.Load<Texture2D>("Images/Toilet"));
            IDict.Add(IAsset.Toilet_Clogged, c.Load<Texture2D>("Images/Toilet_DISARMED"));
            IDict.Add(IAsset.Fireball, c.Load<Texture2D>("Images/FIRE_NORTH"));
            IDict.Add(IAsset.Matchbox, c.Load<Texture2D>("Images/matchBox"));
            IDict.Add(IAsset.Mop, c.Load<Texture2D>("Images/Mop"));
            IDict.Add(IAsset.Desk, c.Load<Texture2D>("Images/Desk"));
            IDict.Add(IAsset.Confuscious, c.Load<Texture2D>("Images/Confuscious"));
            IDict.Add(IAsset.Couch, c.Load<Texture2D>("Images/Couch"));
            IDict.Add(IAsset.Sun, c.Load<Texture2D>("Images/Sun"));
            IDict.Add(IAsset.Moon, c.Load<Texture2D>("Images/Moon"));
            IDict.Add(IAsset.Father, c.Load<Texture2D>("Images/Man"));
            IDict.Add(IAsset.Bed, c.Load<Texture2D>("Images/bed"));
            IDict.Add(IAsset.Bed_Sleep, c.Load<Texture2D>("Images/bed-sleep"));
            IDict.Add(IAsset.Player_Eat1, c.Load<Texture2D>("Images/WolfmanStillEatAni1"));
            IDict.Add(IAsset.Player_Eat2, c.Load<Texture2D>("Images/WolfmanStillEatAni2"));
            IDict.Add(IAsset.Player_Eat3, c.Load<Texture2D>("Images/WolfmanStillEatAni3"));
            IDict.Add(IAsset.Player_Eat4, c.Load<Texture2D>("Images/WolfmanStillEatAni4"));
            IDict.Add(IAsset.Player_Eat5, c.Load<Texture2D>("Images/WolfmanStillEatAni5"));
            IDict.Add(IAsset.Player_Eat6, c.Load<Texture2D>("Images/WolfmanStillEatAni6"));
            IDict.Add(IAsset.Bathroom, c.Load<Texture2D>("Images/Bathroom2"));
            IDict.Add(IAsset.Mother, c.Load<Texture2D>("Images/Lady"));
            IDict.Add(IAsset.PoolTable, c.Load<Texture2D>("Images/Billiards"));
            IDict.Add(IAsset.PoolPanel, c.Load<Texture2D>("Images/PoolEq"));
            IDict.Add(IAsset.BlackBall, c.Load<Texture2D>("Images/BlackBall"));
            IDict.Add(IAsset.BlueBall, c.Load<Texture2D>("Images/BlueBall"));
            IDict.Add(IAsset.BlueBallStripe, c.Load<Texture2D>("Images/BlueBallStripe"));
            IDict.Add(IAsset.YellowBall, c.Load<Texture2D>("Images/YellowBall"));
            IDict.Add(IAsset.YellowBallStripe, c.Load<Texture2D>("Images/YellowBallStripe"));
            IDict.Add(IAsset.RedBall, c.Load<Texture2D>("Images/RedBall"));
            IDict.Add(IAsset.RedBallStripe, c.Load<Texture2D>("Images/RedBallStripe"));
            IDict.Add(IAsset.DefenestratedPoolTable, c.Load<Texture2D>("Images/DefnPoolTable"));
            IDict.Add(IAsset.Goat, c.Load<Texture2D>("Images/Goat"));
            IDict.Add(IAsset.Plant, c.Load<Texture2D>("Images/Plant"));
            IDict.Add(IAsset.Chair, c.Load<Texture2D>("Images/Chair"));
            IDict.Add(IAsset.TableSuck, c.Load<Texture2D>("Images/tablesuck"));
            IDict.Add(IAsset.Fountain1, c.Load<Texture2D>("Images/Fountain1"));
            IDict.Add(IAsset.Fountain2, c.Load<Texture2D>("Images/Fountain2"));
            IDict.Add(IAsset.Fountain3, c.Load<Texture2D>("Images/Fountain3"));

            victorianFont = c.Load<SpriteFont>("Victorian");
            victorianSmall = c.Load<SpriteFont>("VictorianSmall");

            SDict.Add(SAsset.Laser, c.Load<SoundEffect>("Sounds/Lazer"));
            SDict.Add(SAsset.Death, c.Load<SoundEffect>("Sounds/Died"));
            SDict.Add(SAsset.Step1, c.Load<SoundEffect>("Sounds/Step1"));
            SDict.Add(SAsset.Step2, c.Load<SoundEffect>("Sounds/Step2"));
            SDict.Add(SAsset.Mystery, c.Load<SoundEffect>("Sounds/Mystery"));
            SDict.Add(SAsset.Flap1, c.Load<SoundEffect>("Sounds/Flap1"));
            SDict.Add(SAsset.Flap2, c.Load<SoundEffect>("Sounds/Flap2"));
            SDict.Add(SAsset.Flap3, c.Load<SoundEffect>("Sounds/Flap3"));
            SDict.Add(SAsset.Crunch, c.Load<SoundEffect>("Sounds/Crunch"));

            
        }

        public static Texture2D Get(IAsset a)
        {
            return IDict[a];
        }

        public static SoundEffect Get(SAsset a)
        {
            return SDict[a];
        }
    }
}
