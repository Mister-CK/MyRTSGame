using Domain.Model;
using Domain.Units.Component;

namespace Units.View
{
    public class VillagerView : UnitView
    {
        protected override void OnMouseDown()
        {
            unitService.HandleClick(GetComponentInParent<VillagerComponent>());
        }
    }
}