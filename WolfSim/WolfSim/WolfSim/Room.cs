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
    class RoomExit
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

    class Room
    {
        /// <summary>
        /// Something that will be drawn last in the scene.
        /// </summary>
        protected class AfterTexture
        {
            public IAsset asset;
            public Vector2 vector;

            public AfterTexture(IAsset asset, Vector2 vector)
            {
                this.asset = asset;
                this.vector = vector;
            }

            public void Render(SpriteBatch sb, Vector2 offset)
            {
                sb.Draw(AssMan.Get(asset), Util.AddOffset(offset, vector), Color.White);
            }
        }

        private static int EXITW = 20,
                           EXITH = 48,
                           WALLH = 80;

        private static float MOVE_DISTANCE = 20.0f;

        protected Vector2 playerPosition = Vector2.Zero;

        public string name = "NOT NAMED";

        public void ArmEntities()
        {
            if (HouseScreen.bodyAsset == IAsset.Moon)
            {
                foreach (Entity e in entities)
                {
                    e.Arm();
                }
            }
            else
            {
                foreach (Entity e in entities)
                {
                    e.Disarm();
                }
            }
        }

        public void SetPlayerPosition(Vector2 v)
        {
            playerPosition = v;
        }

        public Vector2 GetPlayerPosition()
        {
            return new Vector2(playerPosition.X, playerPosition.Y);
        }

        private int PlayerBottomY(Player p)
        {
            return (int)playerPosition.Y;
        }
        
        public void SetPlayerFromRoom(Room r)
        {
            for (int i = 0; i < exitPoints.Count; i++)
            {
                if (r == exitPoints[i].next)
                {
                    SetPlayerPosition(Util.MoveDirection(exitPoints[i].location.vec, VecDir.OppositeDir(exitPoints[i].location.dir), 50));
                }
            }
        }

        public bool IsPlayerOnExit()
        {
            for (int i = 0; i < exitPoints.Count; i++)
            {
                if (exitPoints[i].next != null)
                {
                    if (Util.Distance(exitPoints[i].location.vec, playerPosition) < MOVE_DISTANCE)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        protected List<Entity> entities = new List<Entity>();
        protected List<RoomExit> exitPoints = new List<RoomExit>();
        protected List<Rectangle> collisionBoxes = new List<Rectangle>();
        protected List<AfterTexture> afterTextures = new List<AfterTexture>();
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

        public Room NextLinker()
        {
            int[] iota = Util.ShuffledIota(exitPoints.Count);
            for (int i = 0; i < exitPoints.Count; i++)
            {
                if (exitPoints[iota[i]].next != null)
                {
                    if (exitPoints[iota[i]].next.ExitsLeft() > 0)
                    {
                        return exitPoints[iota[i]].next;
                    }
                }
            }
            return null;
        }

        public RoomExit NearestExit(Vector2 location)
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
            return nearest;
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
                return true;
            }
            return false;
        }

        public void Update(Screen masterScreen, Player p)
        {
            if (p.alive && p.GetState() != PlayerState.Eating)
            {
                bool moved = false;
                if (KVMA_Keyboard.UpKey())
                {
                    SetPlayerPosition(Util.MoveDirection(playerPosition, Direction.N, Player.speed));
                    if (InCollisionBoxes(playerPosition) || OutOfBounds(playerPosition))
                    {
                        SetPlayerPosition(Util.MoveDirection(playerPosition, Direction.S, Player.speed));
                    }
                    moved = true;
                    p.SetState(PlayerState.WalkNorth);
                }
                if (KVMA_Keyboard.DownKey())
                {
                    SetPlayerPosition(Util.MoveDirection(playerPosition, Direction.S, Player.speed));
                    if (InCollisionBoxes(playerPosition) || OutOfBounds(playerPosition))
                    {
                        SetPlayerPosition(Util.MoveDirection(playerPosition, Direction.N, Player.speed));
                    }
                    moved = true;
                    p.SetState(PlayerState.WalkSouth);
                }
                if (KVMA_Keyboard.LeftKey())
                {
                    SetPlayerPosition(Util.MoveDirection(playerPosition, Direction.W, Player.speed));
                    if (InCollisionBoxes(playerPosition) || OutOfBounds(playerPosition))
                    {
                        SetPlayerPosition(Util.MoveDirection(playerPosition, Direction.E, Player.speed));
                    }
                    moved = true;
                    p.SetState(PlayerState.WalkWest);
                }
                if (KVMA_Keyboard.RightKey())
                {
                    SetPlayerPosition(Util.MoveDirection(playerPosition, Direction.E, Player.speed));
                    if (InCollisionBoxes(playerPosition) || OutOfBounds(playerPosition))
                    {
                        SetPlayerPosition(Util.MoveDirection(playerPosition, Direction.W, Player.speed));
                    }
                    moved = true;
                    p.SetState(PlayerState.WalkEast);
                }
                if (!moved)
                {
                    p.SetState(PlayerState.Idle);
                }
            }

            List<Entity> newEntities = new List<Entity>();
            List<int> deadEnts = new List<int>();
            for(int i = 0; i < entities.Count; i++)
            {
                if (!entities[i].Update(newEntities, p, playerPosition))
                {
                    deadEnts.Add(i);
                }
                if (entities[i].InRange(playerPosition))
                {
                    entities[i].DoCollision(masterScreen, p, playerPosition);
                }
            }

            Entity[] ents = entities.ToArray();
            foreach (int i in deadEnts)
            {
                ents[i] = null;
            }
            entities.Clear();
            for (int i = 0; i < ents.Length; i++)
            {
                if (ents[i] != null)
                {
                    entities.Add(ents[i]);
                }
            }

            foreach (Entity e in newEntities)
            {
                entities.Add(e);
            }
        }

        protected void SortEntities()
        {
            Entity[] ens = entities.ToArray();
            Entity tmp;
            bool done = false;
            while (!done)
            {
                done = true;
                for(int i = 0; i < ens.Length - 1; i++)
                {
                    if (ens[i].BottomY() > ens[i + 1].BottomY())
                    {
                        done = false;
                        tmp = ens[i];
                        ens[i] = ens[i + 1];
                        ens[i + 1] = tmp;
                    }
                }
            }
            entities = new List<Entity>(ens);
        }

        public bool OutOfBounds(Vector2 v)
        {
            return !(v.X > EXITW && v.Y > WALLH && v.X < AssMan.Get(backgroundAsset).Width - EXITW && v.Y < AssMan.Get(backgroundAsset).Height - EXITW);
        }

        public bool InCollisionBoxes(Vector2 v)
        {
            foreach (Rectangle r in collisionBoxes)
            {
                if (r.Contains(new Point((int)v.X, (int)v.Y)))
                {
                    return true;
                }
            }
            foreach (Entity e in entities)
            {
                if (e.CollisionContains(v))
                {
                    return true;
                }
            }
            return false;
        }

        public void Render(SpriteBatch sb, Vector2 offset, Player p)
        {
            bool renderedPlayer = false;
            sb.Draw(AssMan.Get(backgroundAsset), Util.AddOffset(offset, backgroundOrigin), Color.White);
            RenderExits(sb, offset);

            for (int i = 0; i < entities.Count; i++)
            {
                if (entities[i].BottomY() > PlayerBottomY(p) && !renderedPlayer)
                {
                    renderedPlayer = true;
                    p.Render(sb, Util.AddOffset(backgroundOrigin, (Util.AddOffset(offset, playerPosition))));
                }
                entities[i].Render(sb, Util.AddOffset(backgroundOrigin, offset));
            }
            if (!renderedPlayer)
            {
                p.Render(sb, Util.AddOffset(backgroundOrigin, (Util.AddOffset(offset, playerPosition))));
            }

            foreach (AfterTexture a in afterTextures)
            {
                a.Render(sb, Util.AddOffset(backgroundOrigin, offset));
            }

            foreach (Entity e in entities)
            {
                if (e.InRange(playerPosition))
                {
                    e.RenderActivityString(sb, Util.AddOffset(playerPosition, Util.AddOffset(backgroundOrigin, offset)));
                }
            }
        }

        private void RenderWall(SpriteBatch sb, Direction d, Vector2 offset)
        {
            RoomExit[] exits = ExitFilter((e) => e.location.dir == d && e.next != null);
            switch (exits.Length)
            {
                case 0:
                    RenderNoExits(sb, d, offset); break;
                case 1:
                    RenderOneExit(sb, exits[0], d, offset); break;
                case 2:
                    RenderTwoSimilarExits(sb, exits, d, offset); break;
            }

        }

        private void RenderExits(SpriteBatch sb, Vector2 offset)
        {
            RenderWall(sb, Direction.E, offset);
            RenderWall(sb, Direction.S, offset);
            RenderWall(sb, Direction.W, offset);
            RenderNorthWall(sb, offset);
        }

        private void RenderNorthWall(SpriteBatch sb, Vector2 offset)
        {
            RoomExit[] exits = ExitFilter((e) => e.location.dir == Direction.N && e.next != null);
            Texture2D doorAsset = AssMan.Get(IAsset.Door1);

            for (int i = 0; i < exits.Length; i++)
            {
                sb.Draw(doorAsset, Util.AddOffset(backgroundOrigin, Util.AddOffset(offset, new Vector2(exits[i].location.vec.X - (doorAsset.Width >> 1), WALLH - doorAsset.Height))), Color.White);
            }
        }

        private void RenderNoExits(SpriteBatch sb, Direction d, Vector2 offset)
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
            sb.Draw(AssMan.Get(IAsset.Mask), Util.AddOffset(maskRect, offset), Color.Black);
        }

        private void RenderOneExit(SpriteBatch sb, RoomExit e, Direction d, Vector2 offset)
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

            sb.Draw(AssMan.Get(IAsset.Mask), Util.AddOffset(lowRect, offset), Color.Black);
            sb.Draw(AssMan.Get(IAsset.Mask), Util.AddOffset(highRect, offset), Color.Black);
        }

        private void RenderTwoSimilarExits(SpriteBatch sb, RoomExit[] exits, Direction dir, Vector2 offset)
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

            sb.Draw(AssMan.Get(IAsset.Mask), Util.AddOffset(lowRect, offset), Color.Black);
            sb.Draw(AssMan.Get(IAsset.Mask), Util.AddOffset(midRect, offset), Color.Black);
            sb.Draw(AssMan.Get(IAsset.Mask), Util.AddOffset(highRect, offset), Color.Black);

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
            exitPoints.Add(new RoomExit(161, 80, Direction.N));
            exitPoints.Add(new RoomExit(10, 216, Direction.W));
            exitPoints.Add(new RoomExit(310, 216, Direction.E));
            exitPoints.Add(new RoomExit(200, 400, Direction.S));
            SetBackgroundParams();

            entities.Add(new Mother(new Vector2(249, 97)));
            entities.Add(new Plant(new Vector2(36, 52)));
            entities.Add(new Plant(new Vector2(36, 335)));
            entities.Add(new Plant(new Vector2(267, 335)));
            entities.Add(new Plant(new Vector2(267, 52)));
            entities.Add(new Chair(new Vector2(56, 43)));
            entities.Add(new Chair(new Vector2(79, 43)));
            entities.Add(new Chair(new Vector2(241, 43)));
            entities.Add(new Chair(new Vector2(264, 43)));
            entities.Add(new Pillar(new Vector2(147, 236)));

            SortEntities();

            name = "Parlor";
        }
    }

    class Foyer : Room
    {
        public void ResetPlayerPos()
        {
            SetPlayerPosition(new Vector2(319, 418));
            ArmEntities();
        }

        public Foyer()
        {
            this.maxExits = 8;
            this.backgroundAsset = IAsset.Room_Foyer;
            exitPoints.Add(new RoomExit(8, 117, Direction.W));
            exitPoints.Add(new RoomExit(8, 380, Direction.W));
            exitPoints.Add(new RoomExit(91, 80, Direction.N));
            exitPoints.Add(new RoomExit(211, 80, Direction.N));
            exitPoints.Add(new RoomExit(365, 80, Direction.N));
            exitPoints.Add(new RoomExit(512, 80, Direction.N));
            exitPoints.Add(new RoomExit(594, 236, Direction.E));
            exitPoints.Add(new RoomExit(594, 413, Direction.E));
            collisionBoxes.Add(new Rectangle(81, 160, 16, 154));
            collisionBoxes.Add(new Rectangle(81, 160, 519, 55));
            afterTextures.Add(new AfterTexture(IAsset.Foyer_Railing, new Vector2(79, 125)));
            SetBackgroundParams();

            entities.Add(new Pillar(new Vector2(499, 267)));
            entities.Add(new Pillar(new Vector2(199, 267)));

            ResetPlayerPos();
            SortEntities();

            name = "Foyer";
        }
    }

    class DiningRoom : Room
    {
        public DiningRoom()
        {
            this.maxExits = 2;
            this.backgroundAsset = IAsset.Room_Square;
            exitPoints.Add(new RoomExit(189, 80, Direction.N));
            exitPoints.Add(new RoomExit(6, 182, Direction.W));
            exitPoints.Add(new RoomExit(390, 182, Direction.E));
            exitPoints.Add(new RoomExit(83, 394, Direction.S));
            SetBackgroundParams();

            entities.Add(new Bookshelf(new Vector2(324, 9)));
            entities.Add(new Bookshelf(new Vector2(27, 9)));
            entities.Add(new Pillar(new Vector2(183, 166)));

            SortEntities();

            name = "Dining Room";
        }
    }

    class Shed : Room
    {
        public Shed()
        {
            this.maxExits = 1;
            this.backgroundAsset = IAsset.Room_Shed;
            exitPoints.Add(new RoomExit(117, 80, Direction.N));
            exitPoints.Add(new RoomExit(10, 157, Direction.W));
            exitPoints.Add(new RoomExit(230, 157, Direction.E));
            exitPoints.Add(new RoomExit(110, 230, Direction.S));
            SetBackgroundParams();

            entities.Add(new Barrel(new Vector2(20, 80)));
            entities.Add(new Barrel(new Vector2(50, 80)));
            entities.Add(new Barrel(new Vector2(80, 80)));
            entities.Add(new Barrel(new Vector2(110, 80)));
            entities.Add(new Barrel(new Vector2(180, 150)));
            entities.Add(new Barrel(new Vector2(150, 150)));
            entities.Add(new Barrel(new Vector2(120, 150)));
            entities.Add(new Barrel(new Vector2(90, 150)));

            name = "Storage Room";
        }
    }

    class Bedroom : Room
    {
        public Bedroom()
        {
            this.maxExits = 2;
            this.backgroundAsset = IAsset.Room_Vertical;
            exitPoints.Add(new RoomExit(161, 80, Direction.N));
            exitPoints.Add(new RoomExit(10, 216, Direction.W));
            exitPoints.Add(new RoomExit(310, 216, Direction.E));
            exitPoints.Add(new RoomExit(200, 400, Direction.S));
            SetBackgroundParams();

            entities.Add(new Bookshelf(new Vector2(234, 8)));
            entities.Add(new Bookshelf(new Vector2(33, 8)));
            entities.Add(new Chair(new Vector2(81, 49)));
            entities.Add(new Chair(new Vector2(206, 49)));
            entities.Add(new EmptyBed(new Vector2(20, 238)));

            SortEntities();

            name = "Bedroom";
        }
    }

    class Breakfast : Room
    {
        public Breakfast()
        {
            this.maxExits = 2;
            this.backgroundAsset = IAsset.Room_Vertical;
            exitPoints.Add(new RoomExit(161, 80, Direction.N));
            exitPoints.Add(new RoomExit(10, 216, Direction.W));
            exitPoints.Add(new RoomExit(310, 216, Direction.E));
            exitPoints.Add(new RoomExit(200, 400, Direction.S));
            SetBackgroundParams();

            entities.Add(new Fountain(new Vector2(130, 93)));
            entities.Add(new Fountain(new Vector2(135, 294)));
            entities.Add(new Table(new Vector2(112, 169)));
            entities.Add(new Plant(new Vector2(193, 94)));
            entities.Add(new Plant(new Vector2(99, 91)));
            entities.Add(new Plant(new Vector2(101, 297)));
            entities.Add(new Plant(new Vector2(198, 297)));
            entities.Add(new Chair(new Vector2(90, 178)));
            entities.Add(new Chair(new Vector2(204, 185)));

            SortEntities();

            name = "Breakfast Nook";
        }
    }

    class Study : Room
    {
        public Study()
        {
            this.maxExits = 1;
            this.backgroundAsset = IAsset.Room_Vertical;
            exitPoints.Add(new RoomExit(161, 80, Direction.N));
            exitPoints.Add(new RoomExit(10, 216, Direction.W));
            exitPoints.Add(new RoomExit(310, 216, Direction.E));
            exitPoints.Add(new RoomExit(200, 400, Direction.S));
            SetBackgroundParams();

            entities.Add(new Bookshelf(new Vector2(33, 8)));
            entities.Add(new Bookshelf(new Vector2(249, 8)));
            entities.Add(new Bookshelf(new Vector2(201, 8)));

            entities.Add(new Desk(new Vector2(20, 254)));
            entities.Add(new Confuscious(new Vector2(228, 108)));

            SortEntities();

            name = "Study";
        }
    }

    class BilliardRoom : Room
    {
        public BilliardRoom()
        {
            this.maxExits = 2;
            this.backgroundAsset = IAsset.Room_Square;
            exitPoints.Add(new RoomExit(189, 80, Direction.N));
            exitPoints.Add(new RoomExit(6, 182, Direction.W));
            exitPoints.Add(new RoomExit(390, 182, Direction.E));
            exitPoints.Add(new RoomExit(83, 394, Direction.S));
            SetBackgroundParams();

            entities.Add(new PoolTable(new Vector2(106, 185)));
            entities.Add(new PoolPanel(new Vector2(60, 3)));

            SortEntities();

            name = "Billiard Room";
        }
    }

    class MasterBedroom : Room
    {
        public MasterBedroom()
        {
            this.maxExits = 2;
            this.backgroundAsset = IAsset.Room_Square;
            exitPoints.Add(new RoomExit(189, 80, Direction.N));
            exitPoints.Add(new RoomExit(6, 182, Direction.W));
            exitPoints.Add(new RoomExit(390, 182, Direction.E));
            exitPoints.Add(new RoomExit(260, 394, Direction.S));
            SetBackgroundParams();

            entities.Add(new Bed(new Vector2(20, 80)));
            entities.Add(new Father(new Vector2(50, 62)));
            entities.Add(new Bookshelf(new Vector2(324, 9)));
            entities.Add(new Bookshelf(new Vector2(267, 9)));
            entities.Add(new Bookshelf(new Vector2(219, 9)));

            name = "Master Bedroom";
        }
    }

    class Lab : Room
    {
        public Lab()
        {
            this.maxExits = 2;
            this.backgroundAsset = IAsset.Room_Horizontal;
            exitPoints.Add(new RoomExit(101, 80, Direction.N));
            exitPoints.Add(new RoomExit(10, 158, Direction.W));
            exitPoints.Add(new RoomExit(590, 182, Direction.E));
            exitPoints.Add(new RoomExit(538, 230, Direction.S));
            SetBackgroundParams();

            entities.Add(new Toilet(new Vector2(289, 160)));
            entities.Add(new Pillar(new Vector2(140, 12)));
            entities.Add(new Pillar(new Vector2(479, 12)));

            SortEntities();

            name = "Laboratory";
        }
    }

    class Kitchen : Room
    {
        public Kitchen()
        {
            this.maxExits = 2;
            this.backgroundAsset = IAsset.Room_Horizontal;
            exitPoints.Add(new RoomExit(101, 80, Direction.N));
            exitPoints.Add(new RoomExit(10, 158, Direction.W));
            exitPoints.Add(new RoomExit(590, 182, Direction.E));
            exitPoints.Add(new RoomExit(538, 230, Direction.S));
            SetBackgroundParams();

            SortEntities();

            name = "Kitchen";
        }
    }

    class SittingRoom : Room
    {
        public SittingRoom()
        {
            this.maxExits = 2;
            this.backgroundAsset = IAsset.Room_Horizontal;
            exitPoints.Add(new RoomExit(101, 80, Direction.N));
            exitPoints.Add(new RoomExit(10, 158, Direction.W));
            exitPoints.Add(new RoomExit(590, 182, Direction.E));
            exitPoints.Add(new RoomExit(538, 230, Direction.S));
            SetBackgroundParams();

            entities.Add(new Couch(new Vector2(300, 27)));
            entities.Add(new Portrait(new Vector2(216, 1)));
            entities.Add(new Bookshelf(new Vector2(151, 7)));

            SortEntities();


            name = "Sitting Room";
        }
    }

    class Backyard : Room
    {
        public Backyard()
        {
            this.maxExits = 1;
            this.backgroundAsset = IAsset.Room_Backyard;
            exitPoints.Add(new RoomExit(150, 190, Direction.S));
            exitPoints.Add(new RoomExit(10, 130, Direction.W));
            exitPoints.Add(new RoomExit(310, 130, Direction.E));
            exitPoints.Add(new RoomExit(134, 80, Direction.N));

            SortEntities();

            SetBackgroundParams();

            name = "Back Yard";
        }
    }

    class Bathroom : Room
    {
        public Bathroom()
        {
            this.maxExits = 1;
            this.backgroundAsset = IAsset.Bathroom;
            exitPoints.Add(new RoomExit(100, 80, Direction.N));
            exitPoints.Add(new RoomExit(10, 100, Direction.W));
            exitPoints.Add(new RoomExit(190, 100, Direction.E));
            exitPoints.Add(new RoomExit(100, 190, Direction.S));
            SetBackgroundParams();

            entities.Add(new Toilet(new Vector2(133, 123)));
            entities.Add(new Barrel(new Vector2(20, 125)));
            SortEntities();

            name = "Water Closet";
        }
    }
}