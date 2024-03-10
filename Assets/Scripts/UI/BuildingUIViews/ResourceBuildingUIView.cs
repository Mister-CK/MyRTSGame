using MyRTSGame.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ResourceBuildingUIView : MonoBehaviour
{
    [SerializeField] private Image resourceBuildingView;
    [SerializeField] private TextMeshProUGUI resourceBuildingName;
    [SerializeField] private TextMeshProUGUI outputResource1;
    [SerializeField] private TextMeshProUGUI outputResource1Quantity;
    
    public  void ActivateResourceBuildingView(ResourceBuilding building)
    {
        resourceBuildingView.gameObject.SetActive(true);
        resourceBuildingName.text = building.BuildingType.ToString();
        outputResource1.text = building.InventoryWhenCompleted[0].ResourceType + ": ";
        outputResource1Quantity.text = building.InventoryWhenCompleted[0].Quantity.ToString();
    }
}
