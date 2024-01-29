using TMPro;
using UnityEngine;


namespace MyRTSGame.Model
{
    public class VillagerList : MonoBehaviour
    {
        private Villager[] _villagers;

        private void Awake()
        {
            _villagers = FindObjectsOfType<Villager>();
        }

        public Villager[] GetVillagers()
        {
            return this._villagers;
        }
    }
}