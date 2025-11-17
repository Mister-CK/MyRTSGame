using Domain.Model.ResourceSystem.Model.NaturalResources;

namespace ResourceSystem.View
{
    public class GrapesView: NaturalResourceView
    {
        private void OnMouseDown()
        {
            resourceService.HandleClick(GetComponentInParent<Grapes>());
        }
    }
}