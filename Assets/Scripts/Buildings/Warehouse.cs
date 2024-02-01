using UnityEngine;

namespace MyRTSGame.Model
{

    public class Warehouse : Building
    {
        [SerializeField] private int[] startingResourceQuantities = new int[] { 0, 0, 0 };
        //Constructor
        public Warehouse() {
            BuildingType = BuildingType.Warehouse;
        }
        
        public override Resource[] GetRequiredResources()
        {
            return new Resource[] { new() { ResourceType = ResourceType.Lumber, Quantity = 1}, new() { ResourceType = ResourceType.Stone, Quantity = 0 } };
        }

        protected override void Start()
        {
            State = new PlacingState(BuildingType);
            var resourceTypes = new ResourceType[] { ResourceType.Stone, ResourceType.Lumber, ResourceType.Wood };
            if (startingResourceQuantities is { Length: > 0 })
            {
                Inventory = InitInventory(resourceTypes, startingResourceQuantities);
                State = new CompletedState(BuildingType);

            }
            else
            {
                var resourceQuantities = new int[] { 0, 0, 0};
                Inventory = InitInventory(resourceTypes, resourceQuantities);
            } 

            InputTypesWhenCompleted = resourceTypes;
            HasInput = true;
        }
    }
}