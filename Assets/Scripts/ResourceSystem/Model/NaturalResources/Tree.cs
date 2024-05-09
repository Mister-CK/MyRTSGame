using MyRTSGame.Model;
using MyRTSGame.Model.ResourceSystem.Model;

public class Tree : NaturalResource
{

    protected override void Start()
    {
        base.Start();
        Inventory = InventoryHelper.InitInventory(new[] {ResourceType.Lumber});
        ResourceType = ResourceType.Lumber;
    }
}
