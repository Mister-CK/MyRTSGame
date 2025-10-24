using MyRTSGame.Model;
using Units.Model.Component;

namespace Units.View
{
    public class VillagerView : UnitView
    {
        private void OnMouseDown()
        {
            unitService.HandleClick(GetComponentInParent<VillagerComponent>());
        }
    }
}