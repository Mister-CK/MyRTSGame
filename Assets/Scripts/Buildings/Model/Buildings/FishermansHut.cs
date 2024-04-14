namespace MyRTSGame.Model
{
    public class FisherMansHut : ResourceBuilding
    {
    //Constructor
    public FisherMansHut()
    {
        BuildingType = BuildingType.FisherMansHut;
    }

    protected override void Start()
    {
        base.Start();

        OutputTypesWhenCompleted = new[] { ResourceType.Fish };
    }

    public override void StartResourceCreationCoroutine()
    {
        StartCoroutine(CreateResource(15, ResourceType.Fish));
    }
    }
}