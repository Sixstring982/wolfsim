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
    abstract class Entity
    {
        public const float ENTITY_RANGE = 20.0f;
        protected Rectangle collisionBox;
        protected Vector2 location;
        protected Vector2 hitOffset;
        protected int selectedAnim;
        protected Animation[] anims;

        protected bool armed = false;

        public void Arm()
        {
            armed = true;
        }

        public void Disarm()
        {
            armed = false;
        }

        public Vector2 GetLocation()
        {
            return new Vector2(location.X, location.Y);
        }

        public Entity(Vector2 location)
        {
            this.location = location;
        }

        public int BottomY()
        {
            return (int)(location.Y + collisionBox.Y);
        }

        public string activityString = "";

        public abstract bool Update(List<Entity> entities, Player p, Vector2 playerPos);

        public abstract void DoCollision(Screen s, Player p, Vector2 playerPos);

        public bool CollisionContains(Vector2 v)
        {
            return Util.AddOffset(collisionBox, location).Contains(new Point((int)v.X, (int)v.Y));
        }

        public void Render(SpriteBatch sb, Vector2 offset)
        {
            anims[selectedAnim].Render(sb, Util.AddOffset(offset, location));
            //sb.Draw(AssMan.Get(IAsset.Mask), Util.AddOffset(Util.AddOffset(collisionBox, location), offset), Color.Red);
        }

        public void RenderActivityString(SpriteBatch sb, Vector2 offset)
        {
            sb.DrawString(AssMan.victorianFont, activityString, offset, Color.White);
        }

        public bool InRange(Vector2 v)
        {
            if (Util.Distance(v, Util.AddOffset(location, hitOffset)) < ENTITY_RANGE)
            {
                return true;
            }
            return false;
        }
    }

    class FlyingBook : Entity
    {
        private int flySpeed = 5;
        public FlyingBook(Vector2 location) :
            base(location)
        {
            selectedAnim = Game1.rand.Next() % 3;
            anims = new Animation[]
            {
                new Animation(new IAsset[]
                {
                    IAsset.Book_Blue_Open,
                    IAsset.Book_Blue_Closed
                }, 3),
                new Animation(new IAsset[]
                {
                    IAsset.Book_Green_Open,
                    IAsset.Book_Green_Closed
                }, 3),
                new Animation(new IAsset[]
                {
                    IAsset.Book_Red_Open,
                    IAsset.Book_Red_Closed
                }, 3)
            };
            hitOffset = new Vector2(9, 10);
            collisionBox = new Rectangle(1, 0, 16, 18);
        }


        public override bool Update(List<Entity> entities, Player p, Vector2 playerPos)
        {
            location.Y += flySpeed;
            Rectangle scHitOff = Util.AddOffset(collisionBox, location);
            if (Game1.rand.Next() % 75 == 0)
            {
                AssMan.Get((Game1.rand.Next() % 3) + SAsset.Flap1).Play();
            }
            if (scHitOff.Intersects(Util.AddOffset(Util.AddOffset(p.GetRect(), playerPos), p.DrawOffset)))
            {
                p.Kill();
            }
            if (location.Y > Game1.SCREENH)
            {
                return false;
            }
            return true;

        }

        public override void DoCollision(Screen s, Player p, Vector2 playerPos)
        {
        }
    }

    class Bookshelf : Entity
    {
        private bool readAlready = false;

        public Bookshelf(Vector2 location) :
            base(location)
        {
            armed = true;
            selectedAnim = 0;
            anims = new Animation[]
            {  
                new Animation(new IAsset[]
                {
                    IAsset.Bookshelf
                }, 5)
            };
            hitOffset = new Vector2(24, 71);
            activityString = "Read Book";
            collisionBox = new Rectangle();
        }

        public override bool Update(List<Entity> entities, Player p, Vector2 position)
        {
            if (armed)
            {
                if (Game1.rand.Next() % 20 == 0)
                {
                    entities.Add(new FlyingBook(location + new Vector2(Game1.rand.Next() % 40, location.Y)));
                }
            }
            return true;
        }

        public override void DoCollision(Screen parent, Player p, Vector2 playerPos)
        {
            if (!armed)
            {
                if (!readAlready)
                {
                    if (KVMA_Keyboard.SemiAuto(Keys.E))
                    {
                        activityString = "";
                        readAlready = true;
                        Game1.PushScreen(new DialogScreen(parent,
                            StringUtil.RandomGameFortune()));
                    }
                }
            }
        }
    }

    class Pillar : Entity
    {
        private Direction dir = Direction.N;

        public Pillar(Vector2 location) :
            base(location)
        {
            armed = true;
            selectedAnim = 4;
            anims = new Animation[]
            {
                new Animation(new IAsset[]
                {
                    IAsset.Column_Unarmed
                }, 5),
                new Animation(new IAsset[]
                {
                    IAsset.Column_East
                }, 5),
                new Animation(new IAsset[]
                {
                    IAsset.Column_South
                }, 5),
                new Animation(new IAsset[]
                {
                    IAsset.Column_West
                }, 5),
                new Animation(new IAsset[]
                {
                    IAsset.Column_Unarmed
                }, 5)
            };
            hitOffset = new Vector2(15, 78);
            activityString = "Examine";
            collisionBox = new Rectangle(0, 75, 30, 5);
        }

        public override bool Update(List<Entity> entities, Player p, Vector2 playerPos)
        {
            if (armed)
            {
                if (Game1.rand.Next() % 40 == 0)
                {
                    dir = (Direction)(Game1.rand.Next() % 4);
                    selectedAnim = (int)dir;
                    entities.Add(new LaserBeam(dir, Util.AddOffset(Util.AddOffset(location, hitOffset), new Vector2(0, -58))));
                    AssMan.Get(SAsset.Laser).Play();
                }
            }
            return true;
        }

        public override void DoCollision(Screen s, Player p, Vector2 playerPos)
        {
            
        }
    }


    class LaserBeam : Entity
    {
        int liveTicks = 0;
        Direction dir;
        public LaserBeam(Direction dir, Vector2 location) :
            base(location)
        {
            this.dir = dir;
            selectedAnim = (int)dir;
            anims = new Animation[]
            {
                new Animation(new IAsset[]
                {
                    IAsset.Laser_North
                }, 5),
                new Animation(new IAsset[]
                {
                    IAsset.Laser_East
                }, 5),
                new Animation(new IAsset[]
                {
                    IAsset.Laser_South
                }, 5),
                new Animation(new IAsset[]
                {
                    IAsset.Laser_West
                }, 5),
            };

            switch (dir)
            {
                case Direction.N:
                    location.Y -= AssMan.Get(IAsset.Laser_North).Height;
                    collisionBox = new Rectangle(0, 0, 9, 80); break;
                case Direction.E:
                    location.X -= AssMan.Get(IAsset.Laser_West).Width; 
                    collisionBox = new Rectangle(0, 0, 80, 9); break;
                case Direction.S:
                    location.Y += AssMan.Get(IAsset.Laser_North).Height;
                    collisionBox = new Rectangle(0, 0, 9, 80); break;
                default:
                    location.X += AssMan.Get(IAsset.Laser_West).Width;
                    collisionBox = new Rectangle(0, 0, 80, 9); break;
            }
        }

        public override bool Update(List<Entity> entities, Player p, Vector2 playerPos)
        {
            Rectangle scHitOff = Util.AddOffset(collisionBox, location);
            if (scHitOff.Intersects(Util.AddOffset(Util.AddOffset(p.GetRect(), playerPos), p.DrawOffset)))
            {
                p.Kill();
            }
            location = Util.MoveDirection(location, dir, 60.0f);
            liveTicks++;
            if (liveTicks > 15)
            {
                return false;
            }
            return true;
        }

        public override void DoCollision(Screen s, Player p, Vector2 playerPos)
        {
            
        }
    }

    class Confuscious : Entity
    {
        public Confuscious(Vector2 location) :
            base(location)
        {
            activityString = "Speak";
            collisionBox = new Rectangle();
            hitOffset = new Vector2();
            selectedAnim = 0;
            anims = new Animation[]
            {
                new Animation(new IAsset[]
                {
                    IAsset.Portrait1
                }, 50)
            };
        }

        public override void DoCollision(Screen s, Player p, Vector2 playerPos)
        {
            if (KVMA_Keyboard.SemiAuto(Keys.E))
            {
                Game1.PushScreen(new DialogScreen(s, StringUtil.RandomFortune()));
            }
        }

        public override bool Update(List<Entity> entities, Player p, Vector2 playerPos)
        {
            return false;
        }
    }

    class Barrel : Entity
    {
        Item held;
        public Barrel(Vector2 location) :
            base(location)
        {
            activityString = "Take Item";
            collisionBox = new Rectangle(3, 42, 25, 10);
            hitOffset = new Vector2(16, 51);
            selectedAnim = 0;
            held = (Item)(Game1.rand.Next() % (int)Item.Matchbox);
            anims = new Animation[]
            {
                new Animation(new IAsset[]
                {
                    IAsset.Barrel
                }, 5)
            };
        }

        public override void DoCollision(Screen s, Player p, Vector2 playerPos)
        {
            if (KVMA_Keyboard.SemiAuto(Keys.E))
            {
                Item putter = p.PickUpItem(held);
                held = putter;
            }
        }

        public override bool Update(List<Entity> entities, Player p, Vector2 playerPos)
        {
            return true;
        }
    }
}