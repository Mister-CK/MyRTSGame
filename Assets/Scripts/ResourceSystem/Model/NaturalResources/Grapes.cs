using Enums;
using Interface;

namespace MyRTSGame.Model.ResourceSystem.Model.NaturalResources
{
    public class Grapes: NaturalResource
    {
        protected override void Start()
        {
            GrowthRate = 0.2f;
            base.Start();
            Inventory = InventoryHelper.InitInventory(new[] {ResourceType.Wine});
            MaxQuantity = 1;
            ResourceType = ResourceType.Wine;
        }
    }
}