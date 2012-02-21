﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using WorldGeneration.Utils;

using Vaerydian.Components;

namespace Vaerydian.Utils
{
    public class Cell
    {
        public Vector2 Position;
        public Cell Parent;
        public int F = 0;
        public int G = 0;
        public int H = 0;
    }


    public class ASharpPathing
    {

        private GameMap a_GameMap;
        private Vector2 a_Start,a_Finish;
        private Cell a_StartCell, a_FinishCell;
        private List<Cell> a_OpenSet = new List<Cell>();

        public List<Cell> OpenSet
        {
            get { return a_OpenSet; }
            set { a_OpenSet = value; }
        }
        private List<Cell> a_ClosedSet = new List<Cell>();

        public List<Cell> ClosedSet
        {
            get { return a_ClosedSet; }
            set { a_ClosedSet = value; }
        }

        private List<Cell> a_BlockingSet = new List<Cell>();

        public List<Cell> BlockingSet
        {
            get { return a_BlockingSet; }
            set { a_BlockingSet = value; }
        }

        private int linCost = 10;
        private int diaCost = 14;
        private int a_MaxLoops = 60;

        private bool a_Failed = false;

        public bool Failed
        {
            get { return a_Failed; }
            set { a_Failed = value; }
        }

        private bool a_IsFound = false;
        /// <summary>
        /// did it complete?
        /// </summary>
        public bool IsFound
        {
            get { return a_IsFound; }
            set { a_IsFound = value; }
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="start">starting point</param>
        /// <param name="finish">ending point</param>
        /// <param name="map">map to path through</param>
        public ASharpPathing(Vector2 start, Vector2 finish, GameMap map) 
        {
            a_GameMap = map;
            a_Start = start;
            a_Finish = finish;
            a_StartCell = createCell((int)start.X, (int)start.Y);
            a_FinishCell = createCell((int)finish.X, (int)finish.Y);
            a_OpenSet.Add(a_StartCell);

            List<Cell> temp = findAdjacentCells(a_StartCell);
            for (int i = 0; i < temp.Count; i++)
            {
                a_OpenSet.Add(temp[i]);
            }

            a_OpenSet.Remove(a_StartCell);
            a_ClosedSet.Add(a_StartCell);
        }

        /// <summary>
        /// main pathing loop
        /// </summary>
        public void findPath()
        {
            int loopCount = 0;
            List<Cell> workingList = new List<Cell>();
            Cell temp;

            while (loopCount < a_MaxLoops)
            {
                //have we failed to find it?
                if (a_OpenSet.Count == 0)
                {
                    if (contains(a_FinishCell, a_ClosedSet) < 0)
                    {
                        a_Failed = true;
                        return;
                    }
                }

                //find the current loswest cost square
                temp = findLeastCostCell(a_OpenSet);
                a_OpenSet.Remove(temp);
                a_ClosedSet.Add(temp);

                int fin = contains(a_FinishCell, a_ClosedSet);

                //did we find it?
                if (contains(a_FinishCell,a_ClosedSet) >= 0)
                {
                    a_IsFound = true;
                    temp = a_ClosedSet[fin];
                    temp.Parent = a_ClosedSet[fin - 1];
                    a_ClosedSet[fin] = temp;
                    return;
                }
                
                //get adjacent squares
                workingList = findAdjacentCells(temp);

                for (int i = 0; i < workingList.Count; i++)
                {
                    a_OpenSet.Add(workingList[i]);
                }



                loopCount++;
            }
        }

        /// <summary>
        /// return the current path
        /// </summary>
        /// <returns>the current path</returns>
        public List<Cell> getPath()
        {
            int loopCount = 0;

            List<Cell> path = new List<Cell>();

            Cell next = a_ClosedSet[contains(a_FinishCell,a_ClosedSet)];
            path.Add(next);

            while (loopCount < a_ClosedSet.Count)
            {
                next = next.Parent;
                
                if (next.Position == a_StartCell.Position)
                {
                    break;
                }
                path.Add(next);
            }

            List<Cell> finalPath = new List<Cell>();

            for (int i = 0; i < path.Count; i++)
            {
                finalPath.Add(path[path.Count - 1 - i]);
            }
            
            return finalPath;
        }

        /// <summary>
        /// finds the least cost cell in the list
        /// </summary>
        /// <param name="list">list to search</param>
        /// <returns>least cost cell</returns>
        private Cell findLeastCostCell(List<Cell> list)
        {
            Cell minCell = new Cell();
            int min = int.MaxValue;

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].F < min)
                {
                    minCell = list[i];
                    min = minCell.F;
                }
            }
            return minCell;
        }

        /// <summary>
        /// determines F cost for a cell
        /// </summary>
        /// <param name="cell">cell to determine</param>
        /// <returns>the F cost</returns>
        private int findCost(Cell cell)
        {
            cell.F = cell.G + heuristicCost(cell);

            return cell.F;
        }

        /// <summary>
        /// determines the Heuristic Cost for the cell
        /// </summary>
        /// <param name="cell">cell to determine</param>
        /// <returns>the Heuristic cost</returns>
        private int heuristicCost(Cell cell)
        {
            int x, y;

            x = (int) Math.Abs(a_Finish.X - cell.Position.X);
            y = (int) Math.Abs(a_Finish.Y - cell.Position.Y);

            cell.H = (x + y) * 10;

            return cell.H;
        }

        /// <summary>
        /// finds the adjacent cells
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        private List<Cell> findAdjacentCells(Cell cell)
        {
            //setup temps
            Terrain terrain;
            List<Cell> goodAdjacents = new List<Cell>();

            //search ajacent tiles
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    //skip this cell since we already have it
                    if ((i == 0) && (j == 0))
                        continue;

                    //get the terrain
                    terrain = a_GameMap.getTerrain((int) cell.Position.X + i, (int)cell.Position.Y + j);//);
                    //is terrain valid?
                    if (terrain == null)
                        continue;

                    //is it blocking?
                    if (terrain.IsBlocking)
                    {
                        a_BlockingSet.Add(createCell((int)cell.Position.X +i, (int)cell.Position.Y + j));
                        continue;
                    }

                    //create the cell
                    Cell newCell = createCell((int)cell.Position.X + i, (int)cell.Position.Y + j);

                    //if we've already got it, ignore it
                    if (contains(newCell, a_ClosedSet) >= 0)
                        continue;

                    //set its parent
                    newCell.Parent = cell;

                    //set its G cost since we can do that here
                    
                    if ((i == -1 && j == -1) || (i == -1 && j == 1) || (i == 1 && j == -1) || (i == 1 && j == 1))
                    {
                        //since diagonal cell, do tests to see if its a valid move (not cutting a corner)
                        
                        //check UL cell
                        if ((i == -1 && j == -1))
                        {
                            if (cellIsBlocking(new Vector2((int)cell.Position.X, (int)cell.Position.Y - 1)) &&
                                cellIsBlocking(new Vector2((int)newCell.Position.X + 1, (int)newCell.Position.Y)))
                                continue;
                            if (cellIsBlocking(new Vector2((int)cell.Position.X-1, (int)cell.Position.Y)) &&
                                cellIsBlocking(new Vector2((int)newCell.Position.X, (int)newCell.Position.Y+1)))
                                continue;
                        }

                        //check LL cell
                        if ((i == -1 && j == 1))
                        {
                            if (cellIsBlocking(new Vector2((int)cell.Position.X-1, (int)cell.Position.Y)) &&
                                cellIsBlocking(new Vector2((int)newCell.Position.X, (int)newCell.Position.Y-1)))
                                continue;
                            if (cellIsBlocking(new Vector2((int)cell.Position.X, (int)cell.Position.Y+1)) &&
                                cellIsBlocking(new Vector2((int)newCell.Position.X+1, (int)newCell.Position.Y)))
                                continue;
                        }

                        //check LR cell
                        if ((i == 1) && (j == 1))
                        {
                            if (cellIsBlocking(new Vector2((int)cell.Position.X, (int)cell.Position.Y+1)) &&
                                cellIsBlocking(new Vector2((int)newCell.Position.X-1, (int)newCell.Position.Y)))
                                continue;
                            if (cellIsBlocking(new Vector2((int)cell.Position.X+1, (int)cell.Position.Y)) &&
                                cellIsBlocking(new Vector2((int)newCell.Position.X, (int)newCell.Position.Y-1)))
                                continue;
                        }

                        //check UR cell
                        if ((i == 1) && (j == -1))
                        {
                            if (cellIsBlocking(new Vector2((int)cell.Position.X + 1, (int)cell.Position.Y)) &&
                                cellIsBlocking(new Vector2((int)newCell.Position.X, (int)newCell.Position.Y + 1)))
                                continue;
                            if (cellIsBlocking(new Vector2((int)cell.Position.X, (int)cell.Position.Y - 1)) &&
                                cellIsBlocking(new Vector2((int)newCell.Position.X - 1, (int)newCell.Position.Y)))
                                continue;
                        }
                        
                        //valid move, so get G cost                        
                        newCell.G = diaCost + cell.G;
                    }
                    else
                    {
                        newCell.G = linCost + cell.G;
                    }

                    //newCell.G = linCost + cell.G;

                    //finds this cell's cost
                    newCell.F = findCost(newCell);

                    //check to see if we already have this one on an open list
                    int pos = contains(newCell, a_OpenSet);
                    if (pos >= 0)
                    {
                        Cell temp = a_OpenSet[pos];
                        
                        //is cell a better path?
                        if (cell.G + (temp.G - temp.Parent.G) < temp.G)
                        {
                            temp.G -= temp.Parent.G;

                            //yes, so update it, re-calc cost, and continue;
                            temp.Parent = cell;

                            temp.G += temp.Parent.G;

                            temp.F = findCost(temp);

                            a_OpenSet[pos] = temp;
                            continue;
                        }
                        else
                            continue;
                    }

                    //add its location to the list as a good candidate
                    goodAdjacents.Add(newCell);
                }
            }
            //return list
            return goodAdjacents;
        }

        private Cell createCell(int x, int y)
        {
            Cell cell = new Cell();
            cell.Position.X = x;
            cell.Position.Y = y;
            return cell;
        }

        private int contains(Cell cell, List<Cell> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (cell.Position == list[i].Position)
                    return i;
            }
            
            return -1;
        }

        private Cell getCell(Vector2 position, List<Cell> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (position == list[i].Position)
                    return list[i];
            }
            return null;
        }

        private void removeCell(Vector2 position, List<Cell> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (position == list[i].Position)
                    list.RemoveAt(i);
            }
        }

        private bool cellIsBlocking(Vector2 position)
        {
            Terrain terrain;
            terrain = a_GameMap.getTerrain((int)position.X, (int)position.Y);
            //is terrain valid?
            if (terrain == null)
                return true;
            //is it blocking?
            if (terrain.IsBlocking)
                return true;
            return false;
        }

        /// <summary>
        /// reset
        /// </summary>
        /// <param name="start">starting point</param>
        /// <param name="finish">ending point</param>
        /// <param name="map">map to path through</param>
        public void reset(Vector2 start, Vector2 finish, GameMap map)
        {
            reset();
            
            a_GameMap = map;
            a_Start = start;
            a_Finish = finish;
            a_StartCell = createCell((int)start.X, (int)start.Y);
            a_FinishCell = createCell((int)finish.X, (int)finish.Y);

            a_OpenSet.Add(a_StartCell);

            List<Cell> temp = findAdjacentCells(a_StartCell);
            for (int i = 0; i < temp.Count; i++)
            {
                a_OpenSet.Add(temp[i]);
            }

            a_OpenSet.Remove(a_StartCell);
            a_ClosedSet.Add(a_StartCell);
        }

        /// <summary>
        /// resets all lists and flags
        /// </summary>
        private void reset()
        {
            a_OpenSet.Clear();
            a_ClosedSet.Clear();
            a_BlockingSet.Clear();
            

            a_Failed = false;
            a_IsFound = false;
        }

    }
}