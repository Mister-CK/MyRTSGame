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
         
        [SerializeField] private SelectionView selectionView;
        
        private void OnEnable()
        {
            onSelectionEvent.RegisterListener(SelectObject);
            onUpdateUIViewForBuildingEvent.RegisterListener(SetUIView);
        }

        private void OnDisable()
        {
            onSelectionEvent.UnregisterListener(SelectObject);
            onUpdateUIViewForBuildingEvent.UnregisterListener(SetUIView);
        }

        private void SetUIView(IGameEventArgs args)
        {
            if (args is not BuildingEventArgs buildingEventArgs) return;
            selectionView.UpdateView(buildingEventArgs.Building);
        }
        
        public void DeSelectObject()
        {
            onDeselectionEvent.Raise(null);
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
            
            if (selectedObject is Unit selectedUnit)
            {
                onDeleteUnitEvent.Raise(new UnitEventArgs(selectedUnit));
            }
            SelectObject(null);
            selectionView.ClearView();
        }
    }
}