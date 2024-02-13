namespace MyRTSGame.Model
{
    public class FisherMansHut : Building
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
    }

    public override void StartResourceCreationCoroutine()
    {
        StartCoroutine(buildingController.CreateResource(this, 15, ResourceType.Fish));
    }
    }
}