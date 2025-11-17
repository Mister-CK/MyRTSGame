using Enums;
using Interface;

namespace MyRTSGame.Model.ResourceSystem.Model.NaturalResources
{
    public class Wheat : NaturalResource
    {
        protected override void Start()
        {
            GrowthRate = .2f;
            base.Start();
            Inventory = InventoryHelper.InitInventory(new[] {ResourceType.Wheat});
            MaxQuantity = 1;
            ResourceType = ResourceType.Wheat;
        }
    }
}