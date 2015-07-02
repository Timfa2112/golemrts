using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimGame.Engine
{
    public static class Pathfinder
    {
        public static int MaxTries = 1000;
        public static bool AllowDiagonalMovement = true;

        public static List<Vector2> FindPathToWorldPos(Vector2 start, Vector2 end)
        {
            Vector2 gridStart = PathfindConstants.WorldToGrid(start);
            Vector2 gridEnd = PathfindConstants.WorldToGrid(end);

            return FindPathToGridPoint(gridStart, gridEnd);
        }

        public static List<Vector2> FindPathToGridPoint(Vector2 gridStart, Vector2 gridEnd)
        {
            return FindPath(gridStart, gridEnd);
        }

        private static List<Vector2> FindPath(Vector2 origin, Vector2 target, PathfindConstants.Directions fromDir = PathfindConstants.Directions.None)
        {
            List<pathNode> nodes = new List<pathNode>();
            List<Vector2> checkedPositions = new List<Vector2>();
            nodes.Add(new pathNode((int)Math.Round(origin.x), (int)Math.Round(origin.y), new List<int>(), new List<int>(), fromDir));

            bool foundPath = false;
            List<Vector2> resultPath = null;
            int tries = 0;

            while (!foundPath && tries < MaxTries)
            {
                //Debug.Log("Tries: " + tries + "Nodes: " + nodes.Count);
                tries++;
                List<pathNode> newNodes = new List<pathNode>();

                foreach (pathNode node in nodes)
                {
                    if (!checkedPositions.Contains(new Vector2(node.GridPosX, node.GridPosY)))
                    {
                        checkedPositions.Add(new Vector2(node.GridPosX, node.GridPosY));

                        if (Vector2.Distance(new Vector2(node.GridPosX, node.GridPosY), target) < 1)
                        {
                            Vector2[] path = new Vector2[node.GridPathX.Count];

                            for (int i = 0; i < path.Length; i++)
                            {
                                path[i].x = node.GridPathX[i];
                                path[i].y = node.GridPathY[i];
                            }

                            resultPath = new List<Vector2>();
                            resultPath.AddRange(path);
                            foundPath = true;

                            break;
                        }

                        RoomData room = CellSpawner.GetRoomAtGridPosition(node.GridPosX, node.GridPosY);

                        if (room != null)
                        {
                            if (room.NPossible && node.FromDir != PathfindConstants.Directions.North)//N
                            {
                                newNodes.Add(new pathNode(node.GridPosX, node.GridPosY + 1, node.GridPathX, node.GridPathY, PathfindConstants.Directions.South));
                            }

                            if (room.EPossible && node.FromDir != PathfindConstants.Directions.East)//E
                            {
                                newNodes.Add(new pathNode(node.GridPosX + 1, node.GridPosY, node.GridPathX, node.GridPathY, PathfindConstants.Directions.West));
                            }

                            if (room.SPossible && node.FromDir != PathfindConstants.Directions.South)//S
                            {
                                newNodes.Add(new pathNode(node.GridPosX, node.GridPosY - 1, node.GridPathX, node.GridPathY, PathfindConstants.Directions.North));
                            }

                            if (room.WPossible && node.FromDir != PathfindConstants.Directions.West)//W
                            {
                                newNodes.Add(new pathNode(node.GridPosX - 1, node.GridPosY, node.GridPathX, node.GridPathY, PathfindConstants.Directions.East));
                            }

                            if (AllowDiagonalMovement)
                            {
                                if (room.NEPossible && node.FromDir != PathfindConstants.Directions.NorthEast)
                                {
                                    newNodes.Add(new pathNode(node.GridPosX + 1, node.GridPosY + 1, node.GridPathX, node.GridPathY, PathfindConstants.Directions.SouthWest));
                                }

                                if (room.NWPossible && node.FromDir != PathfindConstants.Directions.NorthWest)
                                {
                                    newNodes.Add(new pathNode(node.GridPosX - 1, node.GridPosY + 1, node.GridPathX, node.GridPathY, PathfindConstants.Directions.SouthEast));
                                }

                                if (room.SEPossible && node.FromDir != PathfindConstants.Directions.SouthEast)
                                {
                                    newNodes.Add(new pathNode(node.GridPosX + 1, node.GridPosY - 1, node.GridPathX, node.GridPathY, PathfindConstants.Directions.NorthWest));
                                }

                                if (room.SWPossible && node.FromDir != PathfindConstants.Directions.SouthWest)
                                {
                                    newNodes.Add(new pathNode(node.GridPosX - 1, node.GridPosY - 1, node.GridPathX, node.GridPathY, PathfindConstants.Directions.NorthEast));
                                }
                            }
                        }
                    }
                }
                nodes = new List<pathNode>(newNodes);

                //if(tries == MaxTries)
                //	Debug.LogError("Pathfind unsuccessful, " + tries);
            }

            return resultPath;
        }
    }

    public struct pathNode
    {
        public pathNode(int x, int y, List<int> xHist, List<int> yHist, PathfindConstants.Directions fromDir = PathfindConstants.Directions.None)
        {
            FromDir = fromDir;

            GridPosX = x;
            GridPosY = y;

            GridPathX = new List<int>(xHist);
            GridPathY = new List<int>(yHist);

            GridPathX.Add(x);
            GridPathY.Add(y);
        }

        public PathfindConstants.Directions FromDir;
        public int GridPosX;
        public int GridPosY;
        public List<int> GridPathX;
        public List<int> GridPathY;
    }
}