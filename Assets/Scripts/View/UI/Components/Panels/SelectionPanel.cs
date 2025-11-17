using Buildings.Model;
using Buildings.Model.BuildingGroups;
using Enums;
using Interface;
using Domain.Model.ResourceSystem.Model;
using Domain.Units.Component;
using UnityEngine;
using UnityEngine.UIElements;
using View.Components.Panels.SelectionPanelStrategies;

namespace View.Components.Panels
{
    public class SelectionPanel : HUDPanel
    {
        private ISelectable CurrentSelectedObject { get; set; }
        private ISelectablePanelStrategy _activeStrategy;

        private readonly UnitPanelStrategy _unitStrategy;
        private readonly NaturalResourcePanelStrategy _resourceStrategy;
        private readonly BuildingPanelStrategy _buildingStrategy;
        private readonly HUDController _hudController;
        public SelectionPanel(HUDController hudController, string id = "panel-selection") : base(id) 
        {
            _unitStrategy = new UnitPanelStrategy();
            _resourceStrategy = new NaturalResourcePanelStrategy();
            _buildingStrategy = new BuildingPanelStrategy();
            _hudController = hudController;
        }
        
        public override void Build(VisualElement parent)
        {
            base.Build(parent);
            
            _unitStrategy.Build(this, Root );
            _resourceStrategy.Build(this, Root);
            _buildingStrategy.Build(this, Root);
        }

        /// <summary>
        /// Selects and sets the active strategy based on the type of the selectable object.
        /// </summary>
        private void SetStrategy(ISelectable selectable)
        {
            ISelectablePanelStrategy newStrategy = selectable switch
            {
                UnitComponent => _unitStrategy,
                Building => _buildingStrategy,
                NaturalResource => _resourceStrategy,
                _ => null,
            };

            if (_activeStrategy != null && _activeStrategy != newStrategy)
            {
                _activeStrategy.ClearView();
            }

            _activeStrategy = newStrategy;
        }

        public void SetView(ISelectable selectable)
        {
            if (CurrentSelectedObject != null && CurrentSelectedObject != selectable) ClearView();
            CurrentSelectedObject = selectable;
            Show();

            SetStrategy(selectable);
            
            if (_activeStrategy != null)
            {
                _activeStrategy.SetView(selectable);
            }
            else
            {
                ClearView();
            }
        }

        public void UpdateView()
        {
            if (_activeStrategy == null || CurrentSelectedObject == null) return;
            
            if (CurrentSelectedObject is Building building && _buildingStrategy.HasStateChanged(building))
            {
                _buildingStrategy.SetView(building);
            }
            
            if (CurrentSelectedObject is NaturalResource naturalResource && _resourceStrategy.HasStateChanged(naturalResource))
            {
                _resourceStrategy.SetView(naturalResource);
            }

            _activeStrategy.UpdateView(CurrentSelectedObject);
            
        }
        
        private void ClearView()
        {
            if (_activeStrategy != null)
            {
                _activeStrategy.ClearView();
            }
            CurrentSelectedObject = null;
            _activeStrategy = null;
            
            Hide(); 
        }
        
        public void DeleteObject()
        {
            _hudController.CreateDeleteEvent(CurrentSelectedObject);
        }
        
        public void ShowUnit()
        {
            if (CurrentSelectedObject is not Building building) return;
            _hudController.ShowSelectionPanel(building.GetOccupant());
        }
        
        public void ShowBuilding()
        {
            if (CurrentSelectedObject is not ResourceCollectorComponent resourceCollectorComponent) return;
            _hudController.ShowSelectionPanel(resourceCollectorComponent.CollectorData.Building);
        }
        
        public void ReduceMethod(ResourceType resourceType)
        {
            if (CurrentSelectedObject is not WorkshopBuilding workshopBuilding) return;
            _hudController.ReduceProduction(workshopBuilding, resourceType);
        }
        
        public void AddMethod(ResourceType resourceType)
        {
            if (CurrentSelectedObject is not WorkshopBuilding workshopBuilding) return;
            _hudController.IncreaseProduction(workshopBuilding, resourceType);
        }
        
        public void RemoveTrainingJob(UnitType unitType)
        {
            if (CurrentSelectedObject is not TrainingBuilding trainingBuilding) return;
            _hudController.RemoveTrainingJob(trainingBuilding, unitType);
        }
        
        public void AddTrainigJob(UnitType unitType)
        {
            if (CurrentSelectedObject is not TrainingBuilding trainingBuilding) return;
            _hudController.AddTrainigJob(trainingBuilding, unitType);
        }
        
        
    }
}