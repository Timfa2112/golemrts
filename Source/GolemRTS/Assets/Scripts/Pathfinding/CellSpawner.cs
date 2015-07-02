using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimGame.Engine
{
    public static class CellSpawner
    {
        public static List<RoomData> rooms = new List<RoomData>();

        public static RoomData DefineCellFromWorldPos(Vector2 worldPosition, RoomData.Type type)
        {
            Vector2 gridp = PathfindConstants.WorldToGrid(worldPosition);
            return DefineCell(gridp, type);
        }

        public static RoomData DefineCell(Vector2 gridPos, RoomData.Type type)
        {
            return DefineCell((int)gridPos.x, (int)gridPos.y, type);
        }

        public static RoomData DefineCell(int x, int y, RoomData.Type type)
        {
            if (GetRoomAtGridPosition(x, y, true) == null)
            {
                RoomData data = new RoomData(new Vector2(x, y), type);
                rooms.Add(data);
                return data;
            }
            else
            {
                GetRoomAtGridPosition(x, y, true).type = type;
                return GetRoomAtGridPosition(x, y, true);
            }
        }

        public static void ObliterateWorld()
        {
            rooms = new List<RoomData>();
        }

        public static RoomData GetRoomAtGridPosition(Vector2 pos)
        {
            return GetRoomAtGridPosition((int)pos.x, (int)pos.y);
        }

        public static RoomData GetRoomAtGridPosition(int x, int y, bool ignoreDefault = false)
        {
            foreach (RoomData room in rooms)
            {
                if (room.GridPos.x == x && room.GridPos.y == y)
                {
                    return room;
                }
            }

            if(PathfindConstants.defaultType == RoomData.Type.Passable && !ignoreDefault)
            {
                return DefineCell(x, y, RoomData.Type.Passable);
            }

            return null;
        }
    }

    public class RoomData
    {
        public enum Type
        {
            Passable,
            Impassable
        };

        public RoomData(Vector2 gridPos, Type type)
        {
            GridPos = gridPos;
            this.type = type;
        }

        bool GetNeighborPossible(Vector2 direction)
        {
            RoomData otherRoom = CellSpawner.GetRoomAtGridPosition(GridPos + direction);
            return otherRoom != null && otherRoom.type == RoomData.Type.Passable;
        }

        public bool NPossible
        {
            get
            {
                return GetNeighborPossible(new Vector2(0, 1));
            }
        }
        public bool EPossible
        {
            get
            {
                return GetNeighborPossible(new Vector2(1, 0));
            }
        }
        public bool SPossible
        {
            get
            {
                return GetNeighborPossible(new Vector2(0, -1));
            }
        }
        public bool WPossible
        {
            get
            {
                return GetNeighborPossible(new Vector2(-1, 0));
            }
        }

        public bool NEPossible
        {
            get
            {
                return GetNeighborPossible(new Vector2(1, 1));
            }
        }
        public bool NWPossible
        {
            get
            {
                return GetNeighborPossible(new Vector2(-1, 1));
            }
        }
        public bool SEPossible
        {
            get
            {
                return GetNeighborPossible(new Vector2(1, -1));
            }
        }
        public bool SWPossible
        {
            get
            {
                return GetNeighborPossible(new Vector2(-1, -1));
            }
        }

        public Type type;
        public Vector2 GridPos;
    }
}