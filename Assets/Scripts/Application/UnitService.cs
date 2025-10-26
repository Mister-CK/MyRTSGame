using Application.Factories;
using Buildings.Model;
using Enums;
using Interface;
using MyRTSGame.Model;
using Units.Model.Component;
using UnityEngine;

namespace Application
{
    public class UnitService : MonoBehaviour
    {
        [SerializeField] private UnitFactory unitFactory;

        [SerializeField] private GameEvent onSelectionEvent;
        [SerializeField] private GameEvent onResourceRemovedFromDestination;
        [SerializeField] private GameEvent onResourceAddedToBuilding;
        [SerializeField] private GameEvent onRequestUnitJob;
        [SerializeField] private GameEvent onNewJobNeeded;
        [SerializeField] private GameEvent onCompleteJobEvent;
        [SerializeField] private GameEvent onAbortJobEvent;
        [SerializeField] private GameEvent onPlantResourceEvent;

        
        public void CreateNewUnitRequest(Building trainingBuilding,  UnitType unitType)
        {
            unitFactory.CreateNewUnit(trainingBuilding, unitType);
        }

        public void AssignJobToUnit(UnitComponent unit, Job job)
        {
            unit.AcceptNewJob(job);
        }

        public void DeleteUnitJob(UnitComponent unitComponent, DestinationType? destinationType)
        {
            unitComponent.UnAssignJob(destinationType.GetValueOrDefault());
        }
        
        public void DenyJobRequest(UnitComponent unitComponent)
        {
            unitComponent.Data.SetPendingJobRequest(false);
        }
        
        public void DeleteUnit(UnitComponent unitComponent)
        {
            unitComponent.DeleteUnit();
        }
        
        public void DeleteBuildingForOccupantEvent(Building building)
        {
            var occupant = building.GetOccupant();
            onAbortJobEvent.Raise(new JobEventArgs(occupant.Data.CurrentJob));
            if (occupant is not ResourceCollectorComponent resourceCollector) return;
            resourceCollector.BuildingDeleted();
        }

        public void HandleClick(ISelectable selectable)
        {
            onSelectionEvent.Raise(new SelectionEventArgs(selectable));
        }

        public void RemoveResourceFromDestination(IDestination destination, ResourceType resourceType, int quantity)
        {
            onResourceRemovedFromDestination.Raise(new DestinationResourceTypeQuantityEventArgs(destination,
                resourceType,
                quantity));
        }

        public void AddResourceToDestination(IDestination destination, ResourceType resourceType, int quantity)
        {
            onResourceAddedToBuilding.Raise(new DestinationResourceTypeQuantityEventArgs(destination, resourceType,
                quantity));
        }

        public void CreateUnitJobRequest(UnitComponent unit, JobType jobType)
        {
            unit.Data.SetPendingJobRequest(true);
            onRequestUnitJob.Raise(new UnitWithJobTypeEventArgs(unit, jobType));
        }

        public void CreateNewLookForBuildingJob(UnitComponent unit)
        {
            unit.Data.SetPendingJobRequest(true);
            onRequestUnitJob.Raise(new UnitWithJobTypeEventArgs(unit, JobType.LookForBuildingJob));
        }

        public void CreateJobNeededEvent(JobType jobType, Building destination, Building origin,
            ResourceType? resourceType, UnitType? unitType)
        {
            onNewJobNeeded.Raise(new CreateNewJobEventArgs(jobType, destination, origin, resourceType, unitType));
        }

        public void CompleteJob(Job job)
        {
            onCompleteJobEvent.Raise(new JobEventArgs(job));
        }

        public void CreatePlantResourceEvent(Job job)
        {
            onPlantResourceEvent.Raise(new JobEventArgs(job));
        }
    }
}