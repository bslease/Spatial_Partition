using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpatialPartitionPattern
{
    public class GameController : MonoBehaviour
    {
        // depends on: Soldier, Grid, Enemy, Friendly

        public GameObject friendlyObj;
        public GameObject enemyObj;

        public Material enemyMaterial;
        public Material closestEnemyMaterial;

        public Transform enemyParent;
        public Transform friendlyParent;

        List<Soldier> enemySoldiers = new List<Soldier>();
        List<Soldier> friendlySoldiers = new List<Soldier>();

        List<Soldier> closestEnemies = new List<Soldier>();

        // Grid data
        float mapWidth = 50f;
        int cellSize = 10;

        // Number of soldiers on each team
        int numberOfSoldiers = 100;

        // The spatial partition grid
        Grid grid;

        // Start is called before the first frame update
        void Start()
        {
            grid = new Grid((int)mapWidth, cellSize);

            for (int i = 0; i < numberOfSoldiers; i++)
            {
                // add random enemies and friendlies and store them in a list
                Vector3 randomPos = new Vector3(Random.Range(0f, mapWidth), 0.5f, Random.Range(0f, mapWidth));
                GameObject newEnemy = Instantiate(enemyObj, randomPos, Quaternion.identity) as GameObject;
                enemySoldiers.Add(new Enemy(newEnemy, mapWidth, grid));
                newEnemy.transform.parent = enemyParent;

                randomPos = new Vector3(Random.Range(0f, mapWidth), 0.5f, Random.Range(0f, mapWidth));
                GameObject newFriendly = Instantiate(friendlyObj, randomPos, Quaternion.identity) as GameObject;
                friendlySoldiers.Add(new Friendly(newFriendly, mapWidth));
                newFriendly.transform.parent = friendlyParent;
            }
        }

        // Update is called once per frame
        void Update()
        {
            // move the enemies
            for (int i=0; i < enemySoldiers.Count; i++)
            {
                enemySoldiers[i].Move();
            }

            // reset material of the closest enemies
            for (int i=0; i < closestEnemies.Count; i++)
            {
                closestEnemies[i].soldierMeshRenderer.material = enemyMaterial;
            }

            // reset the list with the closest enemies
            closestEnemies.Clear();
            for (int i=0; i < friendlySoldiers.Count; i++)
            {
                Soldier closestEnemy = grid.FindClosestEnemy(friendlySoldiers[i]);
                if (closestEnemy != null)
                {
                    closestEnemy.soldierMeshRenderer.material = closestEnemyMaterial;
                    closestEnemies.Add(closestEnemy);
                    friendlySoldiers[i].Move(closestEnemy);
                }
            }
        }
    }
}
