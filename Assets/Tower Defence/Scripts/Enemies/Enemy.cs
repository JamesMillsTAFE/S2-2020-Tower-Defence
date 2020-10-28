using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using TowerDefence.Towers;
using TowerDefence.Managers;

namespace TowerDefence.Enemies
{
    public class Enemy : MonoBehaviour
    {
        [System.Serializable]
        public class DeathEvent : UnityEvent<Enemy> { }

        public float XP { get { return xp; } }
        public int Money { get { return money; } }

        [Header("General Stats")]
        [SerializeField, Tooltip("How fast the enemy will move within the game.")]
        private float speed = 1;
        [SerializeField, Tooltip("How much damage the enemy can take before dying.")]
        private float health = 1;
        [SerializeField, Tooltip("How much damage the enemy will do to the player's health.")]
        private float damage = 1;
        [SerializeField, Tooltip("How big is the enemy visually?")]
        private float size = 1;
        // RESISTANCE HERE

        [Header("Rewards")]
        [SerializeField, Tooltip("The amount of experience the killing tower will gain from killing the enemy.")]
        private float xp = 1;
        [SerializeField, Tooltip("The amount of money the player will gain upon killing the enemy.")]
        private int money = 1;

        [Space]

        [SerializeField]
        private DeathEvent onDeath = new DeathEvent();

        private Player player; // The reference to the player gameObject within the scene.

        /// <summary>
        /// Handles damage of the enemy and if below or equal to 0, calls Die
        /// </summary>
        /// <param name="_tower">The tower doing the damage to the enemy.</param>
        public void Damage(float _damage)
        {
            health -= _damage;
            if (health <= 0)
            {
                Die();
            }
        }

        /// <summary>
        /// Handles the visual, and technical features of dying, such as giving the tower experience.
        /// </summary>
        private void Die()
        {
            onDeath.Invoke(this);

            // Visuals
        }

        // Start is called before the first frame update
        void Start()
        {
            // Accessing the only player in the game.
            player = Player.instance;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}