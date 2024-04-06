using UnityEngine;


namespace MyRTSGame.Model
{
    public class SelectionController : MonoBehaviour
    {
        [SerializeField] private GameEvent onSelectionEvent;
        [SerializeField] private GameEvent onDeselectionEvent;
        [SerializeField] private GameEvent onDeleteEvent;
        [SerializeField] private GameEvent onUpdateUIViewForBuildingEvent;
         
        [SerializeField] private SelectionView selectionView;
        private ISelectable CurrentSelectedObject { get; set; }
        
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
            if (CurrentSelectedObject is Building selectedBuilding)
            {
                selectionView.SetView(selectedBuilding);
            }
        }
        
        private void LateUpdate()
        {
            if (Input.GetMouseButtonDown(1))
            {
                onDeselectionEvent.Raise(null);
                if (CurrentSelectedObject != null)
                {
                    selectionView.ClearView();
                    CurrentSelectedObject = null;
                }
            }

            selectionView.UpdateView(CurrentSelectedObject);
        }

        private void SelectObject(IGameEventArgs args)
        {
            if (args is not SelectionEventArgs selectionEventArgs) return;

            var newObject = selectionEventArgs.SelectedObject;

            if (CurrentSelectedObject != null && CurrentSelectedObject != newObject)
            {
                selectionView.ClearView();
            }

            CurrentSelectedObject = newObject;

            selectionView.SetView(CurrentSelectedObject);
        }

        public void CreateDeleteEvent()
        {
            onDeleteEvent.Raise(new SelectionEventArgs(CurrentSelectedObject));
            if (CurrentSelectedObject is Building selectedBuilding)
            {
                Destroy(selectedBuilding.gameObject);
                SelectObject(null);
            }
        }
    }
}