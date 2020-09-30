using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TowerDefence.Towers;

namespace TowerDefence.Managers
{
    public class TowerManager : MonoBehaviour
    {
        public static TowerManager instance = null;

        [SerializeField]
        private List<Tower> spawnableTowers = new List<Tower>();

        private List<Tower> aliveTowers = new List<Tower>();
        private Tower towerToPurchase;

        public void PurchaseTower(TowerPlatform _platform)
        {
            Player.instance.PurchaseTower(towerToPurchase);

            Tower newTower = Instantiate(towerToPurchase);
            _platform.AddTower(newTower);
            aliveTowers.Add(newTower);
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
            towerToPurchase = spawnableTowers[0];
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}