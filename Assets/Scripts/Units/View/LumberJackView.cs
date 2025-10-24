using MyRTSGame.Model;
using Units.Model.Component;

namespace Units.View
{
    public class LumberJackView : UnitView
    {
        private void OnMouseDown()
        {
            unitService.HandleClick(GetComponentInParent<LumberJackComponent>());
        }
    }
}