using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MyRTSGame.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectionView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textComponent;
    [SerializeField] private Button deleteButton;

    public void UpdateView(ISelectable selectable)
    {
        switch (selectable)
        {
            case Building building:
                SetSelectedBuilding(building);
                break;
            case Villager villager:
                SetSelectedVillager(villager);
                break;
        }
    }

    private void SetSelectedBuilding(Building building)
    {
        
        var text = building.BuildingType + "\n" +
                   GetTextForInventory(building.GetInventory()) + "\n" +
                   GetTextForInputTypes(building.InputTypes) + "\n" +
                   "Capacity: " + building.GetCapacity() + "\n" +
                   GetTextForResourcesInJobsForBuilding(building.GetResourcesInJobForBuilding());
                           
        textComponent.text = text;
        SetDeleteButton(true);
    }
    
    //TODO: refactor this method to work for all units.
    private void SetSelectedVillager(Villager villager)
    {
        var text = "Villager" + "\n";
        var job = villager.GetCurrentJob();
        var destinationString = villager.GetHasDestination() ? "hasDestination" : "noDestination" + "\n";
        if (job != null)
            text += job.Origin.BuildingType + "\n" +
                    job.ResourceType + "\n" +
                    job.Destination.BuildingType + "\n" +
                    destinationString;
        textComponent.text = text;
        SetDeleteButton(true);
    }

    private void SetDeleteButton(bool show)
    {
        deleteButton.gameObject.SetActive(show);
    }
    
    public void ClearView()
    {
        textComponent.text = "";
        SetDeleteButton(false);
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