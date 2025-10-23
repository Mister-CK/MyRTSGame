using MyRTSGame.Model;

namespace Units.View
{
    public class LumberJackView : UnitView
    {
        private void OnMouseDown()
        {
            unitService.HandleClick(GetComponentInParent<LumberJack>());
        }
    }
}