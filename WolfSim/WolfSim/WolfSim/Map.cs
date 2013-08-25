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
            currentRoom = new Foyer();
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

        public void Update(Player p)
        {
            if (changeTicks == 0)
            {
                currentRoom.Update(p);
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
        }

        public void ChangeRoom(Vector2 location)
        {
            if (changeTicks == 0)
            {
                RoomExit re = currentRoom.NearestExit(location);
                changeDir = VecDir.OppositeDir(re.location.dir);
                changeRoom = re.next;
                changeTicks++;
            }
        }
    }
}