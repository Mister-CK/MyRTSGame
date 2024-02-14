using UnityEngine;

namespace MyRTSGame.Model
{
    public class UINewVillagerButtonHandler : MonoBehaviour
    {
        [SerializeField] private GameEvent onNewVillagerEvent;
        public void OnButtonClick()
        {
            onNewVillagerEvent.Raise(null);
        }
    }
}