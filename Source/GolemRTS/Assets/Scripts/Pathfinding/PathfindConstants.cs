using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimGame.Engine
{
    public static class PathfindConstants
    {
        public static readonly float GridSize = 1; //Change this to the width and height of the grid
        public static readonly RoomData.Type defaultType = RoomData.Type.Passable;

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
            return RoundedVector(x * GridSize, y * GridSize);
        }

        public static Vector2 WorldToGrid(int x, int y)
        {
            return RoundedVector(x / GridSize, y / GridSize);
        }

        public static Vector2 GridToWorld(float x, float y)
        {
            return GridToWorld((int)Math.Round(x), (int)Math.Round(y));
        }

        public static Vector2 GridToWorld(Vector2 gridPosition)
        {
            return GridToWorld(gridPosition.x, gridPosition.y);
        }

        public static Vector2 WorldToGrid(float x, float y)
        {
            return WorldToGrid((int)Math.Round(x), (int)Math.Round(y));
        }

        public static Vector2 WorldToGrid(Vector2 worldPos)
        {
            return WorldToGrid((int)Math.Round(worldPos.x), (int)Math.Round(worldPos.y));
        }

        public static Vector2 RoundedVector(float x, float y)
        {
            return new Vector2((int)Math.Round(x), (int)Math.Round(y));
        }
    }
}