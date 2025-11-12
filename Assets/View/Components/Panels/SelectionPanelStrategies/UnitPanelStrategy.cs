using Interface;
using Units.Model.Component;
using UnityEngine;
using UnityEngine.UIElements;
using View.Extensions;

namespace View.Components.Panels.SelectionPanelStrategies
{
    public class UnitPanelStrategy : ISelectablePanelStrategy
    {
        // --- CACHED UNIT UI ELEMENTS ---
        private VisualElement _contentRoot;
        private Label _unitPanelHeader;
        private Button _buildingButton;
        private Button _deleteButton;
        private Slider _staminaSlider; 
        
        public void Build(VisualElement rootContainer)
        {
            // Note: The UI element creation is now self-contained here.
            _contentRoot = rootContainer.CreateChild("unit-panel-content");
            
            var topLeftContainer = _contentRoot.CreateChild("panel-top-left-container");
            _buildingButton = topLeftContainer.CreateChild<Button>("building-button");
            _buildingButton.text = "Building";
            // Placeholder debug logs for button clicks
            _buildingButton.clicked += () => { Debug.Log("Building button clicked (Unit)"); }; 
            
            var topMiddleContainer = _contentRoot.CreateChild("panel-top-middle-container");
            _unitPanelHeader = topMiddleContainer.CreateChild<Label>("selection-panel-header"); 
            
            var topRightContainer = _contentRoot.CreateChild("panel-top-right-container");
            _deleteButton = topRightContainer.CreateChild<Button>("delete-button");
            _deleteButton.text = "Delete";
            _deleteButton.clicked += () => { Debug.Log("Delete button clicked (Unit)"); }; 
           
            _contentRoot.CreateChild("panel-bottom-container");
            
            _staminaSlider = new Slider(0, 100, SliderDirection.Horizontal, 1f);
            _staminaSlider.name = "stamina-slider";
            _staminaSlider.label = "Stamina";
            _staminaSlider.SetEnabled(false); 
            _contentRoot.Add(_staminaSlider);
            
            _contentRoot.style.display = DisplayStyle.None;
        }

        public void SetView(ISelectable selectable)
        {
            if (selectable is not UnitComponent unit) return;

            _contentRoot.style.display = DisplayStyle.Flex;
            _unitPanelHeader.text = unit.name;
            
            UpdateView(unit);
        }

        public void UpdateView(ISelectable selectable)
        {
            if (selectable is not UnitComponent unit) return;
            
            _staminaSlider.value = unit.Data.GetStamina(); 
        }

        public void ClearView()
        {
            if (_contentRoot != null) _contentRoot.style.display = DisplayStyle.None;
        }
    }
}