using System;
using System.Collections;
using UnityEngine;

namespace MyRTSGame.Model
{
    public class Lumberjack : Building
    {
        //Constructor
        public Lumberjack()
        {
            BuildingType = BuildingType.LumberJack;
        }
        
        public override Resource[] GetRequiredResources()
        {
            return new Resource[] { new Resource() { ResourceType = ResourceType.Wood, Quantity = 3 }, new Resource() { ResourceType = ResourceType.Stone, Quantity = 2 } };
        }
        
        protected override void Start()
        {
            State = new PlacingState(BuildingType);
            ResourceType[] resourceTypes = new ResourceType[] { ResourceType.Lumber };
            int[] resourceQuantities = new int[] { 0};
            Inventory = InitInventory(resourceTypes, resourceQuantities);
            Capacity = 5;
        }
        
        protected override void StartResourceCreationCoroutine()
        {
            StartCoroutine(CreateLumber());
        }
        
        private IEnumerator CreateLumber()
        {
            while (true)
            {
                yield return new WaitForSeconds(5);
                Resource lumberResource = Array.Find(Inventory, resource => resource.ResourceType == ResourceType.Lumber);
                if (lumberResource != null && lumberResource.Quantity < Capacity)
                {
                    AddResource(ResourceType.Lumber, 1);
                }
                CreateJob(new Job() { Destination = this, ResourceType = ResourceType.Lumber});
            }
        }
    }
}