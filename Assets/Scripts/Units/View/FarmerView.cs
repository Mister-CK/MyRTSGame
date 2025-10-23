using MyRTSGame.Model;

namespace Units.View
{
    public class FarmerView: UnitView
    {
        private void OnMouseDown()
        {
            unitService.HandleClick(GetComponentInParent<Farmer>());
        }
    }
}