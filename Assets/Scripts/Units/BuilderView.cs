using UnityEngine;

namespace MyRTSGame.Model
{
    public class BuilderView : MonoBehaviour
    {
        [SerializeField] private UnitController unitController;

        private void OnMouseDown()
        {
            unitController.HandleClick(GetComponentInParent<Builder>());
        }
    }
}