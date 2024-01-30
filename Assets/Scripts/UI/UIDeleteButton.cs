using UnityEngine;
using MyRTSGame.Model;

public class UIDeleteButton : MonoBehaviour
{
    private SelectionManager _selectionManager;

    public void OnButtonClick()
    {
        _selectionManager = SelectionManager.Instance;
        var selectedObject = _selectionManager.CurrentSelectedObject;

        if (selectedObject is Building selectedBuilding)
        {
            Destroy(selectedBuilding.gameObject);
            _selectionManager.SelectObject(null);
        }
    }
}