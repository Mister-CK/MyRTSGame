using Interface;
using UnityEngine;

namespace Application.EventHandlers
{
    public class UnitEventsHandler: MonoBehaviour
    {
        [SerializeField]  private UnitService unitService;
        [SerializeField] private GameEvent onAssignJob;
        [SerializeField] private GameEvent onNewUnitEvent;
        [SerializeField] private GameEvent onUnitJobDeleted;
        [SerializeField] private GameEvent onJobRequestDenied;
        [SerializeField] private GameEvent onDeleteUnitEvent;
        [SerializeField] private GameEvent onDeleteBuildingForOccupantEvent;
        
        private void OnEnable()
        {
            onAssignJob.RegisterListener(HandleJobAssigned);
            onNewUnitEvent.RegisterListener(HandleCreateNewUnit);
            onUnitJobDeleted.RegisterListener(HandleUnitJobDeleted);
            onJobRequestDenied.RegisterListener(HandleJobRequestDenied);
            onDeleteUnitEvent.RegisterListener(HandleDeleteUnitEvent);
            onDeleteBuildingForOccupantEvent.RegisterListener(HandleOnDeleteBuildingForOccupantEvent);
        }

        private void OnDisable()
        {
            onAssignJob.UnregisterListener(HandleJobAssigned);
            onNewUnitEvent.UnregisterListener(HandleCreateNewUnit);
            onUnitJobDeleted.UnregisterListener(HandleUnitJobDeleted);
            onJobRequestDenied.UnregisterListener(HandleJobRequestDenied);
            onDeleteUnitEvent.UnregisterListener(HandleDeleteUnitEvent);
            onDeleteBuildingForOccupantEvent.UnregisterListener(HandleOnDeleteBuildingForOccupantEvent);
        }

        private void HandleCreateNewUnit(IGameEventArgs args)
        {
            if (args is not TrainingBuildingUnitTypeEventArgs trainingBuildingUnitTypeEventArgs) return;
            unitService.CreateNewUnitRequest(trainingBuildingUnitTypeEventArgs.TrainingBuilding, trainingBuildingUnitTypeEventArgs.UnitType);
        }

        private void HandleJobAssigned(IGameEventArgs args)
        {
            
            if (args is not UnitWithJobEventArgs unitWithJobEventArgs) return;
            unitService.AssignJobToUnit(unitWithJobEventArgs.Unit, unitWithJobEventArgs.Job);
        }

        private void HandleUnitJobDeleted(IGameEventArgs args)
        {
            if (args is not UnitWithJobEventArgsAndDestinationType unitWithJobEventArgsAndDestinationType) return;
            unitService.DeleteUnitJob(unitWithJobEventArgsAndDestinationType.Unit, unitWithJobEventArgsAndDestinationType.DestinationType);
        }
        
        private void HandleJobRequestDenied(IGameEventArgs args)
        {
            if (args is not UnitEventArgs unitEventArgs) return;
            unitService.DenyJobRequest(unitEventArgs.Unit);
        }
        
        private void HandleDeleteUnitEvent(IGameEventArgs args)
        {
            if (args is not UnitEventArgs unitEventArgs) return;
            unitService.DeleteUnit(unitEventArgs.Unit);
        }
        private void HandleOnDeleteBuildingForOccupantEvent(IGameEventArgs args)
        {
            if (args is not BuildingEventArgs buildingEventArgs) return;
            unitService.DeleteBuildingForOccupantEvent(buildingEventArgs.Building);
        }
    }
}
