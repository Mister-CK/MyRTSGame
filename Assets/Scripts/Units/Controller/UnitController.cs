using System;
using UnityEngine;

namespace MyRTSGame.Model
{
    public class UnitController : MonoBehaviour
    {
        [SerializeField] private GameEvent onNewUnitEvent;
        [SerializeField] private GameEvent onSelectionEvent;
        [SerializeField] private GameEvent onResourceRemovedFromDestination;
        [SerializeField] private GameEvent onResourceAddedToBuilding;
        [SerializeField] private GameEvent onUnitJobDeleted;
        [SerializeField] private GameEvent onRequestUnitJob;
        [SerializeField] private GameEvent onAssignJob;
        [SerializeField] private GameEvent onJobRequestDenied;
        [SerializeField] private GameEvent onNewJobNeeded;
        [SerializeField] private Villager villagerPrefab;
        [SerializeField] private Builder builderPrefab;
        [SerializeField] private LumberJack lumberJackPrefab;
        public static UnitController Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        private void OnEnable()
        {
            onAssignJob.RegisterListener(HandleJobAssigned);
            onNewUnitEvent.RegisterListener(HandleCreateNewUnit);
            onUnitJobDeleted.RegisterListener(HandleUnitJobDeleted);
            onJobRequestDenied.RegisterListener(HandleJobRequestDenied);
        }

        private void OnDisable()
        {
            onAssignJob.UnregisterListener(HandleJobAssigned);
            onNewUnitEvent.UnregisterListener(HandleCreateNewUnit);
            onUnitJobDeleted.UnregisterListener(HandleUnitJobDeleted);
            onJobRequestDenied.UnregisterListener(HandleJobRequestDenied);
        }


        private void HandleCreateNewUnit(IGameEventArgs args)
        {
            if (args is not TrainingBuildingUnitTypeEventArgs trainingBuildingUnitTypeEventArgs) return;

            var spawnPosition = trainingBuildingUnitTypeEventArgs.TrainingBuilding.transform.position +
                                new Vector3(2, 0, -2);
            switch (trainingBuildingUnitTypeEventArgs.UnitType)
            {
                case UnitType.Villager:
                    Instantiate(villagerPrefab, spawnPosition, Quaternion.identity);
                    break;
                case UnitType.Builder:
                    Instantiate(builderPrefab, spawnPosition, Quaternion.identity);
                    break;
                case UnitType.LumberJack:
                    Instantiate(lumberJackPrefab, spawnPosition, Quaternion.identity);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(trainingBuildingUnitTypeEventArgs.UnitType.ToString());
            }
        }

        private void HandleJobAssigned(IGameEventArgs args)
        {
            if (args is not UnitWithJobEventArgs unitWithJobEventArgs) return;
            unitWithJobEventArgs.Unit.AcceptNewJob(unitWithJobEventArgs.Job);
        }

        private void HandleUnitJobDeleted(IGameEventArgs args)
        {
            if (args is not UnitWithJobEventArgsAndDestinationType unitWithJobEventArgsAndDestinationType) return;
            if (unitWithJobEventArgsAndDestinationType.Unit is Builder builder)
            {
                builder.UnAssignBuilderJob();
            }

            if (unitWithJobEventArgsAndDestinationType.Unit is Villager villager)
            {
                villager.UnAssignVillagerJob(unitWithJobEventArgsAndDestinationType.DestinationType
                    .GetValueOrDefault());
            }
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

        public void CreateUnitJobRequest(Unit unit, JobType jobType)
        {
            unit.SetPendingJobRequest(true);
            onRequestUnitJob.Raise(new UnitWithJobTypeEventArgs(unit, jobType));
        }

        private void HandleJobRequestDenied(IGameEventArgs args)
        {
            if (args is not UnitEventArgs unitEventArgs) return;
            unitEventArgs.Unit.SetPendingJobRequest(false);
        }

        public void CreateNewLookForBuildingJob(Unit unit)
        {
            unit.SetPendingJobRequest(true);
            onRequestUnitJob.Raise(new UnitWithJobTypeEventArgs(unit, JobType.LookForBuildingJob));
        }

        public void CreateJobNeededEvent(JobType jobType, Building destination, Building origin,
            ResourceType? resourceType, UnitType? unitType)
        {
            onNewJobNeeded.Raise(new CreateNewJobEventArgs(jobType, destination, origin, resourceType, unitType));
        }
    }
}