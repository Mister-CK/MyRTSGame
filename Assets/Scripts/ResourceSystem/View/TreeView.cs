namespace ResourceSystem.View
{
    public class TreeView: NaturalResourceView
    {
        private void OnMouseDown()
        {
            resourceService.HandleClick(GetComponentInParent<Tree>());
        }
    }
}