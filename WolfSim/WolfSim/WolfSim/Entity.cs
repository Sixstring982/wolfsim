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
                p.Kill("Flying Book");
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
        private bool alive = true;

        public Bookshelf(Vector2 location) :
            base(location)
        {
            armed = false;
            selectedAnim = 0;
            anims = new Animation[]
            {  
                new Animation(new IAsset[]
                {
                    IAsset.Bookshelf
                }, 5),
                new Animation(new IAsset[]
                {
                    IAsset.Bookshelf_Burned
                }, 5)
            };
            hitOffset = new Vector2(24, 71);
            activityString = "Read Book";
            collisionBox = new Rectangle();
        }

        public override bool Update(List<Entity> entities, Player p, Vector2 position)
        {
            if (armed && alive)
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
            if (alive)
            {
                if (p.GetItem() == Item.Matchbox)
                {
                    activityString = "Torch";
                    if (KVMA_Keyboard.ActionKey())
                    {
                        alive = false;
                        selectedAnim = 1;
                        activityString = "";
                    }
                }
                else if (!readAlready && !armed)
                {
                    if (KVMA_Keyboard.ActionKey())
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
        private bool canFire = true;
        public Pillar(Vector2 location) :
            base(location)
        {
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
            activityString = "";
            collisionBox = new Rectangle(0, 75, 30, 5);
        }

        public override bool Update(List<Entity> entities, Player p, Vector2 playerPos)
        {
            if (armed && canFire)
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
            if (canFire)
            {
                if (p.GetItem() == Item.Mop)
                {
                    activityString = "Poke";
                }
                if (KVMA_Keyboard.ActionKey())
                {
                    canFire = false;
                    activityString = "";
                }
            }
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
                p.Kill("Laser");
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
            collisionBox = new Rectangle(5, 34, 50, 26);
            hitOffset = new Vector2(30, 48);
            selectedAnim = 0;
            anims = new Animation[]
            {
                new Animation(new IAsset[]
                {
                    IAsset.Confuscious
                }, 50)
            };
        }

        public override void DoCollision(Screen s, Player p, Vector2 playerPos)
        {
            if (KVMA_Keyboard.ActionKey())
            {
                Game1.PushScreen(new DialogScreen(s, StringUtil.RandomFortune()));
            }
        }

        public override bool Update(List<Entity> entities, Player p, Vector2 playerPos)
        {
            return true;
        }
    }

    class Barrel : Entity
    {
        Item held;
        public Barrel(Vector2 location) :
            base(location)
        {
            collisionBox = new Rectangle(0, 42, 32, 10);
            hitOffset = new Vector2(16, 51);
            selectedAnim = 0;
            held = (Item)(Game1.rand.Next() % (int)(Item.Matchbox + 1));
            anims = new Animation[]
            {
                new Animation(new IAsset[]
                {
                    IAsset.Barrel
                }, 5)
            };
            if (held != Item.None)
            {
                activityString = "Take " + held.ToString();
            }
        }

        public override void DoCollision(Screen s, Player p, Vector2 playerPos)
        {
            if (KVMA_Keyboard.ActionKey())
            {
                Item putter = p.PickUpItem(held);
                held = putter;
                if (held != Item.None)
                {
                    activityString = "Take " + held.ToString();
                }
                else
                {
                    activityString = "";
                }
            }
        }

        public override bool Update(List<Entity> entities, Player p, Vector2 playerPos)
        {
            return true;
        }
    }

    class Fireball : Entity
    {
        Vector2 direction = new Vector2(0, 10);
        public Fireball(Vector2 location) :
            base(location)
        {
            collisionBox = new Rectangle(0, 0, 24, 45);
            hitOffset = new Vector2(12, 20);
            selectedAnim = 0;
            anims = new Animation[]
            {
                new Animation(new IAsset[]
                {
                    IAsset.Fireball
                }, 3)
            };
            direction = Util.Rotate(direction, Math.PI - (Math.PI / 4) + ((Game1.rand.NextDouble() * Math.PI) / 4));
        }

        public override void DoCollision(Screen s, Player p, Vector2 playerPos)
        {

        }

        public override bool Update(List<Entity> entities, Player p, Vector2 playerPos)
        {
            Rectangle scHitOff = Util.AddOffset(collisionBox, location);
            if (scHitOff.Intersects(Util.AddOffset(Util.AddOffset(p.GetRect(), playerPos), p.DrawOffset)))
            {
                p.Kill("Fireball");
            }
            location = Util.AddOffset(direction, location);
            if (location.X > Game1.SCREENW || location.Y > Game1.SCREENH ||
                location.X < 0 || location.Y < 0)
            {
                return false;
            }
            return true;
        }
    }

    class Desk : Entity
    {
        private bool readAlready = false;
        public Desk(Vector2 location) :
            base(location)
        {
            collisionBox = new Rectangle(2, 26, 181, 57);
            hitOffset = new Vector2(93, 28);
            selectedAnim = 0;
            anims = new Animation[]
            {
                new Animation(new IAsset[]
                {
                    IAsset.Desk
                }, 5)
            };
            activityString = "Read Page";
        }

        public override void DoCollision(Screen s, Player p, Vector2 playerPos)
        {
            if (!readAlready)
            {
                if (KVMA_Keyboard.ActionKey())
                {
                    readAlready = true;
                    activityString = "";
                    Game1.PushScreen(new DialogScreen(s, StringUtil.NextLore()));
                }
            }
        }

        public override bool Update(List<Entity> entities, Player p, Vector2 playerPos)
        {
            return true;
        }
    }

    class Toilet : Entity
    {
        bool canFire = true;
        Item held = Item.None;
        public Toilet(Vector2 location) :
            base(location)
        {
            collisionBox = new Rectangle(1, 7, 31, 40);
            hitOffset = new Vector2(10, 32);
            selectedAnim = 0;
            anims = new Animation[]
            {
                new Animation(new IAsset[]
                {
                    IAsset.Toilet
                }, 3),
                new Animation(new IAsset[]
                {
                    IAsset.Toilet_Clogged
                }, 3)
            };
        }

        public override void DoCollision(Screen s, Player p, Vector2 playerPos)
        {
            if (p.GetItem() == Item.Plunger)
            {
                activityString = "Clog";
                if (KVMA_Keyboard.ActionKey())
                {
                    canFire = false;
                    p.PickUpItem(held);
                    selectedAnim = 1;
                }
            }
        }

        public override bool Update(List<Entity> entities, Player p, Vector2 playerPos)
        {
            if (canFire && armed)
            {
                if (Game1.rand.Next() % 30 == 0)
                {
                    entities.Add(new Fireball(location));
                }
            }
            return true;
        }
    }


    class Couch : Entity
    {
        public Couch(Vector2 location) :
            base(location)
        {
            collisionBox = new Rectangle(3, 31, 279, 40);
            hitOffset = new Vector2(136, 65);
            selectedAnim = 0;
            anims = new Animation[]
            {
                new Animation(new IAsset[]
                {
                    IAsset.Couch
                }, 3),
            };
        }

        public override void DoCollision(Screen s, Player p, Vector2 playerPos)
        {
            
        }

        public override bool Update(List<Entity> entities, Player p, Vector2 playerPos)
        {
            return true;
        }
    }

    class Portrait : Entity
    {
        public Portrait(Vector2 location) :
            base(location)
        {
            collisionBox = new Rectangle();
            hitOffset = new Vector2(31, 50);
            selectedAnim = 0;
            anims = new Animation[]
            {
                new Animation(new IAsset[]
                {
                    IAsset.Portrait1
                }, 3),
            };
        }

        public override void DoCollision(Screen s, Player p, Vector2 playerPos)
        {

        }

        public override bool Update(List<Entity> entities, Player p, Vector2 playerPos)
        {
            return true;
        }
    }


    class Father : Entity
    {
        private Vector2 initLoc;
        public Father(Vector2 location) :
            base(location)
        {
            initLoc = location;
            collisionBox = new Rectangle(0, 50, 27, 14);
            hitOffset = new Vector2(13, 57);
            selectedAnim = 0;
            anims = new Animation[]
            {
                new Animation(new IAsset[]
                {
                    IAsset.Father
                }, 3),
            };
        }
        public override void DoCollision(Screen s, Player p, Vector2 playerPos)
        {
            if (!armed)
            {
                activityString = "Speak";
                if (KVMA_Keyboard.ActionKey())
                {
                    Game1.PushScreen(new DialogScreen(s, StringUtil.RandomFather()));
                }
            }
        }

        public override bool Update(List<Entity> entities, Player p, Vector2 playerPos)
        {
            if (armed)
            {
                location = new Vector2(Game1.SCREENW << 1, Game1.SCREENH << 1);
            }
            else
            {
                location = new Vector2(initLoc.X, initLoc.Y);
            }
            return true;
        }
    }

    class Mother : Entity
    {
        public Mother(Vector2 location) :
            base(location)
        {
            collisionBox = new Rectangle(0, 50, 27, 14);
            hitOffset = new Vector2(13, 57);
            selectedAnim = 0;
            anims = new Animation[]
            {
                new Animation(new IAsset[]
                {
                    IAsset.Mother
                }, 3),
            };
        }
        public override void DoCollision(Screen s, Player p, Vector2 playerPos)
        {
            if (!armed)
            {
                activityString = "Speak";
                if (KVMA_Keyboard.ActionKey())
                {
                    Game1.PushScreen(new DialogScreen(s, StringUtil.RandomMother()));
                }
            }
        }

        public override bool Update(List<Entity> entities, Player p, Vector2 playerPos)
        {
            return true;
        }
    }

    class PoolPanel : Entity
    {
        public PoolPanel(Vector2 location) :
            base(location)
        {
            collisionBox = new Rectangle();
            hitOffset = new Vector2(23, 57);
            selectedAnim = 0;
            anims = new Animation[]
            {
                new Animation(new IAsset[]
                {
                    IAsset.PoolPanel
                }, 3)
            };
        }

        public override void DoCollision(Screen s, Player p, Vector2 playerPos)
        {
            
        }

        public override bool Update(List<Entity> entities, Player p, Vector2 playerPos)
        {
            return true;
        }
    }

    class PoolBall : Entity
    {
        private Vector2 direction = new Vector2(0, 8);
        public PoolBall(Vector2 location) :
            base(location)
        {
            collisionBox = new Rectangle(0, 0, 10, 10);
            hitOffset = new Vector2(5, 5);
            selectedAnim = Game1.rand.Next() % 7;
            direction = Util.Rotate(direction, Game1.rand.NextDouble() * Math.PI * 2);
            anims = new Animation[]
            {
                new Animation(new IAsset[]
                {
                    IAsset.BlackBall
                }, 3),
                new Animation(new IAsset[]
                {
                    IAsset.BlueBall
                }, 3),
                new Animation(new IAsset[]
                {
                    IAsset.BlueBallStripe
                }, 3),
                new Animation(new IAsset[]
                {
                    IAsset.RedBall
                }, 3),
                new Animation(new IAsset[]
                {
                    IAsset.RedBallStripe
                }, 3),
                new Animation(new IAsset[]
                {
                    IAsset.YellowBall
                }, 3),
                new Animation(new IAsset[]
                {
                    IAsset.YellowBallStripe
                }, 3),
            };
        }
        public override void DoCollision(Screen s, Player p, Vector2 playerPos)
        {
            
        }

        public override bool Update(List<Entity> entities, Player p, Vector2 playerPos)
        {
            Rectangle scHitOff = Util.AddOffset(collisionBox, location);
            if (scHitOff.Intersects(Util.AddOffset(Util.AddOffset(p.GetRect(), playerPos), p.DrawOffset)))
            {
                p.Kill("Pool Ball");
            }
            location = Util.AddOffset(direction, location);
            if (location.X > Game1.SCREENW || location.Y > Game1.SCREENH ||
                location.X < 0 || location.Y < 0)
            {
                return false;
            }
            return true;
        }
    }

    class Chair : Entity
    {
        public Chair(Vector2 location) :
            base(location)
        {
            collisionBox = new Rectangle(4, 41, 20, 10);
            hitOffset = new Vector2(32, 46);
            selectedAnim = 0;
            anims = new Animation[]
            {
                new Animation(new IAsset[]
                {
                    IAsset.Chair
                }, 3)
            };
        }

        public override void DoCollision(Screen s, Player p, Vector2 playerPos)
        {
            
        }

        public override bool Update(List<Entity> entities, Player p, Vector2 playerPos)
        {
            return true;
        }
    }

    class Plant : Entity
    {
        public Plant(Vector2 location) :
            base(location)
        {
            collisionBox = new Rectangle(6, 38, 16, 6);
            hitOffset = new Vector2(12, 51);
            selectedAnim = 0;
            anims = new Animation[]
            {
                new Animation(new IAsset[]
                {
                    IAsset.Plant
                }, 3)
            };
        }

        public override void DoCollision(Screen s, Player p, Vector2 playerPos)
        {
            
        }

        public override bool Update(List<Entity> entities, Player p, Vector2 playerPos)
        {
            return true;
        }
    }

    class PoolTable : Entity
    {
        private bool canFire = true;
        public PoolTable(Vector2 location) :
            base(location)
        {
            collisionBox = new Rectangle(0, 35, 186, 52);
            hitOffset = new Vector2(89, 36);
            selectedAnim = 0;
            anims = new Animation[]
            {
                new Animation(new IAsset[]
                {
                    IAsset.PoolTable
                }, 3),
                new Animation(new IAsset[]
                {
                    IAsset.DefenestratedPoolTable
                }, 3)
            };
        }

        public override void DoCollision(Screen s, Player p, Vector2 playerPos)
        {
            if (p.GetItem() == Item.Goat)
            {
                activityString = "Defenestrate";
                if (KVMA_Keyboard.ActionKey())
                {
                    AssMan.Get(SAsset.Crunch).Play();
                    canFire = false;
                    selectedAnim = 1;
                }
            }
        }

        public override bool Update(List<Entity> entities, Player p, Vector2 playerPos)
        {
            if (armed && canFire)
            {
                if (Game1.rand.Next() % 10 == 0)
                {
                    entities.Add(new PoolBall(Util.AddOffset(location, hitOffset)));
                }
            }
            return true;
        }
    }

    class Table : Entity
    {
        public Table(Vector2 location) :
            base(location)
        {
            collisionBox = new Rectangle(9, 33, 80, 66);
            hitOffset = new Vector2(49, 46);
            selectedAnim = 0;
            anims = new Animation[]
            {
                new Animation(new IAsset[]
                {
                    IAsset.TableSuck
                }, 10)
            };
        }

        public override void DoCollision(Screen s, Player p, Vector2 playerPos)
        {
            
        }

        public override bool Update(List<Entity> entities, Player p, Vector2 playerPos)
        {
            return true;
        }
    }

    class Fountain : Entity
    {
        public Fountain(Vector2 location) :
            base(location)
        {
            collisionBox = new Rectangle(9, 44, 44, 16);
            hitOffset = new Vector2(30, 53);
            selectedAnim = 0;
            anims = new Animation[]
            {
                new Animation(new IAsset[]
                {
                    IAsset.Fountain1,
                    IAsset.Fountain2,
                    IAsset.Fountain3
                }, 10)
            };
        }

        public override void DoCollision(Screen s, Player p, Vector2 playerPos)
        {
            
        }

        public override bool Update(List<Entity> entities, Player p, Vector2 playerPos)
        {
            return true;
        }
    }

    class EmptyBed : Entity
    {
        public EmptyBed(Vector2 location) :
            base(location)
        {
            collisionBox = new Rectangle(0, 72, 80, 14);
            hitOffset = new Vector2(39, 72);
            selectedAnim = 0; 
            anims = new Animation[]
            {
                new Animation(new IAsset[]
                {
                    IAsset.Bed
                }, 3)
            };
        }

        public override void DoCollision(Screen s, Player p, Vector2 playerPos)
        {
            
        }

        public override bool Update(List<Entity> entities, Player p, Vector2 playerPos)
        {
            return true;
        }
    }

    class Bed : Entity
    {
        private bool eaten = false;
        public Bed(Vector2 location) :
            base(location)
        {
            collisionBox = new Rectangle(0, 72, 80, 14);
            hitOffset = new Vector2(39, 72);
            selectedAnim = 0; 
            anims = new Animation[]
            {
                new Animation(new IAsset[]
                {
                    IAsset.Bed
                }, 3),
                new Animation(new IAsset[]
                {
                    IAsset.Bed_Sleep
                }, 3)
            };
        }

        public override void DoCollision(Screen s, Player p, Vector2 playerPos)
        {
            if (armed && !eaten)
            {
                activityString = "Eat";
                if (KVMA_Keyboard.ActionKey())
                {
                    eaten = true;
                    p.Eat();
                    selectedAnim = 0;
                }
            }
        }

        public override bool Update(List<Entity> entities, Player p, Vector2 playerPos)
        {
            if (armed && !eaten)
            {
                selectedAnim = 1;
            }
            else
            {
                selectedAnim = 0;
            }

            return true;
        }
    }
}