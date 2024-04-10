using System.Collections.Generic;
using UnityEngine;

namespace MyRTSGame.Model
{
    public class BuildingList : MonoBehaviour
    {
        private List<Building> _buildings;
        public static BuildingList Instance { get; private set; }
        private bool _firstWarehouse = true;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            _buildings = new List<Building>(FindObjectsOfType<Building>());
        }

        public void SetFirstWareHouse(bool firstWarehouse)
        {
            _firstWarehouse = firstWarehouse;
        }
        
        public bool GetFirstWareHouse()
        {
            return _firstWarehouse;
        }
        
        public List<Building> GetBuildings()
        {
            return _buildings;
        }

        public void AddBuilding(Building newBuilding)
        {
            _buildings.Add(newBuilding);
        }
        public void RemoveBuilding(Building newBuilding)
        {
            _buildings.Remove(newBuilding);
        }
    }
}