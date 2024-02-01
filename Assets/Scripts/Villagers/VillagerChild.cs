using UnityEngine;

namespace MyRTSGame.Model
{
    public class VillagerChild : MonoBehaviour
    {
        private void OnMouseDown()
        {
            var villager = GetComponentInParent<Villager>();
            if (villager != null) villager.HandleClick();
        }
    }
}