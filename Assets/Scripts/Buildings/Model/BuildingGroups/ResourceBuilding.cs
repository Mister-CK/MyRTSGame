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
                if (Inventory[resourceType].Current < Capacity)
                {
                    AddResource(resourceType, 1);
                    buildingController.CreateJobNeededEvent(JobType.VillagerJob, null, this, resourceType);
                }
            }
        }
    }
}