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
        }

        public Map()
        {
            currentRoom = new Foyer();
            currentRoom.AddRoom(new Parlor());
            currentRoom.AddRoom(new Parlor());
            currentRoom.AddRoom(new DiningRoom());
        }

        public void ChangeRoom(Vector2 location)
        {
            currentRoom = currentRoom.NearestExit(location);
        }
    }
}
