using MyRTSGame.Model.ResourceSystem.Model.NaturalResources;

namespace MyRTSGame.Model.ResourceSystem.View
{
    public class RockView: NaturalResourceView
    {
        private void OnMouseDown()
        {
            ResourceController.HandleClick(GetComponentInParent<Rock>());
        }
    }
}