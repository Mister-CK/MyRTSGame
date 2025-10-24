using MyRTSGame.Model;
using Units.Model.Component;

namespace Units.View
{
    public class BuilderView : UnitView
    {
        private void OnMouseDown()
        {
            unitService.HandleClick(GetComponentInParent<BuilderComponent>());
        }
    }
}