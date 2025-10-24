using MyRTSGame.Model;
using Units.Model.Component;

namespace Units.View
{
    public class StoneMinerView : UnitView
    {
        private void OnMouseDown()
        {
            unitService.HandleClick(GetComponentInParent<StoneMinerComponent>());
        }
    }
}