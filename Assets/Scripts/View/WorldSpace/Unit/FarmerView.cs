using MyRTSGame.Model;
using Units.Model.Component;

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