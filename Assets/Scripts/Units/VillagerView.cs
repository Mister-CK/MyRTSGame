using UnityEngine;

namespace MyRTSGame.Model
{
    public class VillagerView : MonoBehaviour
    {
        [SerializeField] private UnitController unitController;
        private void OnMouseDown()
        {
            unitController.HandleClick(GetComponentInParent<Villager>());
        }
    }
}