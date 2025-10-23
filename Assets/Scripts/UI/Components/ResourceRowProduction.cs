using Buildings.Model.BuildingGroups;
using Enums;
using TMPro;
using UnityEngine;

namespace MyRTSGame.Model.Components
{
    public class ResourceRowProduction : MonoBehaviour
    {
        public WorkshopBuilding WorkshopBuilding { get; set; }
        public ResourceType ResourceType { get; set; }
        [SerializeField] public TextMeshProUGUI resourceTypeText;
        [SerializeField] public TextMeshProUGUI quantity;
        
        [SerializeField] private GameEvent onAddProductionJobEvent;
        [SerializeField] private GameEvent onRemoveProductionJobEvent;

        public void UpdateQuantity(int newQuantity)
        {
            quantity.text = newQuantity.ToString();
        }

        public void HandleClickAddButton()
        {   
            onAddProductionJobEvent.Raise(new WorkshopBuildingBuildingResourceTypeEventArgs(WorkshopBuilding, ResourceType));
        }

        public void HandleClickRemoveButton()
        {
            onRemoveProductionJobEvent.Raise(new WorkshopBuildingBuildingResourceTypeEventArgs(WorkshopBuilding, ResourceType));
        }
    }
}