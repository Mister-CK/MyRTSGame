using System.Collections;
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
            base.Start();
            
            capacityForCompletedBuilding = 999;

            JobQueue = JobQueue.GetInstance();
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
        
        public override void StartResourceCreationCoroutine()
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