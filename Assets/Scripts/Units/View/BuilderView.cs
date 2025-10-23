using MyRTSGame.Model;

namespace Units.View
{
    public class BuilderView : UnitView
    {
        private void OnMouseDown()
        {
            unitService.HandleClick(GetComponentInParent<Builder>());
        }
    }
}