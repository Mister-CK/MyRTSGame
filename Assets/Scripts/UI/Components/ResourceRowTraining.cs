using Buildings.Model.BuildingGroups;
using Enums;
using TMPro;
using UnityEngine;

namespace MyRTSGame.Model.Components
{
    public class ResourceRowTraining : MonoBehaviour
    {
        public TrainingBuilding TrainingBuilding { get; set; }
        public UnitType UnitType { get; set; }
        [SerializeField] public TextMeshProUGUI unitTypeText;
        [SerializeField] public TextMeshProUGUI quantity;
        
        [SerializeField] private GameEvent onAddTrainingJobEvent;
        [SerializeField] private GameEvent onRemoveTrainingJobEvent;

        public void UpdateQuantity(int newQuantity)
        {
            quantity.text = newQuantity.ToString();
        }

        public void HandleClickAddButton()
        {   
            onAddTrainingJobEvent.Raise(new TrainingBuildingBuildingResourceTypeEventArgs(TrainingBuilding, UnitType));
        }

        public void HandleClickRemoveButton()
        {
            onRemoveTrainingJobEvent.Raise(new TrainingBuildingBuildingResourceTypeEventArgs(TrainingBuilding, UnitType));
        }
    }
}