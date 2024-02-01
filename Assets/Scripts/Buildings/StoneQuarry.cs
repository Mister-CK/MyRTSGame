using System;
using System.Collections;
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
            _buildingList = BuildingList.Instance; 
            _selectionManager = SelectionManager.Instance;
            State = new PlacingState(BuildingType);
            ResourceType[] resourceTypes = { ResourceType.Stone };
            int[] resourceQuantities = { 0 };
            Inventory = InitInventory(resourceTypes, resourceQuantities);
            Capacity = 5;
            InputTypes = new ResourceType[0];
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
            StartCoroutine(CreateStone());
        }

        private IEnumerator CreateStone()
        {
            while (true)
            {
                yield return new WaitForSeconds(15);
                var lumberResource = Array.Find(Inventory, resource => resource.ResourceType == ResourceType.Stone);
                if (lumberResource != null && lumberResource.Quantity < Capacity) AddResource(ResourceType.Stone, 1);
                CreateJob(new Job { Origin = this, ResourceType = ResourceType.Stone });
            }
        }
    }
}