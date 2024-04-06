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

        ResourceType[] resourceTypes = { ResourceType.Fish};
        var resourceQuantities = new int[resourceTypes.Length];
        InventoryWhenCompleted = InitInventory(resourceTypes, resourceQuantities);
        OutputTypesWhenCompleted = new[] { ResourceType.Fish };
    }

    public override void StartResourceCreationCoroutine()
    {
        StartCoroutine(CreateResource(15, ResourceType.Fish));
    }
    }
}