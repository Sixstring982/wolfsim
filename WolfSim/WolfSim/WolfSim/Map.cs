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

        public void ChangeRoom(Vector2 location)
        {
            currentRoom = currentRoom.NearestExit(location);
        }
    }
}
