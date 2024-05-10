using MyRTSGame.Model.Terrains.Model.Terrains;

namespace MyRTSGame.Model.ResourceSystem.Model.NaturalResources
{
    public class Wheat : NaturalResource
    {
        private Farmland _farmland;
        protected override void Start()
        {
            GrowthRate = .2f;
            base.Start();
            Inventory = InventoryHelper.InitInventory(new[] {ResourceType.Wheat});
            MaxQuantity = 1;
            ResourceType = ResourceType.Wheat;
        }
        
        public void SetFarmland(Farmland farmland)
        {
            _farmland = farmland;
        }
        
        public Farmland GetFarmland()
        {
            return _farmland;
        }
    }
}