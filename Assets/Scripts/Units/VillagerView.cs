using UnityEngine;

namespace MyRTSGame.Model
{
    public class VillagerView : UnitView
    {
        private void OnMouseDown()
        {
            var villager = GetComponentInParent<Villager>();
            if (villager != null) villager.HandleClick();
        }
    }
}