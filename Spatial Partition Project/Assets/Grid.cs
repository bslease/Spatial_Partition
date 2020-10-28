using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpatialPartitionPattern
{
    public class Grid
    {
        // Need this to convert from world coordinate position to cell position
        int cellSize;

        // This is the actual grid, where a soldier is in each cell
        // Each individual soldier links to other soldiers in the same cell
        Soldier[,] cells;

        // Initialize the grid
        public Grid(int mapWidth, int cellSize)
        {
            this.cellSize = cellSize;
            int numberOfCells = mapWidth / cellSize;
            cells = new Soldier[numberOfCells, numberOfCells];
        }

        // Add a soldier to the grid
        public void Add(Soldier soldier)
        {
            // Determine which grid cell the soldier is in
            int cellX = (int)(soldier.soldierTrans.position.x / cellSize);
            int cellZ = (int)(soldier.soldierTrans.position.z / cellSize);

            // Add the soldier to the fron of the lsit for the cell it's in
            soldier.previousSoldier = null;
            soldier.nextSoldier = cells[cellX, cellZ];

            // Associate this cell with this soldier
            cells[cellX, cellZ] = soldier;

            if (soldier.nextSoldier != null)
            {
                // set this soldier to be the previous soldier of the next soldier of this soldier
                soldier.nextSoldier.previousSoldier = soldier;
            }
        }

        // TOUR STOP 04 - Get the closest enemy from the grid
        public Soldier FindClosestEnemy(Soldier friendlySoldier)
        {
            // Determine which cell the friendly soldier is in
            int cellX = (int)(friendlySoldier.soldierTrans.position.x / cellSize);
            int cellZ = (int)(friendlySoldier.soldierTrans.position.z / cellSize);

            // Get the first enemy in that cell (the one at the head of that cell's linked list of soldiers)
            Soldier enemy = cells[cellX, cellZ];

            // Find the closest soldier of all in the linked list
            Soldier closestSoldier = null;
            float bestDistSqr = Mathf.Infinity;
            while (enemy != null)
            {
                float distSqr = (enemy.soldierTrans.position - friendlySoldier.soldierTrans.position).sqrMagnitude;
                if (distSqr < bestDistSqr)
                {
                    bestDistSqr = distSqr;
                    closestSoldier = enemy;
                }
                enemy = enemy.nextSoldier;
            }
            return closestSoldier;
        }

        // When a soldier moves, see if we need to update which cell the soldier is in
        public void Move(Soldier soldier, Vector3 oldPos)
        {
            // Determine which cell the soldier _was_ in
            int oldCellX = (int)(oldPos.x / cellSize);
            int oldCellZ = (int)(oldPos.z / cellSize);

            // Determine which cell the soldier is in now
            int cellX = (int)(soldier.soldierTrans.position.x / cellSize);
            int cellZ = (int)(soldier.soldierTrans.position.z / cellSize);

            if (cellX == oldCellX && cellZ == oldCellZ)
            {
                return;
            }

            // unlink the soldier from its old cell
            if (soldier.previousSoldier != null)
            {
                soldier.previousSoldier.nextSoldier = soldier.nextSoldier;
            }
            if (soldier.nextSoldier != null)
            {
                soldier.nextSoldier.previousSoldier = soldier.previousSoldier;
            }

            // If it's the head of a list, remove it
            if (cells[oldCellX, oldCellZ] == soldier)
            {
                cells[oldCellX, oldCellZ] = soldier.nextSoldier;
            }

            // Add it to the grid at its new cell
            Add(soldier);
        }
    }
}