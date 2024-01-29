using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MyRTSGame.Model
{
    public class BuildingList : MonoBehaviour
    {
        public static BuildingList Instance { get; private set; }

        // [SerializeField] private TextMeshProUGUI textComponent;
        private List<Building> _buildings;

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

        private void Update()
        {
            // UpdateList();
        }
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

        public List<Building> GetBuildings() {
            return _buildings;
        }

        public void AddBuilding(Building newBuilding)
        {
            _buildings.Add(newBuilding);
        }
    }
}