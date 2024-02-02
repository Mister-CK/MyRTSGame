using UnityEngine;

namespace MyRTSGame.Model
{
    public class Warehouse : Building
    {
        [SerializeField] private int[] startingResourceQuantities = { 0, 0, 0 };
        //Constructor
        public Warehouse()
        {
            BuildingType = BuildingType.Warehouse;
        }

        protected override void Start()
        {
            BuildingList = BuildingList.Instance; 
            SelectionManager = SelectionManager.Instance;            
            State = new PlacingState(BuildingType);
            var resourceTypes = new[] { ResourceType.Stone, ResourceType.Lumber, ResourceType.Wood };
            if (BuildingList.GetFirstWareHouse())
            {
                BuildingList.SetFirstWareHouse(false);
                Inventory = InitInventory(resourceTypes, startingResourceQuantities);
                State = new CompletedState(BuildingType);
            }
            else
            {
                var resourceQuantities = new[] { 0, 0, 0 };
                Inventory = InitInventory(resourceTypes, resourceQuantities);
                InventoryWhenCompleted = InitInventory(resourceTypes, resourceQuantities);
            }

            InputTypesWhenCompleted = resourceTypes;
            HasInput = true;
        }

        public override Resource[] GetRequiredResources()
        {
            return new Resource[]
            {
                new() { ResourceType = ResourceType.Lumber, Quantity = 1 },
                new() { ResourceType = ResourceType.Stone, Quantity = 0 }
            };
        }
    }
}