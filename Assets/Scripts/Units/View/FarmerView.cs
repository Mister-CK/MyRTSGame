using MyRTSGame.Model;
using Units.Model.Component;

namespace Units.View
{
    public class FarmerView: UnitView
    {
        private void OnMouseDown()
        {
            unitService.HandleClick(GetComponentInParent<FarmerComponent>());
        }
    }
}