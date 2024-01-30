using System;
using System.Collections;
using Unity.VisualScripting;
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
        
        public override Resource[] GetRequiredResources()
        {
            return new Resource[] { new Resource() { ResourceType = ResourceType.Wood, Quantity = 3 }, new Resource() { ResourceType = ResourceType.Stone, Quantity = 2 } };
        }
        
        protected override void Start()
        {
            State = new PlacingState(BuildingType);
            ResourceType[] resourceTypes = new ResourceType[] { ResourceType.Stone };
            int[] resourceQuantities = new int[] { 0 };
            Inventory = InitInventory(resourceTypes, resourceQuantities);
            Capacity = 5;
            InputTypes = new ResourceType[0];
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
                Resource lumberResource = Array.Find(Inventory, resource => resource.ResourceType == ResourceType.Stone);
                if (lumberResource != null && lumberResource.Quantity < Capacity)
                {
                    AddResource(ResourceType.Stone, 1);
                }
                CreateJob(new Job() { Destination = this, ResourceType = ResourceType.Stone });
            }
        }
    }
}
