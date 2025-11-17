using MyRTSGame.Model;
using Units.Model.Component;

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