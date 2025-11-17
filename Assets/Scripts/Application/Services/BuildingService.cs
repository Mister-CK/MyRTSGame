using Buildings.Model;
using Buildings.Model.BuildingGroups;
using Enums;
using Interface;
using JetBrains.Annotations;
using Domain.Model;
using System.Collections.Generic;
using UnityEngine;

namespace Application.Services
{
    public class BuildingService : MonoBehaviour
    {
        [SerializeField] private GameEvent onDeleteBuildingForOccupantEvent;
        [SerializeField] private GameEvent onUpdateUIViewForBuildingEvent;
        [SerializeField] private GameEvent onNewUnitEvent;
        [SerializeField] private GameEvent onNewJobNeeded;
        [SerializeField] private GameEvent onDeleteVillagerJobsEvent;
        [SerializeField] private GameEvent onDeleteBuilderJobsEvent;
        
        public void CreateJobNeededEvent(JobType jobType, Building destination, Building origin, ResourceType? resourceType, UnitType? unitType)
        {
            onNewJobNeeded.Raise(new CreateNewJobEventArgs(jobType, destination, origin, resourceType, unitType));
        }
        
        public void CreateNewUnitEvent(TrainingBuilding trainingBuilding, UnitType unitType)
        {
            onNewUnitEvent.Raise(new TrainingBuildingUnitTypeEventArgs(trainingBuilding, unitType));
        }
        
        public void CreateUpdateViewForBuildingEvent(Building building)
        {
            onUpdateUIViewForBuildingEvent.Raise(new BuildingEventArgs(building));
        }
        
        public void CreateDeleteBuildingForOccupantEvent(Building building)
        {
            onDeleteBuildingForOccupantEvent.Raise(new BuildingEventArgs(building));
        }
        
        public void CreateDeleteJobsForBuildingEvent(List<VillagerJob> villagerJobsFromThisBuilding, List<VillagerJob> villagerJobsToThisBuilding, List <BuilderJob> builderJobsForThisBuilding)
        {
            onDeleteVillagerJobsEvent.Raise(new VillagerJobListEventArgs(villagerJobsFromThisBuilding, DestinationType.Origin));
            onDeleteVillagerJobsEvent.Raise(new VillagerJobListEventArgs(villagerJobsToThisBuilding, DestinationType.Destination));
            onDeleteBuilderJobsEvent.Raise(new BuilderJobListEventArgs(builderJobsForThisBuilding));
        }
        
        public void AddResource(IDestination destination, ResourceType resourceType, int quantity)
        {
            if (destination is not Building building) return;
            building.AddResource(resourceType, quantity);
        }
        
        public void RemoveResource(IDestination destination, ResourceType resourceType, int quantity)
        {
            if (destination is not Building building) return;
            building.RemoveResource(resourceType, quantity);
        }
        
        public void AddProductionJob(WorkshopBuilding workshopBuilding, ResourceType resourceType)
        {
            workshopBuilding.AddProductionJob(resourceType);
        }
        
        public void RemoveProductionJob(WorkshopBuilding workshopBuilding, ResourceType resourceType)
        {
            workshopBuilding.RemoveProductionJob(resourceType);
        }
        
        public void AddTrainingJob(TrainingBuilding trainingBuilding, UnitType unitType)
        {
            trainingBuilding.AddTrainingJob(unitType);
        }
        
        public void RemoveTrainingJob(TrainingBuilding trainingBuilding, UnitType unitType)
        {
            trainingBuilding.RemoveTrainingJob(unitType);
        }

        public void DeleteBuilding(Building building)
        {
            building.DeleteBuilding();
        }
        
        public void RemoveOccupantFromBuilding(Building building)
        {
            building.SetOccupant(null);
        }

        public void CreateNewJob(Job job)
        {
            if (job is CollectResourceJob) return;
            if (job.Destination is not Building building) return;
            building.AddJobToDestination(job);
            if (job is VillagerJob villagerJob)
            {
                villagerJob.Origin.AddVillagerJobFromThisBuilding(villagerJob);
            }
        }
        
        public void DeleteJob([CanBeNull] Job job)
        {
            if (job?.Destination is not Building building) return;
            building.RemoveJobFromDestination(job);
            if (job is VillagerJob villagerJob)
            {
                villagerJob.Origin.RemoveVillagerJobFromThisBuilding(villagerJob);
            }
        }

        public void CompleteJob(Job job)
        {
            if (job.Destination is not Building building) return;
            if (job is LookingForBuildingJob lookingForBuildingJob)
            {
                building.SetOccupant(lookingForBuildingJob.Unit);
            }
            building.RemoveJobFromDestination(job);
            if (job is VillagerJob villagerJob)
            {
                villagerJob.Origin.RemoveVillagerJobFromThisBuilding(villagerJob);
            }
        }
    }
}