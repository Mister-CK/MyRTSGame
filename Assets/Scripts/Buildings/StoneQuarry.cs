using System;
using System.Collections;
using UnityEngine;

namespace MyRTSGame.Model
{
    public class StoneQuarry : Building
    {
        //Constructor
        public StoneQuarry() {
            SetBuildingType(BuildingType.StoneQuarry);
        }
        protected override void Start()
        {
            SetBuildingType(BuildingType.StoneQuarry);
            state = new FoundationState(buildingType);
            ResourceType[] resourceTypes = new ResourceType[] { ResourceType.Stone };
            int[] resourceQuantities = new int[] { 0 };
            inventory = InitInventory(resourceTypes, resourceQuantities);
            _capacity = 5;
            _inputTypes = new ResourceType[0];
            StartCoroutine(CreateStone());
        }

        private IEnumerator CreateStone()
        {
            while (true)
            {
                yield return new WaitForSeconds(15);
                Resource lumberResource = Array.Find(inventory, resource => resource.ResourceType == ResourceType.Stone);
                if (lumberResource != null && lumberResource.Quantity < _capacity)
                {
                    AddResource(ResourceType.Stone, 1);
                }
                CreateJob(new Job() { Destination = this, ResourceType = ResourceType.Stone });
            }
        }

        // Update is called once per frame
        void Update()
        {
        }

        
    }
}
