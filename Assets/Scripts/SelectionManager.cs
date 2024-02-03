using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MyRTSGame.Model
{
    public class SelectionManager : MonoBehaviour
    {
        public static SelectionManager Instance;

        [SerializeField] private Button deleteButton;
        [SerializeField] private TextMeshProUGUI textComponent;
        public ISelectable CurrentSelectedObject { get; private set; }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(1)) CurrentSelectedObject = null;

            if (CurrentSelectedObject == null)
            {
                textComponent.text = "";
                SetDeleteButton(false);
                return;
            }

            if (CurrentSelectedObject is Building building)
            {
                var text = building.BuildingType + "\n" +
                           GetTextForInventory(building.GetInventory()) + "\n" +
                           building.GetState() + "\n" +
                           GetTextForInputTypes(building.InputTypes) + "\n" +
                           "Capacity: " + building.GetCapacity() +
                           building.InputTypes + "\n" +
                           GetTextForResourcesInJobsForBuilding(building.GetResourcesInJobForBuilding());
                           
                textComponent.text = text;
                SetDeleteButton(true);
                return;
            }

            if (CurrentSelectedObject is Villager villager)
            {
                var text = "";
                text += "Villager" + "\n";
                var job = villager.GetCurrentJob();
                var destinationString = villager.HasDestination() ? "hasDestination" : "noDestination" + "\n";
                if (job != null)
                    text += job.Origin.BuildingType + "\n" 
                                                    + job.ResourceType + "\n" 
                                                    + job.Destination.BuildingType + "\n"
                                                    + destinationString;    
                else
                    text += "No job" + "\n";

                textComponent.text = text;
                SetDeleteButton(true);
            }
        }

        public void SelectObject(ISelectable newObject)
        {
            if (CurrentSelectedObject != null)
            {
                // You can add code here to hide the details of the previously selected object
            }

            CurrentSelectedObject = newObject;
        }

        private void SetDeleteButton(bool show)
        {
            deleteButton.gameObject.SetActive(show);
        }

        private static string GetTextForInputTypes(IEnumerable<ResourceType> inputTypes)
        {
            return inputTypes.Aggregate("input types: ", (current, resourceType) => current + resourceType + ", ")
                .TrimEnd(',', ' ');
        }

        private static string GetTextForInventory(Resource[] inventory)
        {
            var inventoryText = "";
            foreach (var resource in inventory) inventoryText += resource.ResourceType + ":" + resource.Quantity + " ";

            return inventoryText;
        }
        
        private static string GetTextForResourcesInJobsForBuilding(Resource[] resInJobs)
        {
            var resInJobsText = "";
            foreach (var resource in resInJobs) resInJobsText += resource.ResourceType + ":" + resource.Quantity + " ";

            return resInJobsText;
        }
    }
}