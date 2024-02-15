namespace MyRTSGame.Model
{
    public class BuilderView : UnitView
    {
        private void OnMouseDown()
        {
            unitController.HandleClick(GetComponentInParent<Builder>());
        }
    }
}