using Domain;
using Domain.Model;
using Domain.Units.Component;

namespace Units.View
{
    public class LumberJackView : UnitView
    {
        protected override void OnMouseDown()
        {
            unitService.HandleClick(GetComponentInParent<LumberJackComponent>());
        }
    }
}