namespace MyRTSGame.Model.ResourceSystem.View
{
    public class TreeView: NaturalResourceView
    {
        private void OnMouseDown()
        {
            ResourceController.HandleClick(GetComponentInParent<Tree>());
        }
    }
}