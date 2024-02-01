using System.Collections.Generic;
using UnityEngine;

namespace MyRTSGame.Model
{
    public class BuildingList : MonoBehaviour
    {
        // [SerializeField] private TextMeshProUGUI textComponent;
        private List<Building> _buildings;
        public static BuildingList Instance { get; private set; }

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

        // void Update()
        // {
        //     // UpdateList();
        // }
        // private void UpdateList()
        // {
        //     string buildingList = "";
        //     foreach (Building building in _buildings)
        //     {
        //         Resource[] inventory = building.GetInventory();
        //         string inventoryText = GetTextForInventory(inventory);
        //         buildingList += building.BuildingType + ": " + inventoryText + "\n";
        //     }
        //     textComponent.text = buildingList;
        // }

        // private static string GetTextForInventory(Resource[] inventory) {
        //     string inventoryText = "";
        //     foreach (Resource resource in inventory) {
        //         inventoryText += resource.ResourceType + ":" + resource.Quantity + " ";
        //     }
        //     return inventoryText;
        // }

        public List<Building> GetBuildings()
        {
            return _buildings;
        }

        public void AddBuilding(Building newBuilding)
        {
            _buildings.Add(newBuilding);
        }
    }
}