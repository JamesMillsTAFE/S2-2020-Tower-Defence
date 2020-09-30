using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TowerDefence.Enemies;
using TowerDefence.Managers;


namespace TowerDefence.Towers
{
    public abstract class Tower : MonoBehaviour
    {
        #region Properties
        public string TowerName // The public accessor for the towerName variable.
        {
            get => towerName;
        }

        public string Description // The public accessor for the description variable.
        {
            get => description;
        }


        public int Cost // The public accessor for the cost variable.
        {
            get => cost;
        }

        /// <summary>
        /// Gets a formatted string containing all of the description, Tower properties,
        /// name and cost to be displayed on the UI.
        /// </summary>
        public string UiDisplayText
        {
            get
            {
                // .ToString is not necessary
                // string.Format - Formats a string based on the first parameter, replacing any {X} with the relevant parameter
                // \n = new line
                string display = string.Format("Name: {0} Cost: {1}\n", TowerName, Cost.ToString());
                display += Description + "\n";
                display += string.Format("Min Range: {0}, Max Range: {1}, Damage: {2}", minimumRange.ToString(), maximumRange.ToString(), damage.ToString());
                return display;
            }
        }


        /// <summary>
        /// Calculates the required experience based on the current level
        /// and the experience scalar.
        /// </summary>
        private float RequiredXP
        {
            get
            {
                // if the level is equal to 1, simply return the baseRequiredXp
                if (level == 1)
                {
                    return baseRequiredXp;
                }


                // Multiply the level by the experienceScalar to get the multiplier
                // for the baseRequiredXp
                return baseRequiredXp * (level * experienceScaler);
            }
        }


        /// <summary>
        /// The maximum range the tower can reach, based on it's level
        /// </summary>
        public float MaximumRange
        {
            get
            {
                // Multiplying is faster than dividing
                return maximumRange * (level * 0.5f + 0.5f);
            }
        }


        /// <summary>
        /// The amount of damage the tower does, multiplied by it's level.
        /// </summary>
        public float Damage
        {
            get
            {
                // Multiplying is faster than dividing
                return damage * (level * 0.5f + 0.5f);
            }
        }


        /// <summary>
        /// The Enemy the turret is currently targeting, if no enemy is targeted this is null.
        /// </summary>
        protected Enemy TargetedEnemy
        {
            get
            {
                return target;
            }
        }
        #endregion


        [Header("General Stats")]
        [SerializeField]
        private string towerName = "";
        [SerializeField, TextArea]
        private string description = "";
        [SerializeField, Range(1, 10)]
        private int cost = 1;

        [Header("Attack Stats")]
        [SerializeField, Min(0.1f)]
        private float damage = 1;
        [SerializeField, Min(0.1f)]
        private float minimumRange = 1;
        [SerializeField]
        private float maximumRange = 5;
        [SerializeField, Min(0.1f)]
        protected float fireRate = 0.1f;

        [Header("Experience Stats")]
        [SerializeField, Range(2, 5)]
        private int maxLevel = 3;
        [SerializeField, Min(1)]
        private float baseRequiredXp = 5;
        [SerializeField, Min(1)]
        private float experienceScaler = 1;


        private int level = 1;
        private float xp = 0;
        private Enemy target = null;


        private float currentTime = 0;


#if UNITY_EDITOR
        // OnValidate runs whenever a variable is changed within the inspector of this class
        private void OnValidate()
        {
            maximumRange = Mathf.Clamp(maximumRange, minimumRange + 1, float.MaxValue);
        }


        // OnDrawGizmosSelected draws helpful visuals only when the object is selected
        private void OnDrawGizmosSelected()
        {
            // Draw a mostly transparent red sphere indicating the minimum range
            Gizmos.color = new Color(1, 0, 0, 0.25f);
            Gizmos.DrawSphere(transform.position, minimumRange);


            // Draw a mostly transparent blue sphere indicating the maximum range
            Gizmos.color = new Color(0, 0, 1, 0.25f);
            Gizmos.DrawSphere(transform.position, MaximumRange);
        }
#endif


        public void AddExperience(float _xp)
        {
            xp += _xp;
            // Check that the level is not maxed out and that we have 
            // passed the required experience to level up
            if (level < maxLevel)
            {
                if (xp >= RequiredXP)
                {
                    LevelUp();
                }
            }
        }

        protected abstract void RenderAttackVisuals();
        protected abstract void RenderLevelUpVisuals();

        private void LevelUp()
        {
            level++;
            xp = 0;

            RenderLevelUpVisuals();
        }


        private void Fire()
        {
            // Make sure that there is actually something to target, if there is, damage it
            if (target != null)
            {
                target.Damage(this);

                RenderAttackVisuals();
            }
        }

        private void FireWhenReady()
        {
            // Make sure that there is actually a target
            if (target != null)
            {
                // If the timer is less than the fireRate, add the deltaTime
                // to make sure the turret fires in real time.
                if (currentTime < fireRate)
                {
                    currentTime += Time.deltaTime;
                }
                else
                {
                    // Reset the current time and fire.
                    currentTime = 0;
                    Fire();
                }
            }
        }

        private void Target()
        {
            // Get enemies within range
            Enemy[] closeEnemies = EnemyManager.instance.GetClosestEnemies(transform, MaximumRange, minimumRange);

            // Call get closest enemy
            target = GetClosestEnemy(closeEnemies);
        }


        // _enemies is the array of enemies within range
        private Enemy GetClosestEnemy(Enemy[] _enemies)
        {
            float closestDist = float.MaxValue;
            Enemy closest = null;

            foreach (Enemy enemy in _enemies)
            {
                // Distance between us and the enemy
                float distToEnemy = Vector3.Distance(enemy.transform.position, transform.position);

                // If the enemy is closer than the current, make it the closest
                // and the distance the new closest distance
                if (distToEnemy < closestDist)
                {
                    closestDist = distToEnemy;
                    closest = enemy;
                }
            }

            return closest;
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            Target();
            FireWhenReady();
        }
    }
}