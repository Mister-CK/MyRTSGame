using TMPro;
using UnityEngine;


namespace MyRTSGame.Model
{
    public class VillagerList : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textComponent;
        private Villager[] _villagers; // populate this with your buildings

        private void Awake()
        {
            _villagers = FindObjectsOfType<Villager>();
        }

        private void Update()
        {
            UpdateList();
        }

        private void UpdateList()
        {
            string villagerList = "";
            foreach (Villager villager in _villagers)
            {
                villagerList += villager.name + "\n";
            }
            textComponent.text = villagerList;
        }

        public Villager[] GetVillagers()
        {
            return this._villagers;
        }
    }
}