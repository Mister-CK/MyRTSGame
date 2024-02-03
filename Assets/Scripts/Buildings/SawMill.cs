using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyRTSGame.Model
{
    public class SawMill : Building
    {
        //Constructor
        public SawMill()
        {
            BuildingType = BuildingType.SawMill;
        }

        protected override void Start()
        {            
            capacityForCompletedBuilding = 5;
            resourceCountNeededForConstruction = 3;
            JobController = JobController.GetInstance();
            BuildingList = BuildingList.Instance; 
            SelectionManager = SelectionManager.Instance;
            State = new PlacingState(BuildingType);
            ResourceType[] resourceTypes = { ResourceType.Lumber, ResourceType.Wood };
            int[] resourceQuantities = { 0, 0 };
            Inventory = InitInventory(resourceTypes, resourceQuantities);
            InventoryWhenCompleted = InitInventory(resourceTypes, resourceQuantities);
            InputTypesWhenCompleted = new[] { ResourceType.Lumber };
            Capacity = 5;
            HasInput = true;
        }
        
        public override void StartResourceCreationCoroutine()
        {
            StartCoroutine(CreateWoodFromLumber());
        }

        private IEnumerator CreateWoodFromLumber()
        {
            Resource[] input = { new() { ResourceType = ResourceType.Lumber, Quantity = 1 } };
            Resource[] output = { new() { ResourceType = ResourceType.Wood, Quantity = 1 } };

            while (true)
            {
                yield return new WaitForSeconds(10);
                var hasRequiredResources = true;
                var isNotFull = Array.Find(Inventory, res => res.ResourceType == ResourceType.Wood).Quantity < Capacity;
                foreach (var resource in input)
                    if (Array.Find(Inventory, res => res.ResourceType == resource.ResourceType).Quantity <
                        resource.Quantity)
                        hasRequiredResources = false;
                if (hasRequiredResources && isNotFull)
                {
                    TransmuteResource(input, output);
                    JobController.CreateJob(new Job { Origin = this, ResourceType = ResourceType.Wood });
                }
            }
        }
    }
}