namespace MyRTSGame.Model.ResourceSystem.Model.NaturalResources
{
    public class Rock : NaturalResource
    {
        protected override void Start()
        {
            base.Start();
            Inventory = InventoryHelper.InitInventory(new[] {ResourceType.Stone});
            MaxQuantity = 100;
            ResourceType = ResourceType.Stone;
        }
    }
}