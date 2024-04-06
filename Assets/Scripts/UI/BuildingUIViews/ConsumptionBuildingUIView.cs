using System.Collections.Generic;
using MyRTSGame.Model;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


public class ConsumptionBuildingUIView : MonoBehaviour
{
    [SerializeField] private Image consumptionBuildingView;
    [SerializeField] private TextMeshProUGUI consumptionBuildingName;
    [SerializeField] private GameObject inputTitlePrefab;
    [SerializeField] private GameObject columnsPrefab;
    [SerializeField] private GameObject resourceRowOutputPrefab;
    [SerializeField] private GameObject inputLayoutGrid;
    
    private List<ResourceRowOutput> _resourceRowOutputs = new List<ResourceRowOutput>();
    public void ActivateConsumptionBuildingView(ConsumptionBuilding building)
    {
        consumptionBuildingView.gameObject.SetActive(true);
        consumptionBuildingName.text = building.BuildingType.ToString();
        Instantiate(inputTitlePrefab, inputLayoutGrid.transform);
        Instantiate(columnsPrefab, inputLayoutGrid.transform);

        foreach (var res in building.InventoryWhenCompleted)
        {
            var resourceRow = Instantiate(resourceRowOutputPrefab, inputLayoutGrid.transform);
            var resourceRowOutput = resourceRow.GetComponent<ResourceRowOutput>();
            resourceRow.GetComponent<ResourceRowOutput>().ResourceType.text = res.ResourceType.ToString();
            resourceRow.GetComponent<ResourceRowOutput>().Quantity.text = res.Quantity.ToString();
            _resourceRowOutputs.Add(resourceRowOutput);
        }
    }
    
    public void UpdateResourceQuantities(ConsumptionBuilding building)
    {
        for (int i = 0; i < _resourceRowOutputs.Count; i++)
        {
            _resourceRowOutputs[i].UpdateQuantity(building.InventoryWhenCompleted[i].Quantity);
        }
    }
    
    public void DeactivateConsumptionBuildingView()
    {
        // Destroy all child objects (rows) of the outputLayoutGrid
        foreach (Transform child in inputLayoutGrid.transform)
        {
            Destroy(child.gameObject);
        }
        _resourceRowOutputs = new List<ResourceRowOutput>();
        consumptionBuildingView.gameObject.SetActive(false);
    }
}
