using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyRTSGame.Model
{
    public class StoneQuarry : Building
    {
        //Constructor
        public StoneQuarry()
        {
            BuildingType = BuildingType.StoneQuarry;
        }

        protected override void Start()
        {
            JobController = JobController.GetInstance();
            BuildingList = BuildingList.Instance; 
            SelectionManager = SelectionManager.Instance;
            State = new PlacingState(BuildingType);
            ResourceType[] resourceTypes = { ResourceType.Stone };
            int[] resourceQuantities = { 0 };
            Inventory = InitInventory(resourceTypes, resourceQuantities);
            InventoryWhenCompleted = InitInventory(resourceTypes, resourceQuantities);

            Capacity = 5;
        }

        public override IEnumerable<Resource> GetRequiredResources()
        {
            return new Resource[]
            {
                new() { ResourceType = ResourceType.Wood, Quantity = 3 },
                new() { ResourceType = ResourceType.Stone, Quantity = 3 }
            };
        }

        protected override void StartResourceCreationCoroutine()
        {
            StartCoroutine(CreateStone());
        }

        private IEnumerator CreateStone()
        {
            while (true)
            {
                yield return new WaitForSeconds(15);
                var lumberResource = Array.Find(Inventory, resource => resource.ResourceType == ResourceType.Stone);
                if (lumberResource != null && lumberResource.Quantity < Capacity) AddResource(ResourceType.Stone, 1);
                JobController.CreateJob(new Job { Origin = this, ResourceType = ResourceType.Stone });
            }
        }
    }
}