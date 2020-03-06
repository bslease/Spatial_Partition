using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpatialPartitionPattern
{
    public class Enemy : Soldier
    {
        Vector3 currentTarget;  // the position the soldier is heading for
        Vector3 oldPos;         // the position the soldier was in
        float mapWidth;
        Grid grid;

        // Enemy constructor
        public Enemy(GameObject soldierObj, float mapWidth, Grid grid)
        {
            this.soldierTrans = soldierObj.transform;
            this.soldierMeshRenderer = soldierObj.GetComponent<MeshRenderer>();
            this.mapWidth = mapWidth;
            this.grid = grid;

            // add this unit to the grid
            grid.Add(this);
            oldPos = soldierTrans.position;
            this.walkSpeed = 5f;
            GetNewTarget();
        }

        void GetNewTarget()
        {
            currentTarget = new Vector3(Random.Range(0f, mapWidth), 0.5f, Random.Range(0f, mapWidth));
            // rotate toward the target
            soldierTrans.rotation = Quaternion.LookRotation(currentTarget - soldierTrans.position);
        }

        public override void Move()
        {
            // move toward the target
            soldierTrans.Translate(Vector3.forward * Time.deltaTime * walkSpeed);
            // see if the soldier has moved to another cell
            grid.Move(this, oldPos);
            // save current position as oldPos for future grid.Move checks
            oldPos = soldierTrans.position;

            if ((soldierTrans.position - currentTarget).magnitude < 1f)
            {
                GetNewTarget();
            }

        }
    }
}