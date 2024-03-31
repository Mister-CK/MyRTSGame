using System;
using System.Collections;
using UnityEngine;

namespace MyRTSGame.Model
{
    public abstract class ResourceBuilding : Building
    {
        public IEnumerator CreateResource(int timeInSeconds, ResourceType resourceType)
        {
            while (true)
            {
                yield return new WaitForSeconds(timeInSeconds);
                var resToCreate = Array.Find(Inventory, resource => resource.ResourceType == resourceType);
                if (resToCreate != null && resToCreate.Quantity < Capacity) AddResource(resourceType, 1);
                
                buildingController.CreateVillagerJobNeededEvent(this, resourceType);
            }
        }
    }
}