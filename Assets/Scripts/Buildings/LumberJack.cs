using System;
using System.Collections;
using UnityEngine;

namespace MyRTSGame.Model
{
    public class Lumberjack : Building
    {
        //Constructor
        public Lumberjack() {
            SetBuildingType(BuildingType.LumberJack);
        }
        protected override void Start()
        {
            SetBuildingType(BuildingType.LumberJack);
            state = new FoundationState(buildingType);
            ResourceType[] resourceTypes = new ResourceType[] { ResourceType.Lumber };
            int[] resourceQuantities = new int[] { 0};
            inventory = InitInventory(resourceTypes, resourceQuantities);
            _capacity = 5;
            StartCoroutine(CreateLumber());
        }

        private IEnumerator CreateLumber()
        {
            while (true)
            {
                yield return new WaitForSeconds(5);
                Resource lumberResource = Array.Find(inventory, resource => resource.ResourceType == ResourceType.Lumber);
                if (lumberResource != null && lumberResource.Quantity < _capacity)
                {
                    AddResource(ResourceType.Lumber, 1);
                }
                CreateJob(new Job() { Destination = this, ResourceType = ResourceType.Lumber});
            }
        }
    }
}