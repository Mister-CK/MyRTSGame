using System.Collections;
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
            JobQueue = JobQueue.GetInstance();
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
                StartResourceCreationCoroutine();
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
        
        protected override void StartResourceCreationCoroutine()
        {
            StartCoroutine(CreateJobsForDeliverableResources(this));
        }
        
        private IEnumerator CreateJobsForDeliverableResources(Warehouse warehouse)
        {
            while (true)
            {
                yield return new WaitForSeconds(5);
                JobController.CreateJobsForBuilding(warehouse);
            }
        }
    }
}