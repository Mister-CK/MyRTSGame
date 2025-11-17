using Interface;
using Domain.Units.Component;
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

        private SelectionPanel _selectionPanel;
        
        public void Build(SelectionPanel selectionPanel, VisualElement rootContainer)
        {
            // Note: The UI element creation is now self-contained here.
            _contentRoot = rootContainer.CreateChild("unit-panel-content");
            _selectionPanel = selectionPanel;
            
            var topLeftContainer = _contentRoot.CreateChild("panel-top-left-container");
            _buildingButton = topLeftContainer.CreateChild<Button>("building-button");
            _buildingButton.text = "Building";
            _buildingButton.style.display = DisplayStyle.None;
            
            var topMiddleContainer = _contentRoot.CreateChild("panel-top-middle-container");
            _unitPanelHeader = topMiddleContainer.CreateChild<Label>("selection-panel-header"); 
            
            var topRightContainer = _contentRoot.CreateChild("panel-top-right-container");
            _deleteButton = topRightContainer.CreateChild<Button>("delete-button");
            _deleteButton.text = "Delete";
            _deleteButton.clicked += () =>
            {
                _selectionPanel.DeleteObject();
            }; 
           
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
            if (unit is ResourceCollectorComponent resourceCollectorComponent)
            {
                if (resourceCollectorComponent.CollectorData.Building)
                {
                    _buildingButton.style.display = DisplayStyle.Flex;
                    _buildingButton.clicked += () =>
                    {
                        _selectionPanel.ShowBuilding();
                    };
                }
            }
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