using Buildings.Model;
using Buildings.Model.BuildingGroups;
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
        private VisualElement _headerContainer; 
        private Button _leftButton;  
        private Label _buildingNameLabel;
        private Button _rightButton;
        
        private VisualElement _statusBarContainer; 
        private Slider _progressSlider;
        private VisualElement _dynamicContentContainer; 

        private Type _lastKnownStateType;

        private ResourceTableComponent _inputTable; 
        private ResourceTableComponent _outputTable; 
        private ProductionTableComponent _productionTable; 
        private TrainingTableComponent _trainingTable;
        
        private SelectionPanel _selectionPanel;

        public void Build(SelectionPanel selectionPanel, VisualElement rootContainer)
        {
            _contentRoot = rootContainer.CreateChild("building-panel-content");
            _selectionPanel = selectionPanel;
            
            _headerContainer = _contentRoot.CreateChild("building-header-container");
            _headerContainer.style.flexDirection = FlexDirection.Row;
            _headerContainer.style.alignItems = Align.Center;
            _headerContainer.style.marginBottom = 10;
            
            _leftButton = _headerContainer.CreateChild<Button>("building-left-button");
            _leftButton.text = "Unit";
            _leftButton.style.width = 30;
            _leftButton.style.marginRight = 10;
            _leftButton.style.display = DisplayStyle.None; 
            
            _buildingNameLabel = _headerContainer.CreateChild<Label>("building-name-label");
            _buildingNameLabel.AddToClassList("selection-panel-header");
            
            _rightButton = _headerContainer.CreateChild<Button>("building-right-button");
            _rightButton.text = "Delete";
            _rightButton.style.width = 30;
            _rightButton.style.marginLeft = 10;
            _rightButton.style.display = DisplayStyle.None;
            _rightButton.clicked += () =>
            {
                _selectionPanel.DeleteObject();
            };
            
            _statusBarContainer = _contentRoot.CreateChild("building-status-bar-container");
            _statusBarContainer.style.flexDirection = FlexDirection.Column;

            _progressSlider = new Slider(0, 100, SliderDirection.Horizontal, 1f);
            _progressSlider.name = "progress-slider";
            _progressSlider.label = "Progress";
            _progressSlider.SetEnabled(false);
            
            _statusBarContainer.Add(_progressSlider);
            _statusBarContainer.style.display = DisplayStyle.None;

            _inputTable = _contentRoot.CreateChild<ResourceTableComponent>("resource-table");
            _inputTable.SetHeaderText("Input");
            _inputTable.style.display = DisplayStyle.None;
            _inputTable.SetTargetLabel("Incomming");
            
            _outputTable = _contentRoot.CreateChild<ResourceTableComponent>("resource-table");
            _outputTable.SetHeaderText("Output");
            _outputTable.style.display = DisplayStyle.None;
            _outputTable.SetTargetLabel("OutGoing");
            
            _productionTable = _contentRoot.CreateChild<ProductionTableComponent>("production-table");
            _productionTable.SetHeaderText("Production");
            _productionTable.style.display = DisplayStyle.None;
                
            _trainingTable = _contentRoot.CreateChild<TrainingTableComponent>("training-table");
            _trainingTable.SetHeaderText("Training");
            _trainingTable.style.display = DisplayStyle.None;
            
            _dynamicContentContainer = _contentRoot.CreateChild("building-dynamic-content");
            _dynamicContentContainer.style.flexGrow = 1;
            _contentRoot.style.display = DisplayStyle.None;
        }

        public void SetView(ISelectable selectable)
        {
            if (selectable is not Building building) return;

            _contentRoot.style.display = DisplayStyle.Flex;

            _buildingNameLabel.text = building.GetBuildingType().ToString();
            
            if (building.GetOccupant() != null)
            {
                _leftButton.style.display = DisplayStyle.Flex;
                _leftButton.clicked += () =>
                {
                    _selectionPanel.ShowUnit();
                };
            }
            _rightButton.style.display = DisplayStyle.Flex;

            _dynamicContentContainer.Clear(); 
            
            var state = building.GetState();
            _lastKnownStateType = state.GetType(); 
            
            if (state is FoundationState)
            {
                _inputTable.style.display = DisplayStyle.Flex;
                _inputTable.SetInput(building.GetInventory(), building.InputTypes);
                
                _outputTable.style.display = DisplayStyle.None;
                _productionTable.style.display = DisplayStyle.None;
                _trainingTable.style.display = DisplayStyle.None;
            }

            else if (state is ConstructionState)
            {
                //Hide foundation style
                _inputTable.style.display = DisplayStyle.None;
                _outputTable.style.display = DisplayStyle.None;
                _productionTable.style.display = DisplayStyle.None;
                _trainingTable.style.display = DisplayStyle.None;

                _statusBarContainer.style.display = DisplayStyle.Flex;
                _progressSlider.label = "Construction Progress";
            }

            else if (state is CompletedState)
            {
                //Hide construction style
                _statusBarContainer.style.display = DisplayStyle.None;

                if (building.HasInput())
                {
                    _inputTable.style.display = DisplayStyle.Flex;
                    _inputTable.SetInput(building.GetInventory(), building.InputTypesWhenCompleted);
                }
                else _inputTable.style.display = DisplayStyle.None;

                if (building.HasOuput())
                {
                    _outputTable.style.display = DisplayStyle.Flex;
                    _outputTable.SetOutput(building.GetInventory(), building.OutputTypesWhenCompleted);
                }
                else _outputTable.style.display = DisplayStyle.None;

                if (building is WorkshopBuilding)
                {
                    _productionTable.style.display = DisplayStyle.Flex;
                    _productionTable.SetProduction(_selectionPanel, building.GetInventory(), building.OutputTypesWhenCompleted);
                }
                else _productionTable.style.display = DisplayStyle.None;

                if (building is TrainingBuilding trainingBuilding)
                {
                    _trainingTable.style.display = DisplayStyle.Flex;
                    _trainingTable.SetProduction(_selectionPanel, trainingBuilding);
                }
                else _trainingTable.style.display = DisplayStyle.None;

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
            if (building.GetOccupant() != null) _leftButton.style.display = DisplayStyle.Flex;

            if (state is FoundationState foundationState)
            {
                _inputTable.UpdateInputData(building.GetInventory());
                return;
            }
            if (state is ConstructionState constructionState)
            {
                _progressSlider.value = constructionState.GetPercentageCompleted();
                return;
            }
            
            if (state is CompletedState completedState)
            {
                if (building.HasInput()) _inputTable.UpdateInputData(building.GetInventory());
                if (building.HasOuput()) _outputTable.UpdateOutputData(building.GetInventory());
                if (building is WorkshopBuilding workshopBuilding) _productionTable.UpdateProduction(building.GetInventory(), workshopBuilding);
                if (building is TrainingBuilding trainingBuilding) _trainingTable.UpdateProduction(building.GetInventory(), trainingBuilding);
                return;
            }
        }
        
        public bool HasStateChanged(Building building)
        {
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
