using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TowerDefence.Managers;

namespace TowerDefence.Enemies
{
    public class Spawner : MonoBehaviour
    {
        public float SpawnRate
        {
            get
            {
                return spawnRate;
            }
        }

        [SerializeField]
        private float spawnRate = 1;

        private float currentTime = 0;
        private EnemyManager enemyManager;

        // Start is called before the first frame update
        void Start()
        {
            enemyManager = EnemyManager.instance;
        }

        // Update is called once per frame
        void Update()
        {
            // Increment the time by deltaTime if the current time is less than the SpawnRate
            if (currentTime < SpawnRate)
            {
                currentTime += Time.deltaTime;
            }
            else
            {
                currentTime = 0;
                // Attempt to spawn the enemy via the EnemyManager singleton
                if (enemyManager != null)
                {
                    enemyManager.SpawnEnemy(transform);
                }
            }
        }
    }
}