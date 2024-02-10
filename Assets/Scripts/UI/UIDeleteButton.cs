using UnityEngine;

namespace MyRTSGame.Model
{
    public class UIDeleteButton : MonoBehaviour
    {
        [SerializeField] private GameEvent onDeleteEvent;
        public void OnButtonClick()
        {
            onDeleteEvent.Raise(null);
        }
    }
}