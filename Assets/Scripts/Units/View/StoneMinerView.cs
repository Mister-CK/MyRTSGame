using MyRTSGame.Model;

namespace Units.View
{
    public class StoneMinerView : UnitView
    {
        private void OnMouseDown()
        {
            unitService.HandleClick(GetComponentInParent<StoneMiner>());
        }
    }
}