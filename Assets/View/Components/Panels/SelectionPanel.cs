using Buildings.Model;
using Interface;
using MyRTSGame.Model.ResourceSystem.Model;
using Units.Model.Component;
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

        public SelectionPanel(string id = "panel-selection") : base(id) 
        {
            _unitStrategy = new UnitPanelStrategy();
            _resourceStrategy = new NaturalResourcePanelStrategy();
            _buildingStrategy = new BuildingPanelStrategy();
        }
        
        public override void Build(VisualElement parent)
        {
            base.Build(parent);
            
            _unitStrategy.Build(Root);
            _resourceStrategy.Build(Root);
            _buildingStrategy.Build(Root);
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
        
        public void ClearView()
        {
            if (_activeStrategy != null)
            {
                _activeStrategy.ClearView();
            }
            CurrentSelectedObject = null;
            _activeStrategy = null;
            
            Hide(); 
        }
    }
}