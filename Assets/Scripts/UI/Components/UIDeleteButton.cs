using UnityEngine;

namespace UI.Components
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