using Buildings.Model;
using Buildings.Model.BuildingStates;
using Interface;
using System;
using UnityEngine.UIElements;
using View.Components.Inventory;
using View.Extensions;

namespace View.Components.Panels.SelectionPanelStrategies
{
    public class BuildingPanelStrategy : ISelectablePanelStrategy
    {
        // --- CACHED BUILDING UI ELEMENTS ---
        private VisualElement _contentRoot;
        private Label _buildingNameLabel;
        private VisualElement _statusBarContainer; 
        private Slider _progressSlider;
        private VisualElement _dynamicContentContainer; 

        private Type _lastKnownStateType;

        private ResourceTableComponent _inputTable; 
        private ResourceTableComponent _outputTable; 
        
        public void Build(VisualElement rootContainer)
        {
            _contentRoot = rootContainer.CreateChild("building-panel-content");
            
            _buildingNameLabel = _contentRoot.CreateChild<Label>("building-name-label");
            _buildingNameLabel.AddToClassList("selection-panel-header");

            _statusBarContainer = _contentRoot.CreateChild("building-status-bar-container");
            _statusBarContainer.style.flexDirection = FlexDirection.Column;

            _progressSlider = new Slider(0, 100, SliderDirection.Horizontal, 1f);
            _progressSlider.name = "progress-slider";
            _progressSlider.label = "Progress";
            _progressSlider.SetEnabled(false);
            
            _statusBarContainer.Add(_progressSlider);
            _statusBarContainer.style.display = DisplayStyle.None;

            _inputTable = _contentRoot.CreateChild<ResourceTableComponent>("resource-table");
            _inputTable.SetHeaderText("input");
            _inputTable.style.display = DisplayStyle.None;
            
            _outputTable = _contentRoot.CreateChild<ResourceTableComponent>("resource-table");
            _outputTable.SetHeaderText("output");
            _outputTable.style.display = DisplayStyle.None;
                
            _dynamicContentContainer = _contentRoot.CreateChild("building-dynamic-content");
            _dynamicContentContainer.style.flexGrow = 1;
            _contentRoot.style.display = DisplayStyle.None;
        }

        public void SetView(ISelectable selectable)
        {
            if (selectable is not Building building) return;

            _contentRoot.style.display = DisplayStyle.Flex;

            _buildingNameLabel.text = building.GetBuildingType().ToString();
            
            _dynamicContentContainer.Clear(); 
            
            var state = building.GetState();
            _lastKnownStateType = state.GetType(); 
            
            if (state is FoundationState)
            {
                _inputTable.style.display = DisplayStyle.Flex;
            }

            else if (state is ConstructionState)
            {
                //Hide foundation style
                _inputTable.style.display = DisplayStyle.None;
                
                _statusBarContainer.style.display = DisplayStyle.Flex;
                _progressSlider.label = "Construction Progress";
            }

            else if (state is CompletedState)
            {
                _statusBarContainer.style.display = DisplayStyle.None;
                if (building.HasInput())
                {
                    _inputTable.style.display = DisplayStyle.Flex;
                }
                
                if (building.HasOuput())
                {
                    _outputTable.style.display = DisplayStyle.Flex;
                }
                
            }
            
            UpdateView(building);
        }

        public void UpdateView(ISelectable selectable)
        {
            if (selectable is not Building building) return;
            if (HasStateChanged(building))
            {
                SetView(building);
                return;
            }
            
            var state = building.GetState();
            
            if (state is FoundationState foundationState)
            {
                _inputTable.SetInput(building.GetInventory(), building.InputTypes);
                return;
            }
            if (state is ConstructionState constructionState)
            {
                _progressSlider.value = constructionState.GetPercentageCompleted();
                return;
            }
            
            if (state is CompletedState completedState)
            {
                if (building.HasInput()) _inputTable.SetInput(building.GetInventory(), building.InputTypesWhenCompleted);
                if (building.HasOuput()) _outputTable.SetOutput(building.GetInventory(), building.OutputTypesWhenCompleted);

                return;
            }
        }

        /// <summary>
        /// Checks if the Building's state (Foundation, Construction, Completed) has changed.
        /// </summary>
        /// <param name="building">The currently selected building.</param>
        /// <returns>True if the state type has changed.</returns>
        public bool HasStateChanged(Building building)
        {
            // This is the core logic that triggers the full SetView reload in the main panel.
            var currentState = building.GetState().GetType();
            return currentState != _lastKnownStateType;
        }

        public void ClearView()
        {
            if (_contentRoot != null) _contentRoot.style.display = DisplayStyle.None;
            _lastKnownStateType = null;
            _dynamicContentContainer?.Clear();
        }
    }
}
