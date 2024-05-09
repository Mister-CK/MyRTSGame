namespace MyRTSGame.Model.ResourceSystem.Model.NaturalResources
{
    public class Rock : NaturalResource
    {
        protected override void Start()
        {
            GrowthRate = 100f;
            base.Start();
            Inventory = InventoryHelper.InitInventory(new[] {ResourceType.Stone});
            MaxQuantity = 100;
            ResourceType = ResourceType.Stone;

        }
    }
}