using MyRTSGame.Model;

namespace Units.View
{
    public class StoneMinerView : UnitView
    {
        private void OnMouseDown()
        {
            unitController.HandleClick(GetComponentInParent<StoneMiner>());
        }
    }
}