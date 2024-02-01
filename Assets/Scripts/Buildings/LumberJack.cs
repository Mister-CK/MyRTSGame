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

        protected override void Start()
        {
            State = new PlacingState(BuildingType);
            var resourceTypes = new[] { ResourceType.Lumber };
            var resourceQuantities = new[] { 0 };
            Inventory = InitInventory(resourceTypes, resourceQuantities);
            Capacity = 5;
        }

        public override Resource[] GetRequiredResources()
        {
            return new Resource[]
            {
                new() { ResourceType = ResourceType.Wood, Quantity = 3 },
                new() { ResourceType = ResourceType.Stone, Quantity = 2 }
            };
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
                var lumberResource = Array.Find(Inventory, resource => resource.ResourceType == ResourceType.Lumber);
                if (lumberResource != null && lumberResource.Quantity < Capacity) AddResource(ResourceType.Lumber, 1);
                CreateJob(new Job { Origin = this, ResourceType = ResourceType.Lumber });
            }
        }
    }
}