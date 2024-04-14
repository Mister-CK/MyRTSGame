using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyRTSGame.Model
{
    public class Warehouse : SpecialBuilding
    {
        public GameEvent onCreateJobsForWarehouse;
        private readonly ResourceType[] _resourceTypes = (ResourceType[]) Enum.GetValues(typeof(ResourceType));
        [SerializeField] private List<ResourceType> startingResourceKeys;
        [SerializeField] private List<int> startingResourceValues;
        private Dictionary<ResourceType, int> startingResources;
        [SerializeField] private int[] startingResourceQuantities;

        //Constructor
        public Warehouse()
        {
            BuildingType = BuildingType.Warehouse;
        }

        protected override void Start()
        {
            Dictionary<ResourceType, InventoryData> startingResources = new Dictionary<ResourceType, InventoryData>();

            for (int i = 0; i < startingResourceKeys.Count; i++)
            {
                startingResources.Add(startingResourceKeys[i], new InventoryData(){Current = startingResourceValues[i], Incoming = 0, Outgoing = 0});
            }
            
            base.Start();
            capacityForCompletedBuilding = 999;
            if (BuildingList.GetFirstWareHouse())
            {
                BuildingList.SetFirstWareHouse(false);
                Inventory = InitInventory(_resourceTypes);
                foreach (var keyValuePair in startingResources)
                {
                    Inventory[keyValuePair.Key] = keyValuePair.Value;
                }
                State = new CompletedState(BuildingType);
                StartResourceCreationCoroutine();
                BCollider.size = new Vector3(3, 3, 3);
                BCollider.center = new Vector3(1.5f, 1.5f, 1.5f);

            }
            else
            {
                InventoryWhenCompleted = InitInventory(_resourceTypes);
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