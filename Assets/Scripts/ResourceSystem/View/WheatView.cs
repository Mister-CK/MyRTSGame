using MyRTSGame.Model.ResourceSystem.Model.NaturalResources;

namespace ResourceSystem.View
{
    public class WheatView: NaturalResourceView
    {
        private void OnMouseDown()
        {
            resourceService.HandleClick(GetComponentInParent<Wheat>());
        }
    }
}