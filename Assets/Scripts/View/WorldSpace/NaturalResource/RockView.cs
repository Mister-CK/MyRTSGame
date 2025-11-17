using MyRTSGame.Model.ResourceSystem.Model.NaturalResources;

namespace ResourceSystem.View
{
    public class RockView: NaturalResourceView
    {
        private void OnMouseDown()
        {
            resourceService.HandleClick(GetComponentInParent<Rock>());
        }
    }
}