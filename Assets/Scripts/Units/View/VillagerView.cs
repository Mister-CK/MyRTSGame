using MyRTSGame.Model;

namespace Units.View
{
    public class VillagerView : UnitView
    {
        private void OnMouseDown()
        {
            unitController.HandleClick(GetComponentInParent<Villager>());
        }
    }
}