using MyRTSGame.Model;
using MyRTSGame.Model.ResourceSystem.Model;

public class Tree : NaturalResource
{

    public void Awake()
    {
        Resource = new Resource(){ResourceType = ResourceType.Wood, Quantity = 50};
        ResourceController.CreateAddResourceJobsEvent(this);
    }
}
