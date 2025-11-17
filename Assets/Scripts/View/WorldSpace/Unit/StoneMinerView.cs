using Domain.Model;
using Domain.Units.Component;

namespace Units.View
{
    public class StoneMinerView : UnitView
    {
        protected override void OnMouseDown()
        {
            unitService.HandleClick(GetComponentInParent<StoneMinerComponent>());
        }
    }
}