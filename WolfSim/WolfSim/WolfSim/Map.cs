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
    class Map
    {
        private List<Room> linkerRooms = new List<Room>();
        private List<Room> endRooms = new List<Room>();

        private Foyer rootRoom;
        private Room currentRoom;

        public Room GetCurrentRoom()
        {
            return currentRoom;
        }

        private const int maxChangeTicks = 100;
        private int changeTicks = 0;
        private Vector2 changeVec = Vector2.Zero;
        private Direction changeDir = Direction.N;
        private Room changeRoom = null;
        private int ChangeAnimSpeed = 25;

        private string RoomStatusString = "";
        private int statusStringLife = 0;

        private void RenderStatusString(SpriteBatch sb)
        {
            if (statusStringLife > 0)
            {
                statusStringLife += 2;
                sb.DrawString(AssMan.victorianFont, RoomStatusString, new Vector2(0, 0), Color.White);
                if (statusStringLife < 255)
                {
                    sb.Draw(AssMan.Get(IAsset.Mask), new Rectangle(0, 0, 200, 100), new Color(0, 0, 0, 255 - statusStringLife));
                }
                else
                {
                    sb.Draw(AssMan.Get(IAsset.Mask), new Rectangle(0, 0, 200, 100), new Color(0, 0, 0, 255 - (512 - statusStringLife)));
                }
                if (statusStringLife >= 512)
                {
                    statusStringLife = 0;
                }
            }
        }

        private void StartStatusRender(string s)
        {
            RoomStatusString = s;
            statusStringLife = 1;
        }

        private void FillRoomLists()
        {
            linkerRooms.Clear();
            endRooms.Clear();

            linkerRooms.Add(new Parlor());
            linkerRooms.Add(new DiningRoom());
            linkerRooms.Add(new Bedroom());
            linkerRooms.Add(new Breakfast());
            linkerRooms.Add(new BilliardRoom());
            linkerRooms.Add(new Kitchen());
            linkerRooms.Add(new SittingRoom());

            endRooms.Add(new Shed());
            endRooms.Add(new Lab());
            endRooms.Add(new Study());
            endRooms.Add(new Backyard());
        }

        public Map()
        {
            GenerateMap();
        }

        private bool AddToMap(Room current, Room r)
        {
            if (!current.AddRoom(r))
            {
                Room nextL = current.NextLinker();
                if (nextL != null)
                {
                    return AddToMap(nextL, r);
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        private void GenerateMap()
        {
            FillRoomLists();
            currentRoom = rootRoom = new Foyer();
            int[] linkerIo = Util.ShuffledIota(linkerRooms.Count);
            for (int i = 0; i < linkerIo.Length; i++)
            {
                while (!AddToMap(currentRoom, linkerRooms[linkerIo[i]])) ;
            }

            linkerIo = Util.ShuffledIota(endRooms.Count);
            for (int i = 0; i < linkerIo.Length; i++)
            {
                AddToMap(currentRoom, endRooms[linkerIo[i]]);
            }
        }

        public void Update(Screen masterScreen, Player p)
        {
            if (changeTicks == 0)
            {
                currentRoom.Update(masterScreen, p);
                if (currentRoom.IsPlayerOnExit())
                {
                    ChangeRoom(currentRoom.GetPlayerPosition());
                }
            }
        }

        public void Render(SpriteBatch sb, Player p)
        {
            if (changeTicks > 0)
            {
                changeTicks++;

                if (changeTicks == maxChangeTicks >> 1)
                {
                    changeRoom.SetPlayerFromRoom(currentRoom);
                    currentRoom = changeRoom;
                    if (changeDir == Direction.N || changeDir == Direction.S)
                    {
                        changeVec.Y = -changeVec.Y;
                    }
                    else
                    {
                        changeVec.X = -changeVec.X;
                    }
                }

                changeVec = Util.MoveDirection(changeVec, changeDir, ChangeAnimSpeed);
                if (changeVec == Vector2.Zero)
                {
                    changeTicks = 0;
                    changeRoom = null;
                }

                currentRoom.Render(sb, changeVec, p);
            }
            else
            {
                currentRoom.Render(sb, Vector2.Zero, p);
            }
            RenderStatusString(sb);
        }

        public void ChangeRoom(Vector2 location)
        {
            if (changeTicks == 0)
            {
                RoomExit re = currentRoom.NearestExit(location);
                changeDir = VecDir.OppositeDir(re.location.dir);
                changeRoom = re.next;
                changeTicks++;
                StartStatusRender(re.next.name);
            }
        }
    }
}