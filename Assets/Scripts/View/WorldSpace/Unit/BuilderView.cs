using Domain;
using Domain.Model;
using Domain.Units.Component;

namespace Units.View
{
    public class BuilderView : UnitView
    {
        protected override void OnMouseDown()
        {
            unitService.HandleClick(GetComponentInParent<BuilderComponent>());
        }
    }
}