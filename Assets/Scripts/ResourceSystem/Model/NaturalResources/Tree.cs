using MyRTSGame.Model;
using MyRTSGame.Model.ResourceSystem.Model;

public class Tree : NaturalResource
{

    protected override void Start()
    {
        base.Start();
        Resource = new Resource(){ResourceType = ResourceType.Lumber, Quantity = 1};
        ResourceController.CreateAddResourceJobsEvent(this);
    }
}
