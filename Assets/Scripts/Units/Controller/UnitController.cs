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
        
        
        private void OnEnable()
        {
            onNewVillagerEvent.RegisterListener(HandleNewJobNeeded);
        }

        private void OnDisable()
        {
            onNewVillagerEvent.UnregisterListener(HandleNewJobNeeded);
        }
        
        private void HandleNewJobNeeded(IGameEventArgs args)
        {
            Instantiate(villagerPrefab, new Vector3(-10,0,-10), Quaternion.identity);
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