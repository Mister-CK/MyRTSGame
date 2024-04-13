using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] private GameObject resourceRowInputPrefab;
    [SerializeField] private GameObject inputLayoutGrid;
    
    private List<ResourceRowInput> _resourceRowsInput = new List<ResourceRowInput>();
    Dictionary<ResourceType, int> resourceQuantities = new Dictionary<ResourceType, int>();

    public void ActivateConsumptionBuildingView(ConsumptionBuilding building)
    {
        consumptionBuildingView.gameObject.SetActive(true);
        consumptionBuildingName.text = building.BuildingType.ToString();
        Instantiate(inputTitlePrefab, inputLayoutGrid.transform);
        Instantiate(columnsPrefab, inputLayoutGrid.transform);

        foreach (var res in building.InventoryWhenCompleted)
        {
            resourceQuantities[res.Key] = res.Value;
        }
        
        foreach (var inputType in building.InputTypesWhenCompleted)
        {
            var resourceRow = Instantiate(resourceRowInputPrefab, inputLayoutGrid.transform);
            var resourceRowInput = resourceRow.GetComponent<ResourceRowInput>();
            resourceRowInput.ResourceType = inputType;
            resourceRowInput.resourceTypeText.text = inputType.ToString();
            resourceRowInput.quantity.text = resourceQuantities[inputType].ToString();
            _resourceRowsInput.Add(resourceRowInput);
        }
    }
    
    public void UpdateResourceQuantities(ConsumptionBuilding building)
    {
        for (var i = 0; i < _resourceRowsInput.Count; i++)
        {
            var resType = _resourceRowsInput[i].ResourceType;
            foreach (var res in building.Inventory)
            {
                if (res.Key == resType)
                {
                    _resourceRowsInput[i].UpdateQuantity(res.Value);
                    break;
                }
            }
            
            foreach (var res in building.GetIncomingResources())
            {
                if (res.ResourceType == resType)
                {
                    _resourceRowsInput[i].UpdateInIncomingJobs(res.Quantity);
                    break;
                }
            }
        }
    }
    
    public void DeactivateConsumptionBuildingView()
    {
        // Destroy all child objects (rows) of the outputLayoutGrid
        foreach (Transform child in inputLayoutGrid.transform)
        {
            Destroy(child.gameObject);
        }
        _resourceRowsInput = new List<ResourceRowInput>();
        consumptionBuildingView.gameObject.SetActive(false);
    }
}
