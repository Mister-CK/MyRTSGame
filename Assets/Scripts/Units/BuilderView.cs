using UnityEngine;

namespace MyRTSGame.Model
{
    public class BuilderView : MonoBehaviour
    {
        private void OnMouseDown()
        {
            var builder = GetComponentInParent<Builder>();
            if (builder != null) builder.HandleClick();
        }
    }
}