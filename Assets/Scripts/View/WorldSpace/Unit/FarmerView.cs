using Domain.Model;
using Domain.Units.Component;

namespace Units.View
{
    public class FarmerView: UnitView
    {
        protected override void OnMouseDown()
        {
            unitService.HandleClick(GetComponentInParent<FarmerComponent>());
        }
    }
}