using Buildings.Model;
using Interface;
using Units.Model.Component;
using UnityEngine;


namespace MyRTSGame.Model
{
    public class SelectionController : MonoBehaviour
    {
        [SerializeField] private GameEvent onSelectionEvent;
        [SerializeField] private GameEvent onDeselectionEvent;
        [SerializeField] private GameEvent onDeleteBuildingEvent;
        [SerializeField] private GameEvent onDeleteUnitEvent;
        [SerializeField] private GameEvent onUpdateUIViewForBuildingEvent;
        [SerializeField] private GameEvent onRemoveOccupantFromBuildingEvent;
        
        [SerializeField] private SelectionView selectionView;
        
        private void OnEnable()
        {
            onSelectionEvent.RegisterListener(SelectObject);
            onUpdateUIViewForBuildingEvent.RegisterListener(UpdateUIView);
        }

        private void OnDisable()
        {
            onSelectionEvent.UnregisterListener(SelectObject);
            onUpdateUIViewForBuildingEvent.UnregisterListener(UpdateUIView);
        }

        private void UpdateUIView(IGameEventArgs args)
        {
            if (args is not BuildingEventArgs buildingEventArgs) return;
            selectionView.UpdateView(buildingEventArgs.Building);
        }

        private void LateUpdate()
        {
            if (Input.GetMouseButtonDown(1))
            {
                onDeselectionEvent.Raise(null);
            }
        }

        private void SelectObject(IGameEventArgs args)
        {
            if (args is not SelectionEventArgs selectionEventArgs) return;

            var newObject = selectionEventArgs.SelectedObject;
            selectionView.SelectObject(newObject);
        }

        public void CreateDeleteEvent(ISelectable selectedObject)
        {
            if (selectedObject is Building selectedBuilding)
            {
                onDeleteBuildingEvent.Raise(new BuildingEventArgs(selectedBuilding));
            }
            
            if (selectedObject is UnitComponent selectedUnit)
            {
                if (selectedUnit is ResourceCollectorComponent resourceCollector)
                {
                    var building = resourceCollector.CollectorData.Building;
                    if (building != null)
                    {
                        onRemoveOccupantFromBuildingEvent.Raise(new BuildingEventArgs(building));
                    }
                }
                onDeleteUnitEvent.Raise(new UnitEventArgs(selectedUnit));
            }
            SelectObject(null);
            selectionView.ClearView();
        }
    }
}