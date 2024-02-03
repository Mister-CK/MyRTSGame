using System.Collections.Generic;
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
            JobController = JobController.GetInstance();
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

        public override IEnumerable<Resource> GetRequiredResources()
        {
            return new Resource[]
            {
                new() { ResourceType = ResourceType.Lumber, Quantity = 1 },
                new() { ResourceType = ResourceType.Stone, Quantity = 1 }
            };
        }
        
        // protected override void StartResourceCreationCoroutine()
        // {
        //     StartCoroutine(CreateJobsForDeliverableResources());
        // }
        //
        // private void CreateJobsForDeliverableResources()
        // {         
        //     foreach (var resource in Inventory)
        //     {
        //         if (resource.Quantity > 0)
        //         {
        //             CreateJob(new Job { Origin = this, ResourceType = resource.ResourceType });
        //         }
        //     }
        // }
    }
}