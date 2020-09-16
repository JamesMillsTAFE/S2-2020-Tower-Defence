using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TowerDefence.Enemies;

namespace TowerDefence.Managers
{
    public class EnemyManager : MonoBehaviour
    {
        public static EnemyManager instance = null;

        [SerializeField]
        private GameObject enemyPrefab;

        private List<Enemy> aliveEnemies = new List<Enemy>();

        public void SpawnEnemy(Transform _spawner)
        {
            GameObject newEnemy = Instantiate(enemyPrefab, _spawner.position, enemyPrefab.transform.rotation);
            aliveEnemies.Add(newEnemy.GetComponent<Enemy>());
        }

        /// <summary>
        /// Loops through all enemies alive in the game, finding the closest enemies within a certain range.
        /// </summary>
        /// <param name="_target">The object we are comparing the distance to.</param>
        /// <param name="_maxRange">The range we are finding enemies within.</param>
        /// <param name="_minRange">The range the enemies must at least be from the target.</param>
        /// <returns>The list of enemies within the given range.</returns>
        public Enemy[] GetClosestEnemies(Transform _target, float _maxRange, float _minRange = 0)
        {
            List<Enemy> closeEnemies = new List<Enemy>();

            foreach(Enemy enemy in aliveEnemies)
            {
                // Detect if the enemy is within the specified range, if so, add it to the list
                float distance = Vector3.Distance(enemy.transform.position, _target.position);
                if (distance < _maxRange && distance > _minRange)
                {
                    closeEnemies.Add(enemy);
                }
            }

            // Converts the list to an array
            return closeEnemies.ToArray();
        }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}