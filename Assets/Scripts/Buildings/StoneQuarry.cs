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
            capacityForCompletedBuilding = 5;
            resourceCountNeededForConstruction = 3;
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

        public override void StartResourceCreationCoroutine()
        {
            StartCoroutine(BuildingController.CreateResource(15, ResourceType.Stone));
        }
    }
}