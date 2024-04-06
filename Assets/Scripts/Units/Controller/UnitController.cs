using UnityEngine;

namespace MyRTSGame.Model
{
    public class UnitController : MonoBehaviour
    {
        [SerializeField] private GameEvent onNewVillagerEvent;
        [SerializeField] private GameEvent onSelectionEvent;
        [SerializeField] private GameEvent onResourceRemovedFromBuilding;
        [SerializeField] private GameEvent onResourceAddedToBuilding;
        
        [SerializeField] private Villager villagerPrefab;
        
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
            onNewVillagerEvent.RegisterListener(HandleCreateNewVillager);
        }

        private void OnDisable()
        {
            onNewVillagerEvent.UnregisterListener(HandleCreateNewVillager);
        }
        
        private void HandleCreateNewVillager(IGameEventArgs args)
        {
            if (args is not TrainingBuildingEventArgs trainingBuildingEventArgs) return;

            var spawnPosition = trainingBuildingEventArgs.TrainingBuilding.transform.position + new Vector3(2, 0, -2); 
            Instantiate(villagerPrefab, spawnPosition, Quaternion.identity);
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
                

    }
}