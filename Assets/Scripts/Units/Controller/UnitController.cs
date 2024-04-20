using System;
using UnityEngine;

namespace MyRTSGame.Model
{
    public class UnitController : MonoBehaviour
    {
        [SerializeField] private GameEvent onNewUnitEvent;
        [SerializeField] private GameEvent onSelectionEvent;
        [SerializeField] private GameEvent onResourceRemovedFromBuilding;
        [SerializeField] private GameEvent onResourceAddedToBuilding;
        [SerializeField] private GameEvent onVillagerJobDeleted; 
        [SerializeField] private GameEvent onBuilderJobDeleted;
        [SerializeField] private GameEvent onRequestUnitJob;
        [SerializeField] private GameEvent onRequestConsumptionJob;
        [SerializeField] private GameEvent onAssignJob;
        [SerializeField] private GameEvent onJobRequestDenied;
        [SerializeField] private Villager villagerPrefab;
        [SerializeField] private Builder builderPrefab;
        
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
            onVillagerJobDeleted.RegisterListener(HandleVillagerJobDeleted);
            onBuilderJobDeleted.RegisterListener(HandleBuilderJobDeleted);
            onJobRequestDenied.RegisterListener(HandleJobRequestDenied);
        }

        private void OnDisable()
        {
            onAssignJob.UnregisterListener(HandleJobAssigned);
            onNewUnitEvent.UnregisterListener(HandleCreateNewUnit);
            onVillagerJobDeleted.UnregisterListener(HandleVillagerJobDeleted);
            onBuilderJobDeleted.UnregisterListener(HandleBuilderJobDeleted);
            onJobRequestDenied.UnregisterListener(HandleJobRequestDenied);
        }
        
        
        private void HandleCreateNewUnit(IGameEventArgs args)
        {
            if (args is not TrainingBuildingUnitTypeEventArgs trainingBuildingUnitTypeEventArgs) return;

            var spawnPosition = trainingBuildingUnitTypeEventArgs.TrainingBuilding.transform.position + new Vector3(2, 0, -2); 
            switch (trainingBuildingUnitTypeEventArgs.UnitType)
            {
                case UnitType.Villager:
                    Instantiate(villagerPrefab, spawnPosition, Quaternion.identity);
                    break;
                case UnitType.Builder:
                    Instantiate(builderPrefab, spawnPosition, Quaternion.identity);
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
        
        private void HandleVillagerJobDeleted(IGameEventArgs args)
        {
            if (args is not VillagerWithJobEventArgsAndDestinationtype villagerWithJobEventArgsAndDestinationtype) return;
            
            villagerWithJobEventArgsAndDestinationtype.Villager.UnAssignVillagerJob(villagerWithJobEventArgsAndDestinationtype.DestinationType);
        }
        
        private void HandleBuilderJobDeleted(IGameEventArgs args)
        {
            if (args is not BuilderWithJobEventArgs builderWithJobEventArgs) return;
            
            builderWithJobEventArgs.Builder.UnAssignBuilderJob();
        }
        
        public void HandleClick(ISelectable selectable)
        {
            onSelectionEvent.Raise(new SelectionEventArgs(selectable));
        }  
        
        public void RemoveResourceFromBuilding(Building building, ResourceType resourceType, int quantity)
        {
            onResourceRemovedFromBuilding.Raise(new BuildingResourceTypeQuantityEventArgs(building, resourceType,
                quantity));
        }

        public void AddResourceToBuilding(Building building, ResourceType resourceType, int quantity)
        {
            onResourceAddedToBuilding.Raise(new BuildingResourceTypeQuantityEventArgs(building, resourceType,
                quantity));
        }

        public void CreateUnitJobRequest(Unit unit)
        {
            unit.SetPendingJobRequest(true);
            onRequestUnitJob.Raise(new UnitEventArgs(unit));
        }

        public void CreateConsumptionJobRequest(Unit unit)
        {
            unit.SetPendingJobRequest(true);
            onRequestConsumptionJob.Raise(new UnitEventArgs(unit));
        }
        
        private void HandleJobRequestDenied(IGameEventArgs args)
        {
            if (args is not UnitEventArgs unitEventArgs) return;
            unitEventArgs.Unit.SetPendingJobRequest(false);
        }
    }
}