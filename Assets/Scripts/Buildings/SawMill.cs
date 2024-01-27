using System;
using System.Collections;
using UnityEngine;

namespace MyRTSGame.Model
{
    public class SawMill : Building
    {
        //Constructor
        public SawMill() {
            SetBuildingType(BuildingType.SawMill);
        }
        protected override void Start()
        {
            SetBuildingType(BuildingType.SawMill);
            state = new FoundationState(buildingType);
            ResourceType[] resourceTypes = new ResourceType[] {ResourceType.Lumber, ResourceType.Wood };
            int[] resourceQuantities = new int[] {0, 0};
            inventory = InitInventory(resourceTypes, resourceQuantities);
            _inputTypes = new ResourceType[] { ResourceType.Lumber };
            _capacity = 5;

            StartCoroutine(CreateWoodFromLumber());
        }

        private IEnumerator CreateWoodFromLumber()
        {
            Resource[] input = new Resource[] { new Resource() { ResourceType = ResourceType.Lumber, Quantity = 1 } };
            Resource[] output = new Resource[] { new Resource() { ResourceType = ResourceType.Wood, Quantity = 1 } };
            
            while (true)
            {
                yield return new WaitForSeconds(10);
                bool hasRequiredResources = true;
                bool isNotFull = Array.Find(inventory, res => res.ResourceType == ResourceType.Wood).Quantity < _capacity;
                foreach(Resource resource in input) {
                    if (Array.Find(inventory, res => res.ResourceType == resource.ResourceType).Quantity < resource.Quantity) {
                        hasRequiredResources = false;
                    }
                }
                if (hasRequiredResources && isNotFull) {
                    TransmuteResource(input, output);
                    CreateJob(new Job() { Destination = this , ResourceType = ResourceType.Wood });
                }
            }
        }

    }
}