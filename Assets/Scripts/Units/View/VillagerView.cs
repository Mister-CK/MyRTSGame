using MyRTSGame.Model;

namespace Units.View
{
    public class VillagerView : UnitView
    {
        private void OnMouseDown()
        {
            unitService.HandleClick(GetComponentInParent<Villager>());
        }
    }
}