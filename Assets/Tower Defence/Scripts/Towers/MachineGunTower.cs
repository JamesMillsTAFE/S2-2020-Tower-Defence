using System.Collections;
using System.Collections.Generic;
using TowerDefence.Towers;
using UnityEngine;
using TowerDefence.Utilities;


namespace TowerDefence.Towers
{
    public class MachineGunTower : Tower
    {
        [Header("Machine Gun Specifics")]
        [SerializeField]
        private Transform turret;
        [SerializeField]
        private Transform gunHolder;
        [SerializeField]
        private LineRenderer bulletLine;
        [SerializeField]
        private Transform leftFirePoint;
        [SerializeField]
        private Transform rightFirePoint;

        private bool fireLeft = false;

        private float resetTime = 0;
        private bool hasResetVisuals = false;

        protected override void RenderAttackVisuals()
        {
            MathUtils.DistanceAndDirection(out float _distance, out Vector3 direction, gunHolder, TargetedEnemy.transform);
            gunHolder.rotation = Quaternion.LookRotation(direction);

            if (fireLeft)
            {
                RenderBulletLine(leftFirePoint);
            }
            else
            {
                RenderBulletLine(rightFirePoint);
            }

            fireLeft = !fireLeft;
            hasResetVisuals = false;
        }

        protected override void RenderLevelUpVisuals()
        {
            Debug.Log("I am leveling up!");
        }

        protected override void Update()
        {
            base.Update();

            // Detect if we have no enemy AND that we haven't already reset the visuals
            if (TargetedEnemy == null && !hasResetVisuals)
            {
                // Check if the current time is less than the fire rate
                if (resetTime < fireRate)
                {
                    // Add to the currentTime
                    resetTime += Time.deltaTime;
                }
                else
                {
                    // Disable line renderer
                    // Reset timer to 0
                    // Set reset visuals flag to true
                    bulletLine.positionCount = 0;
                    resetTime = 0;
                    hasResetVisuals = true;
                }
            }
        }

        private void RenderBulletLine(Transform _start)
        {
            // Spawns a line with two points from the start to the targeted enemy
            bulletLine.positionCount = 2;
            bulletLine.SetPosition(0, _start.position);
            bulletLine.SetPosition(1, TargetedEnemy.transform.position);
        }
    }
}