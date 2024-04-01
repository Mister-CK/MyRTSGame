using UnityEngine;

namespace MyRTSGame.Model
{
    public class UnitView : MonoBehaviour
    {
        public UnitController unitController;
        
        private void Start()
        {
            unitController = UnitController.Instance;
        }
    }
}