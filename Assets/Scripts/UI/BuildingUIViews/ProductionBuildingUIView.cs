using System.Collections.Generic;
using MyRTSGame.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProductionBuildingUIView : MonoBehaviour
{
    [SerializeField] private Image productionBuildingView;
    [SerializeField] private TextMeshProUGUI productionBuildingName;
    
    [SerializeField] private GameObject columnsPrefab;
    [SerializeField] private GameObject resourceRowInputPrefab;
    [SerializeField] private GameObject resourceRowOutputPrefab;

    [SerializeField] private GameObject outputLayoutGrid;
    [SerializeField] private GameObject outputTitlePrefab;

    [SerializeField] private GameObject inputLayoutGrid;
    [SerializeField] private GameObject inputTitlePrefab;

    private List<ResourceRowInput> _resourceRowsInput = new List<ResourceRowInput>();
    private List<ResourceRowOutput> _resourceRowsOutput = new List<ResourceRowOutput>();
    
    Dictionary<ResourceType, int> resourceQuantities = new Dictionary<ResourceType, int>();

    public void ActivateProductionBuildingView(ProductionBuilding building)
    {
        productionBuildingView.gameObject.SetActive(true);
        productionBuildingName.text = building.BuildingType.ToString();
        Instantiate(outputTitlePrefab, outputLayoutGrid.transform);
        Instantiate(columnsPrefab, outputLayoutGrid.transform);

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
        
        foreach (var outputType in building.OutputTypesWhenCompleted)
        {
            var resourceRow = Instantiate(resourceRowOutputPrefab, outputLayoutGrid.transform);
            var resourceRowOutput = resourceRow.GetComponent<ResourceRowOutput>();
            resourceRowOutput.ResourceType = outputType;
            resourceRowOutput.resourceTypeText.text = outputType.ToString();
            resourceRowOutput.quantity.text = resourceQuantities[outputType].ToString();
            _resourceRowsOutput.Add(resourceRowOutput);
        }
    }
    
    public void UpdateResourceQuantities(ProductionBuilding building)
    {
        var resourceRowsOutputCount = _resourceRowsOutput.Count;
        for (var i = 0; i < resourceRowsOutputCount; i++)
        {
            var resType = _resourceRowsOutput[i].ResourceType;
            foreach (var res in building.Inventory)
            {
                if (res.Key == resType)
                {
                    _resourceRowsOutput[i].UpdateQuantity(res.Value);
                    break;
                }
            }
            
            foreach (var res in building.GetOutgoingResources())
            {
                if (res.ResourceType == resType)
                {
                    _resourceRowsOutput[i].UpdateInOutGoingJobs(res.Quantity);
                    break;
                }
            }
        }
        
        var resourceRowsInputCount = _resourceRowsInput.Count;
        for (var i = 0; i < resourceRowsInputCount; i++)
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
    
    
    
    public void DeactivateProductionBuildingView()
    {
        // Destroy all child objects (rows) of the outputLayoutGrid
        foreach (Transform child in outputLayoutGrid.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in inputLayoutGrid.transform)
        {
            Destroy(child.gameObject);
        }
        _resourceRowsInput = new List<ResourceRowInput>();
        _resourceRowsOutput = new List<ResourceRowOutput>();
        productionBuildingView.gameObject.SetActive(false);
    }
}