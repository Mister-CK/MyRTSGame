using UnityEngine;

namespace MyRTSGame.Model
{
    public class UnitController : MonoBehaviour
    {
        [SerializeField] private GameEvent onNewVillagerEvent;
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
    }
}