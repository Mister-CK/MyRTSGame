using System;
using System.Collections;
using System.Collections.Generic;
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
            capacityForCompletedBuilding = 5;
            resourceCountNeededForConstruction = 3;
            JobController = JobController.GetInstance();
            BuildingList = BuildingList.Instance;
            SelectionManager = SelectionManager.Instance;
            State = new PlacingState(BuildingType);
            var resourceTypes = new[] { ResourceType.Lumber };
            var resourceQuantities = new[] { 0 };
            Inventory = InitInventory(resourceTypes, resourceQuantities);
            InventoryWhenCompleted = InitInventory(resourceTypes, resourceQuantities);
            Capacity = 5;
        }
        
        public override void StartResourceCreationCoroutine()
        {
            StartCoroutine(BuildingController.CreateResource(5, ResourceType.Lumber));
        }
    }
}