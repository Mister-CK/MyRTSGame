using Domain.Model.ResourceSystem.Model;
using Enums;
using Interface;

public class Tree : NaturalResource
{
    protected override void Start()
    {
        GrowthRate = 0.2f;
        base.Start();
        Inventory = InventoryHelper.InitInventory(new[] {ResourceType.Lumber});
        MaxQuantity = 2;
        ResourceType = ResourceType.Lumber;
    }
}
