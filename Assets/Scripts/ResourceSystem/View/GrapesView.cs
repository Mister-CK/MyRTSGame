using MyRTSGame.Model.ResourceSystem.Model.NaturalResources;

namespace MyRTSGame.Model.ResourceSystem.View
{
    public class GrapesView: NaturalResourceView
    {
        private void OnMouseDown()
        {
            ResourceController.HandleClick(GetComponentInParent<Grapes>());
        }
    }
}