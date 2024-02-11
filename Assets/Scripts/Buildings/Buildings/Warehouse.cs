using System;
using System.Collections;
using UnityEngine;

namespace MyRTSGame.Model
{
    public class Warehouse : Building
    {
        public GameEvent onCreateJobsForWarehouse;
        private readonly ResourceType[] _resourceTypes = (ResourceType[]) Enum.GetValues(typeof(ResourceType));
        [SerializeField] private int[] startingResourceQuantities;        
        
        //Constructor
        public Warehouse()
        {
            BuildingType = BuildingType.Warehouse;
        }

        protected override void Start()
        {

            base.Start();
            capacityForCompletedBuilding = 999;
            if (BuildingList.GetFirstWareHouse())
            {
                BuildingList.SetFirstWareHouse(false);
                Inventory = InitInventory(_resourceTypes, startingResourceQuantities);
                State = new CompletedState(BuildingType);
                StartResourceCreationCoroutine();
            }
            else
            {
                var resourceQuantities = new int[_resourceTypes.Length];
                Inventory = InitInventory(_resourceTypes, resourceQuantities);
                InventoryWhenCompleted = InitInventory(_resourceTypes, resourceQuantities);
            }

            InputTypesWhenCompleted = _resourceTypes;
            HasInput = true;
        }
        
        public override void StartResourceCreationCoroutine()
        {
            StartCoroutine(CreateJobsForDeliverableResources(this));
        }
        
        private IEnumerator CreateJobsForDeliverableResources(Building building)
        {
            while (true)
            {
                yield return new WaitForSeconds(5);
                onCreateJobsForWarehouse.Raise(new BuildingEventArgs(building));
            }
        }
    }
}