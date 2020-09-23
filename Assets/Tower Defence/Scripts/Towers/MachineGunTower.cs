using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefence.Towers
{
    public class MachineGunTower : Tower
    {
        protected override void RenderAttackVisuals()
        {
            Debug.Log("I am a machine gun tower");
        }
    }
}