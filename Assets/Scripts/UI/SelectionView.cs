using System.Collections.Generic;
using System.Linq;
using MyRTSGame.Model;
using MyRTSGame.Model.UnitViews;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectionView : MonoBehaviour
{
    [SerializeField] private FoundationStateBuildingUIView foundationStateBuildingUIView;
    [SerializeField] private ConstructionStateBuildingUIView constructionStateBuildingUIView;
    [SerializeField] private CompletedStateBuildingUIView completedStateBuildingUIView;
    [SerializeField] private UnitUIView unitUIView;
    [SerializeField] public SelectionController selectionController;
    private GameObject _currentGrid = null; 
    private Dictionary<ResourceType, TextMeshProUGUI> _resourceTexts = new Dictionary<ResourceType, TextMeshProUGUI>();
    private ISelectable CurrentSelectedObject { get; set; }

    private void LateUpdate()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (CurrentSelectedObject != null)
            {
                ClearView();
                CurrentSelectedObject = null;
            }
        }

        UpdateView(CurrentSelectedObject);
    }

    public void UpdateView(ISelectable selectable)
    {
        if (selectable != CurrentSelectedObject) return;
        switch (selectable)
        {
            case Building building:
                UpdateSelectedBuilding(building);
                break;
            case Unit unit:
                UpdateSelectedUnit(unit);
                break;
        }
    }
    public void SetView(ISelectable selectable)
    {
        ClearView();
        switch (selectable)
        {
            case Building building:
                SetSelectedBuilding(building);
                break;
            case Unit unit:
                SetSelectedUnit(unit);
                break;
        }
    }

    private void UpdateSelectedUnit(Unit unit)
    {
        unitUIView.UpdateView(unit);
    }
    
    private void SetSelectedUnit(Unit unit)
    {
        unitUIView.ActivateView(unit);
    }
    
    private void UpdateSelectedBuilding(Building building)
    {
        if (building.State is FoundationState)
        {
            foundationStateBuildingUIView.UpdateResourceQuantities(building);
            return;
        }
        if (building.State is ConstructionState)
        {
            // Nothing to update for construction state
            return;
        }
        
        if (building is Warehouse warehouse)
        {
            foreach (var resource in warehouse.GetInventory())
            {
                if (_resourceTexts.TryGetValue(resource.Key, out var newTextComponent))
                {
                    newTextComponent.text = $"{resource.Key}: {resource.Value.Current}";
                }
            }
            return;
        }
        
        if (building.State is CompletedState)
        {
            completedStateBuildingUIView.UpdateResourceQuantities(building);
            completedStateBuildingUIView.SetOccupantButton(building);
            return;
        }
    }
    
    private void SetSelectedBuilding(Building building)
    {
        if (building.State is FoundationState)
        {
            foundationStateBuildingUIView.ActivateFoundationStateBuildingView(building);
            return;
        }
        if (building.State is ConstructionState)
        {
            constructionStateBuildingUIView.ActivateConstructionStateBuildingView(building);
            return;
        }
        
        if (building is Warehouse warehouse)
        {
            CreateResourceGridForBuilding(warehouse);
            return;
        }
        
        if (building.State is CompletedState)
        {
            completedStateBuildingUIView.ActivateBuildingView(building);
            return;
        }
    }
    
    public void ClearView()
    {
        _resourceTexts = new Dictionary<ResourceType, TextMeshProUGUI>();

        if (_currentGrid != null)
        {
            Destroy(_currentGrid);
            _resourceTexts.Clear();
        }
        
        unitUIView.DeactivateView();
        completedStateBuildingUIView.DeactivateBuildingView();
        foundationStateBuildingUIView.DeactivateFoundationStateBuildingView();
        constructionStateBuildingUIView.DeactivateConstructionStateBuildingView();
    }

    public void SelectObject(ISelectable selectable)
    {
        if (CurrentSelectedObject != null && CurrentSelectedObject != selectable)
        {
            ClearView();
        }

        CurrentSelectedObject = selectable;

        SetView(CurrentSelectedObject);
    }
    
    public void HandleOccupantButtonClick()
    {
        Debug.Log("HandleOccupantButtonClick clicked");
        if (CurrentSelectedObject is not Building building) return;
        Debug.Log(building.GetOccupant());
        SelectObject(building.GetOccupant());
    }
    
    public void HandleDeleteButtonClick()
    {
        selectionController.CreateDeleteEvent(CurrentSelectedObject);
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
            textComponentCell.text = $"{resource.Key}: {resource.Value.Current}";
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
            _resourceTexts[resource.Key] = textComponentCell;
        }
        
    }
}