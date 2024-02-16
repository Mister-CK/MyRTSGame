using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MyRTSGame.Model
{
    public class SelectionController : MonoBehaviour
    {
        [SerializeField] private GameEvent onSelectionEvent;
        [SerializeField] private GameEvent onDeselectionEvent;
        [SerializeField] private GameEvent onDeleteEvent;
        
        [SerializeField] private SelectionView selectionView;
        private ISelectable CurrentSelectedObject { get; set; }
        
        private void OnEnable()
        {
            onSelectionEvent.RegisterListener(SelectObject);
            onDeleteEvent.RegisterListener(DeleteSelectedObject);
        }

        private void OnDisable()
        {
            onSelectionEvent.UnregisterListener(SelectObject);
            onDeleteEvent.UnregisterListener(DeleteSelectedObject);
        }
        
        
        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                onDeselectionEvent.Raise(null);
                CurrentSelectedObject = null;
                selectionView.ClearView();
            }
        }

        private void SelectObject(IGameEventArgs args)
        {
            if (args is not SelectionEventArgs selectionEventArgs) return;

            var newObject = selectionEventArgs.SelectedObject;
            
            CurrentSelectedObject = newObject;
            
            selectionView.UpdateView(CurrentSelectedObject);
        }

        private void DeleteSelectedObject(IGameEventArgs args)
        {
            //TODO: delete associated jobs, fix unit behaviour.
            if (CurrentSelectedObject is Building selectedBuilding)
            {
                Destroy(selectedBuilding.gameObject);
                SelectObject(null);
            }
        }
    }
}