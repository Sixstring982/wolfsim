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
    class Room
    {
        private static int EXITW = 20,
                           EXITH = 48,
                           WALLH = 80;

        private static float MOVE_DISTANCE = 20.0f;

        protected class RoomExit
        {
            public VecDir location;
            public Room next;

            public RoomExit(VecDir location)
            {
                this.location = location;
                this.next = null;
            }

            public RoomExit(float x, float y, Direction d)
            {
                this.location = new VecDir(new Vector2(x, y), d);
            }
        }

        protected List<VecDir> entityPoints = new List<VecDir>();
        protected List<RoomExit> exitPoints = new List<RoomExit>();
        protected IAsset backgroundAsset;
        public Vector2 backgroundOrigin;
        protected int maxExits;

        protected void AddParent(Room parent, Direction d)
        {
            int[] iota = Util.ShuffledIota(exitPoints.Count);
            d = VecDir.OppositeDir(d);

            for (int i = 0; i < iota.Length; i++)
            {
                if (exitPoints[i].location.dir == d)
                {
                    exitPoints[i].next = parent;
                }
            }
        }

        public Room NearestExit(Vector2 location)
        {
            RoomExit nearest = null;
            double nearestDist = double.PositiveInfinity;
            for (int i = 0; i < exitPoints.Count; i++)
            {
                if (exitPoints[i].next != null)
                {
                    if (Util.Distance(location, exitPoints[i].location.vec) < nearestDist)
                    {
                        nearest = exitPoints[i];
                        nearestDist = Util.Distance(location, exitPoints[i].location.vec);
                    }
                }
            }
            return nearest.next;
        }

        protected void SetBackgroundParams()
        {
            Texture2D bgtex = AssMan.Get(backgroundAsset);
            backgroundOrigin = new Vector2((Game1.SCREENW - bgtex.Width) >> 1, (Game1.SCREENH - bgtex.Height) >> 1);
        }

        private delegate bool ExitSelector(RoomExit e);
        private delegate bool ExitComparator(RoomExit a, RoomExit b);

        private int ExitsLeft()
        {
            int ct = 0;
            foreach (RoomExit r in exitPoints)
            {
                if (r.next != null)
                {
                    ct++;
                }
            }
            return maxExits - ct;
        }

        private RoomExit NextExit()
        {
            if(ExitsLeft() > 0)
            {
                int[] ios = Util.ShuffledIota(exitPoints.Count);
                for (int i = 0; i < ios.Length; i++)
                {
                    if (exitPoints[ios[i]].next == null)
                    {
                        return exitPoints[ios[i]];
                    }
                }
            }
            return null;
        }

        public bool AddRoom(Room r)
        {
            RoomExit e = NextExit();
            if (e != null)
            {
                r.AddParent(this, e.location.dir);
                e.next = r;
            }
            return false;
        }

        public void Render(SpriteBatch sb)
        {
            sb.Draw(AssMan.Get(backgroundAsset), backgroundOrigin, Color.White);
            RenderExits(sb);
        }

        private void RenderWall(SpriteBatch sb, Direction d)
        {
            RoomExit[] exits = ExitFilter((e) => e.location.dir == d && e.next != null);
            switch (exits.Length)
            {
                case 0:
                    RenderNoExits(sb, d); break;
                case 1:
                    RenderOneExit(sb, exits[0], d); break;
                case 2:
                    RenderTwoSimilarExits(sb, exits, d); break;
            }

        }

        private void RenderExits(SpriteBatch sb)
        {
            RenderWall(sb, Direction.E);
            RenderWall(sb, Direction.S);
            RenderWall(sb, Direction.W);
            RenderNorthWall(sb);
        }

        private void RenderNorthWall(SpriteBatch sb)
        {
            RoomExit[] exits = ExitFilter((e) => e.location.dir == Direction.N && e.next != null);
            Texture2D doorAsset = AssMan.Get(IAsset.Door1);

            for (int i = 0; i < exits.Length; i++)
            {

                sb.Draw(doorAsset, Util.AddOffset(backgroundOrigin, new Vector2(exits[i].location.vec.X - (doorAsset.Width >> 1), WALLH - doorAsset.Height)), Color.White);
            }
        }

        private void RenderNoExits(SpriteBatch sb, Direction d)
        {
            Rectangle maskRect = new Rectangle(0, WALLH, EXITW, AssMan.Get(backgroundAsset).Height - WALLH);
            if (d == Direction.E)
            {
                maskRect.X = AssMan.Get(backgroundAsset).Width - EXITW;
            }
            else if (d == Direction.S)
            {
                maskRect = new Rectangle(0, AssMan.Get(backgroundAsset).Height - EXITW, AssMan.Get(backgroundAsset).Width, EXITW);
            }

            maskRect = Util.AddOffset(maskRect, backgroundOrigin);
            sb.Draw(AssMan.Get(IAsset.Mask), maskRect, Color.Black);
        }

        private void RenderOneExit(SpriteBatch sb, RoomExit e, Direction d)
        {
            Texture2D bgtex = AssMan.Get(backgroundAsset);
            Rectangle lowRect = new Rectangle(0, WALLH, EXITW, (int)(e.location.vec.Y - (EXITH >> 1) - WALLH));
            Rectangle highRect = new Rectangle(0, lowRect.Y + lowRect.Height + EXITH, EXITW, bgtex.Height - (lowRect.Height + lowRect.Y + EXITH));

            if (d == Direction.E)
            {
                highRect.X = lowRect.X = bgtex.Width - EXITW;
            }
            else if (d == Direction.S)
            {
                lowRect = new Rectangle(0, bgtex.Height - EXITW, (int)e.location.vec.X - (EXITH >> 1), EXITW);
                highRect = new Rectangle(lowRect.X + lowRect.Width + EXITH, lowRect.Y, bgtex.Width - (EXITH >> 1) - (int)e.location.vec.X, EXITW);
            }
            lowRect = Util.AddOffset(lowRect, backgroundOrigin);
            highRect = Util.AddOffset(highRect, backgroundOrigin);

            sb.Draw(AssMan.Get(IAsset.Mask), lowRect, Color.Black);
            sb.Draw(AssMan.Get(IAsset.Mask), highRect, Color.Black);
        }

        private void RenderTwoSimilarExits(SpriteBatch sb, RoomExit[] exits, Direction dir)
        {
            Texture2D bgtex = AssMan.Get(backgroundAsset);
            Rectangle lowRect, midRect, highRect;
            if (dir != Direction.S)
            {
                int x = dir == Direction.W ? 0 : bgtex.Width - EXITW;
                SwitchIf(exits, (a, b) => a.location.vec.Y > b.location.vec.Y);
                lowRect = new Rectangle(x, WALLH, EXITW, (int)exits[0].location.vec.Y - (EXITH >> 1) - WALLH);
                midRect = new Rectangle(x, lowRect.Y + lowRect.Height + EXITH, EXITW, (int)(exits[1].location.vec.Y - exits[0].location.vec.Y) - EXITH);
                highRect = new Rectangle(x, midRect.Y + midRect.Height + EXITH, EXITW, bgtex.Height - (midRect.Height + midRect.Y + EXITH));
            }
            else
            {
                SwitchIf(exits, (a, b) => a.location.vec.X > b.location.vec.X);
                lowRect = new Rectangle(0, bgtex.Height - EXITW, (int)exits[0].location.vec.X - (EXITH >> 1), EXITW);
                midRect = new Rectangle(lowRect.X + lowRect.Width + EXITH, lowRect.Y, (int)(exits[1].location.vec.X - exits[0].location.vec.X) - EXITH, EXITW);
                highRect = new Rectangle(midRect.X + midRect.Width + EXITH, midRect.Y, bgtex.Width - (midRect.Width + midRect.X + EXITH), EXITW);
            }
            lowRect = Util.AddOffset(lowRect, backgroundOrigin);
            midRect = Util.AddOffset(midRect, backgroundOrigin);
            highRect = Util.AddOffset(highRect, backgroundOrigin);

            sb.Draw(AssMan.Get(IAsset.Mask), lowRect, Color.Black);
            sb.Draw(AssMan.Get(IAsset.Mask), midRect, Color.Black);
            sb.Draw(AssMan.Get(IAsset.Mask), highRect, Color.Black);

        }

        private void SwitchIf(RoomExit[] es, ExitComparator f)
        {
            if (es.Length > 1)
            {
                RoomExit tmp;
                if (f(es[0], es[1]))
                {
                    tmp = es[0];
                    es[0] = es[1];
                    es[1] = tmp;
                }
            }
        }

        private RoomExit[] ExitFilter(ExitSelector f)
        {
            List<RoomExit> es = new List<RoomExit>();
            foreach (RoomExit e in exitPoints)
            {
                if (f(e))
                {
                    es.Add(e);
                }
            }
            return es.ToArray();
        }
    }

    class Parlor : Room
    {
        public Parlor()
        {
            this.maxExits = 2;
            this.backgroundAsset = IAsset.Room_Vertical;
            exitPoints.Add(new RoomExit(161, 70, Direction.N));
            exitPoints.Add(new RoomExit(10, 216, Direction.W));
            exitPoints.Add(new RoomExit(410, 216, Direction.E));
            exitPoints.Add(new RoomExit(200, 400, Direction.S));
            SetBackgroundParams();
        }
    }

    class Foyer : Room
    {
        public Foyer()
        {
            this.maxExits = 8;
            this.backgroundAsset = IAsset.Room_Foyer;
            exitPoints.Add(new RoomExit(8, 117, Direction.W));
            exitPoints.Add(new RoomExit(8, 380, Direction.W));
            exitPoints.Add(new RoomExit(91, 52, Direction.N));
            exitPoints.Add(new RoomExit(211, 45, Direction.N));
            exitPoints.Add(new RoomExit(365, 42, Direction.N));
            exitPoints.Add(new RoomExit(512, 45, Direction.N));
            exitPoints.Add(new RoomExit(594, 236, Direction.E));
            exitPoints.Add(new RoomExit(594, 413, Direction.E));
            SetBackgroundParams();
        }
    }

    class DiningRoom : Room
    {
        public DiningRoom()
        {
            this.maxExits = 2;
            this.backgroundAsset = IAsset.Room_Square;
            exitPoints.Add(new RoomExit(189, 41, Direction.N));
            exitPoints.Add(new RoomExit(6, 182, Direction.W));
            exitPoints.Add(new RoomExit(390, 182, Direction.E));
            exitPoints.Add(new RoomExit(83, 394, Direction.S));
            SetBackgroundParams();
        }
    }
}