using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimGame.Engine
{
    public static class PathfindConstants
    {
        public static readonly float GridSize = 1f; //Change this to the width and height of the grid
        public static readonly RoomData.Type defaultType = RoomData.Type.Impassable;

        public enum Directions
        {
            North,
            East,
            South,
            West,
            NorthEast,
            NorthWest,
            SouthEast,
            SouthWest,
            None
        }

        public static Vector2 GridToWorld(int x, int y)
        {
			return RoundedVector((float)x * GridSize, (float)y * GridSize);
        }

        public static Vector2 WorldToGrid(int x, int y)
        {
			return RoundedVector((float)x / GridSize, (float)y / GridSize);
        }

        public static Vector2 GridToWorld(float x, float y)
        {
            return GridToWorld((int)Mathf.Round(x), (int)Mathf.Round(y));
        }

        public static Vector2 GridToWorld(Vector2 gridPosition)
        {
            return GridToWorld(gridPosition.x, gridPosition.y);
        }

        public static Vector2 WorldToGrid(float x, float y)
        {
            return WorldToGrid((int)Mathf.Round(x), (int)Mathf.Round(y));
        }

        public static Vector2 WorldToGrid(Vector2 worldPos)
        {
            return WorldToGrid((int)Mathf.Round(worldPos.x), (int)Mathf.Round(worldPos.y));
        }

        public static Vector2 RoundedVector(float x, float y)
        {
            return new Vector2((int)Mathf.Round(x), (int)Mathf.Round(y));
        }
    }
}