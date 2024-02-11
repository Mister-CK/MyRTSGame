using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MyRTSGame.Model
{
    public class SelectionManager : MonoBehaviour
    {
        [SerializeField] private GameEvent onSelectionEvent;
        [SerializeField] private GameEvent onDeselectionEvent;
        [SerializeField] private GameEvent onDeleteEvent;

        [SerializeField] private Button deleteButton;
        [SerializeField] private TextMeshProUGUI textComponent;
        
        private ISelectable CurrentSelectedObject { get; set; }
        
        private void OnEnable()
        {
            onSelectionEvent.RegisterListener(SelectObject);
            onDeleteEvent.RegisterListener(DeleteSelectedObject);
        }

        private void OnDisable()
        {
            onSelectionEvent.UnregisterListener(SelectObject);
            onDeleteEvent.UnregisterListener(DeleteSelectedObject);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                onDeselectionEvent.Raise(null);
                CurrentSelectedObject = null;
            }

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
                           "Capacity: " + building.GetCapacity() + "\n" +
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
                var destinationString = villager.GetHasDestination() ? "hasDestination" : "noDestination" + "\n";
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

        private void SelectObject(IGameEventArgs args)
        {
            if (args is not SelectionEventArgs selectionEventArgs) return;

            ISelectable newObject = selectionEventArgs.SelectedObject;

            if (CurrentSelectedObject != null)
            {
                // You can add code here to hide the details of the previously selected object
            }

            CurrentSelectedObject = newObject;
        }

        private void DeleteSelectedObject(IGameEventArgs args)
        {
            if (CurrentSelectedObject is Building selectedBuilding)
            {
                Destroy(selectedBuilding.gameObject);
                SelectObject(null);
            }
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

        private static string GetTextForInventory(IEnumerable<Resource> inventory)
        {
            return string.Join(" ", inventory.Select(resource => $"{resource.ResourceType}:{resource.Quantity}"));
        }
        
        private static string GetTextForResourcesInJobsForBuilding(IEnumerable<Resource> resInJobs)
        {
            return string.Join(" ", resInJobs.Select(resource => $"{resource.ResourceType}:{resource.Quantity}"));
        }
    }
}