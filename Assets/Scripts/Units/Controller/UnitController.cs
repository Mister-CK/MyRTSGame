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
        [SerializeField] private GameEvent onDeleteBuildingEvent; // not used
        [SerializeField] private GameEvent onRequestVillagerJob;
        [SerializeField] private GameEvent onVillagerJobAssigned;
        [SerializeField] private GameEvent onVillagerJobDeleted; 
        
        [SerializeField] private Villager villagerPrefab;
        [SerializeField] private Builder builderPrefab;
        [SerializeField] private UnitList unitList; // not used
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
            onNewUnitEvent.RegisterListener(HandleCreateNewUnit);
            onVillagerJobAssigned.RegisterListener(HandleVillagerJobAssigned);
            onVillagerJobDeleted.RegisterListener(HandleVillagerJobDeleted);
        }

        private void OnDisable()
        {
            onNewUnitEvent.UnregisterListener(HandleCreateNewUnit);
            onVillagerJobAssigned.UnregisterListener(HandleVillagerJobAssigned);
            onVillagerJobDeleted.UnregisterListener(HandleVillagerJobDeleted);

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

        private void HandleVillagerJobAssigned(IGameEventArgs args)
        {
            if (args is not VillagerWithJobEventArgs villagerWithJobEventArgs) return;
            
            villagerWithJobEventArgs.Villager.AcceptNewVillagerJob(villagerWithJobEventArgs.VillagerJob);
        }
        
        private void HandleVillagerJobDeleted(IGameEventArgs args)
        {
            if (args is not VillagerWithJobEventArgsAndDestinationtype villagerWithJobEventArgsAndDestinationtype) return;
            
            villagerWithJobEventArgsAndDestinationtype.Villager.UnAssignVillagerJob(villagerWithJobEventArgsAndDestinationtype.DestinationType);
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

        public void CreateVillagerJobRequest(Villager villager)
        {
            onRequestVillagerJob.Raise(new VillagerEventArgs(villager));
        }
    }
}