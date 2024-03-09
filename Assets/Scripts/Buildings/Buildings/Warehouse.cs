using System;
using System.Collections;
using UnityEngine;

namespace MyRTSGame.Model
{
    public class Warehouse : SpecialBuilding
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
                BCollider.size = new Vector3(3, 3, 3);
                BCollider.center = new Vector3(1.5f, 1.5f, 1.5f);

            }
            else
            {
                var resourceQuantities = new int[_resourceTypes.Length];
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