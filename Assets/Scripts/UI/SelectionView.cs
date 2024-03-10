using System.Collections.Generic;
using System.Linq;
using MyRTSGame.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectionView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textComponent;
    [SerializeField] private Button deleteButton;
    [SerializeField] private Button newVillButton; 
    [SerializeField] private ResourceBuildingUIView resourceBuildingUIView;
    private GameObject _currentGrid = null; 
    private Dictionary<ResourceType, TextMeshProUGUI> _resourceTexts = new Dictionary<ResourceType, TextMeshProUGUI>();
    
    public void UpdateView(ISelectable selectable)
    {
        switch (selectable)
        {
            case Building building:
                UpdateSelectedBuilding(building);
                break;
            case Villager villager:
                SetSelectedVillager(villager);
                break;
        }
    }
    public void SetView(ISelectable selectable)
    {
        // Destroy the existing grid if it exists
        if (_currentGrid != null)
        {
            Destroy(_currentGrid);
            _resourceTexts.Clear();
            newVillButton.gameObject.SetActive(false);
        }

        // is this check necessary?
        if (resourceBuildingUIView != null)
        {
            resourceBuildingUIView.DeactivateResourceBuildingView();
        }

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

    private void UpdateSelectedBuilding(Building building)
    {
        var text = building.BuildingType + 
                   // GetTextForInventory(building.GetInventory()) + "\n" +
                   "\n" + GetTextForInputTypes(building.InputTypes) +
                   "\n" + GetTextForResourcesInJobsForBuilding(building.GetResourcesInJobForBuilding());
        
        switch (building)
        {
            case ResourceBuilding _:
                text += "\nBuilding Class: Resource Building";
                break;
            case ProductionBuilding _:
                text += "\nBuilding Class: Production Building";
                break;
            case WorkshopBuilding _:
                text += "\nBuilding Class: Workshop Building";
                break;
            case SpecialBuilding _:
                //TODO: refactor this into seperate methods per building
                //warehouse
                switch (building) 
                {
                    case Warehouse:
                        // Populate the grid with resources and their counts
                        foreach (var resource in building.GetInventory())
                        {
                            if (_resourceTexts.TryGetValue(resource.ResourceType, out var newTextComponent))
                            {
                                newTextComponent.text = $"{resource.ResourceType}: {resource.Quantity}";
                            }
                        }
                        break;
                }
                text += "\nBuilding Class: Special Building";
                break;
        }
                           
        textComponent.text = text;
        SetDeleteButton(true);
    }
    
    private void SetSelectedBuilding(Building building)
    {
        var text = "" + building.BuildingType;
        
        switch (building)
        {
            case ResourceBuilding resourceBuilding:
                resourceBuildingUIView.ActivateResourceBuildingView(resourceBuilding);
                break;
            case ProductionBuilding _:
                text += "\nBuilding Class: Production Building" + "\n" + GetTextForInputTypes(building.InputTypes);
                text += "\n" + GetTextForResourcesInJobsForBuilding(building.GetResourcesInJobForBuilding());     
                break;
            case WorkshopBuilding _:
                text += "\nBuilding Class: Workshop Building";
                text += "\n" + GetTextForResourcesInJobsForBuilding(building.GetResourcesInJobForBuilding());     
                break;
            case SpecialBuilding _:
                switch (building) 
                {
                    case Warehouse:
                        CreateResourceGridForBuilding(building);
                        break;
                    case Restaurant:
                        // CreateResourceGridForBuilding(building);
                        break;
                    case School:
                        newVillButton.gameObject.SetActive(true);
                        newVillButton.transform.SetParent(transform);
                        // CreateResourceGridForBuilding(building);
                        break;
                    case Castle:
                        // CreateResourceGridForBuilding(building);
                        break;
                    case GuardTower:
                        // CreateResourceGridForBuilding(building);
                        break;
                }
                break;
        }

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
        _resourceTexts = new Dictionary<ResourceType, TextMeshProUGUI>();

        if (_currentGrid != null)
        {
            Destroy(_currentGrid);
            _resourceTexts.Clear();
        }
        newVillButton.gameObject.SetActive(false);
        
        // is this check necessary?
        if (resourceBuildingUIView != null)
        {
            resourceBuildingUIView.DeactivateResourceBuildingView();
        }
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
    
    private void CreateResourceGridForBuilding(Building building)
    {
        // Create a new GameObject to serve as the parent for your grid
        _currentGrid = new GameObject("Grid");
        _currentGrid.transform.SetParent(transform); // Set the parent to the current transform

        // Add a GridLayoutGroup component to the GameObject
        GridLayoutGroup gridLayoutGroup = _currentGrid.AddComponent<GridLayoutGroup>();

        // Set the properties of the GridLayoutGroup
        gridLayoutGroup.cellSize = new Vector2(70, 25); // Set the size of each cell in the grid
        gridLayoutGroup.spacing = new Vector2(10, 10); // Set the spacing between cells
        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount; // Set the constraint to FixedColumnCount
        gridLayoutGroup.constraintCount = 4; // Set the number of columns to 4
        gridLayoutGroup.childAlignment = TextAnchor.MiddleCenter; // Center the cells

            
        // // Set the RectTransform properties of the gridObject to align its top left with the top left of its parent
        RectTransform gridRectTransform = _currentGrid.GetComponent<RectTransform>();
        gridRectTransform.anchorMin = new Vector2(.5f, .5f); // Position the anchors at the top left of the parent
        gridRectTransform.anchorMax = new Vector2(.5f, .5f); // Position the anchors at the top left of the parent
        gridRectTransform.pivot = new Vector2(.5f, .5f); // Position the pivot at the top left of the RectTransform
        gridRectTransform.anchoredPosition = Vector2.zero; // Position the RectTransform at its anchors

        // Populate the grid with resources and their counts
        foreach (var resource in building.GetInventory())
        {
            // Create a new GameObject to represent the cell
            GameObject cellObject = new GameObject("Cell");
            cellObject.transform.SetParent(_currentGrid.transform);

            // Add an Image component to the cell GameObject and set its color
            Image cellImage = cellObject.AddComponent<Image>();
            cellImage.color = Color.yellow; // Set the background color of the cell

            // Create a new TextMeshProUGUI object to display the resource type and quantity
            GameObject textObject = new GameObject("ResourceText");
            TextMeshProUGUI textComponentCell = textObject.AddComponent<TextMeshProUGUI>();
            textComponentCell.text = $"{resource.ResourceType}: {resource.Quantity}";
            textComponentCell.color = Color.black; // Set the color of the text
            textComponentCell.fontSize = 10; // Set the font size to make the text smaller
            textComponentCell.alignment = TextAlignmentOptions.Center; // Center the text

            // Add the TextMeshProUGUI object to the cell
            textObject.transform.SetParent(cellObject.transform);
            RectTransform textRectTransform = textObject.GetComponent<RectTransform>();
            textRectTransform.anchorMin = Vector2.zero; // Set the minimum anchor to the bottom left corner
            textRectTransform.anchorMax = Vector2.one; // Set the maximum anchor to the top right corner
            textRectTransform.pivot = new Vector2(0.5f, 0.5f); // Position the pivot at the center of the RectTransform
            textRectTransform.anchoredPosition = Vector2.zero; // Position the RectTransform at its anchors
            textRectTransform.sizeDelta = Vector2.zero; // Let the text fill the cell
            
            // Add the TextMeshProUGUI object to the resourceTexts dictionary
            _resourceTexts[resource.ResourceType] = textComponentCell;
        }
        
    }
}