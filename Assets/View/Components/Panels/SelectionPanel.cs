using Buildings.Model;
using Enums;
using Interface;
using MyRTSGame.Model.ResourceSystem.Model;
using MyRTSGame.Model.ResourceSystem.Model.ResourceStates;
using Units.Model.Component;
using UnityEngine;
using UnityEngine.UIElements;
using View.Extensions;

namespace View.Components.Panels
{
    public class SelectionPanel : HUDPanel
    {
        private ISelectable CurrentSelectedObject { get; set; }
        
        // --- CACHED UNIT UI ELEMENTS ---
        private VisualElement _unitPanelContent;
        private Label _unitPanelHeader;
        private Button _buildingButton;
        private Button _deleteButton;
        private Slider _staminaSlider; 
        
        // --- CACHED RESOURCE UI ELEMENTS ---
        private VisualElement _resourcePanelContent;
        private Label _resourceNameLabel; 
        private VisualElement _statusBarContainer; 
        private Slider _growthSlider; 
        private Label _resourceQuantityLabel; 
        
        private ResourceType _primaryResourceType; 

        public SelectionPanel(string id = "panel-selection") : base(id) { }
        
        public override void Build(VisualElement parent)
        {
            base.Build(parent);
            CreateUnitPanelContent();
            CreateNaturalResourcePanelContent(); 
        }
        private void CreateUnitPanelContent()
        {
            _unitPanelContent = Root.CreateChild("unit-panel-content");
            
            var topLeftContainer = _unitPanelContent.CreateChild("panel-top-left-container");
            _buildingButton = topLeftContainer.CreateChild<Button>("building-button");
            _buildingButton.text = "Building";
            _buildingButton.clicked += () => { Debug.Log("Building button clicked"); };
            
            var topMiddleContainer = _unitPanelContent.CreateChild("panel-top-middle-container");
            _unitPanelHeader = topMiddleContainer.CreateChild<Label>("selection-panel-header"); 
            
            var topRightContainer = _unitPanelContent.CreateChild("panel-top-right-container");
            _deleteButton = topRightContainer.CreateChild<Button>("delete-button");
            _deleteButton.text = "Delete";
            _deleteButton.clicked += () => { Debug.Log("Delete button clicked"); };
           
            _unitPanelContent.CreateChild("panel-bottom-container");
            
            _staminaSlider = new Slider(0, 100, SliderDirection.Horizontal, 1f);
            _staminaSlider.name = "stamina-slider";
            _staminaSlider.label = "Stamina";
            _staminaSlider.SetEnabled(false); 
            _unitPanelContent.Add(_staminaSlider);
            
            _unitPanelContent.style.display = DisplayStyle.None;
        }
        private void CreateNaturalResourcePanelContent()
        {
            _resourcePanelContent = Root.CreateChild("resource-panel-content");
            
            _resourceNameLabel = _resourcePanelContent.CreateChild<Label>("resource-name-label");
            _resourceNameLabel.AddToClassList("selection-panel-header");

            _statusBarContainer = _resourcePanelContent.CreateChild("status-bar-container");
            _statusBarContainer.style.flexDirection = FlexDirection.Column;
            
            _growthSlider = new Slider(0, 100, SliderDirection.Horizontal, 1f);
            _growthSlider.name = "growth-slider";
            _growthSlider.label = "Growth Progress";
            _growthSlider.SetEnabled(false); 
            _statusBarContainer.Add(_growthSlider);
            
            var resourceGrid = _resourcePanelContent.CreateChild("resource-grid");
            resourceGrid.style.flexDirection = FlexDirection.Row;
            resourceGrid.style.justifyContent = Justify.SpaceBetween;
            
            var typeLabel = resourceGrid.CreateChild<Label>("resource-type-text");
            typeLabel.text = "Quantity:"; 
            
            _resourceQuantityLabel = resourceGrid.CreateChild<Label>("resource-quantity-label");
            _resourceQuantityLabel.AddToClassList("quantity-value");

            _resourcePanelContent.style.display = DisplayStyle.None;
        }
        
        public void SetView(ISelectable selectable)
        {
            if (CurrentSelectedObject != null && CurrentSelectedObject != selectable) ClearView();
            CurrentSelectedObject = selectable;
            Show();

            
            switch (selectable)
            {
                case UnitComponent unit:
                    SetSelectedUnit(unit);
                    break;
                case Building building:
                    SetSelectedBuilding(building);
                    break;
                case NaturalResource resource:
                    SetSelectedResource(resource);
                    break;
                default:
                    ClearView();
                    break;
            }
        }

        private void SetSelectedUnit(UnitComponent unit)
        {
            _resourcePanelContent.style.display = DisplayStyle.None;
            _unitPanelContent.style.display = DisplayStyle.Flex;

            _unitPanelHeader.text = unit.name;
            UpdateUnitData(unit);
        }

        private void SetSelectedBuilding(Building building)
        {
            ClearView();
        }
        
        private void SetSelectedResource(NaturalResource resource)
        {
            _unitPanelContent.style.display = DisplayStyle.None;
            _resourcePanelContent.style.display = DisplayStyle.Flex;
            
            _primaryResourceType = resource.GetResourceType();
            
            _resourceNameLabel.text = resource.name;

            if (resource.GetState() is GrowingState)
            {
                _statusBarContainer.style.display = DisplayStyle.Flex;
            }
            else
            {
                _statusBarContainer.style.display = DisplayStyle.None;
            }
            
            UpdateResourceData(resource);
        }

        public void UpdateView()
        {
            if (CurrentSelectedObject is NaturalResource resource)
            {
                UpdateResourceData(resource);
                return;
            }
            if (CurrentSelectedObject is UnitComponent unit)
            {
                UpdateUnitData(unit);
                return;
            }
        }
        
        private void UpdateUnitData(UnitComponent unit)
        {
            _staminaSlider.value = unit.Data.GetStamina(); 
        }
        
        private void UpdateResourceData(NaturalResource resource)
        {
            if (resource.GetState() is GrowingState growingState)
            {
                _growthSlider.value = growingState.GetPercentageGrown(); 
            }
            
            var inventory = resource.GetInventory();
            if (inventory.ContainsKey(_primaryResourceType))
            {
                var quantity = inventory[_primaryResourceType].Current;
                _resourceQuantityLabel.text = quantity.ToString();
            }
        }
        
        private void ClearView()
        {
            CurrentSelectedObject = null;
            
            if (_unitPanelContent != null) _unitPanelContent.style.display = DisplayStyle.None;
            if (_resourcePanelContent != null) _resourcePanelContent.style.display = DisplayStyle.None;
            
            _primaryResourceType = default; 
            
            Hide(); 
        }
    }
}