using MyRTSGame.Model.ResourceSystem.Model.NaturalResources;

namespace MyRTSGame.Model.ResourceSystem.View
{
    public class WheatView: NaturalResourceView
    {
        private void OnMouseDown()
        {
            ResourceController.HandleClick(GetComponentInParent<Wheat>());
        }
    }
}