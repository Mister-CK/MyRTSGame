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
    [SerializeField] private GameObject resourceRowPrefab;
    
    [SerializeField] private GameObject outputLayoutGrid;
    [SerializeField] private GameObject outputTitlePrefab;

    [SerializeField] private GameObject inputLayoutGrid;
    [SerializeField] private GameObject inputTitlePrefab;

    
    private List<ResourceRowOutput> _resourceRows = new List<ResourceRowOutput>();
    
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
            resourceQuantities[res.ResourceType] = res.Quantity;
        }
        
        foreach (var inputType in building.InputTypesWhenCompleted)
        {
            var resourceRow = Instantiate(resourceRowPrefab, inputLayoutGrid.transform);
            var resourceRowInput = resourceRow.GetComponent<ResourceRowOutput>();
            resourceRowInput.ResourceType.text = inputType.ToString();
            resourceRowInput.Quantity.text = resourceQuantities[inputType].ToString();
            _resourceRows.Add(resourceRowInput);
        }
        
        foreach (var outputType in building.OutputTypesWhenCompleted)
        {
            var resourceRow = Instantiate(resourceRowPrefab, outputLayoutGrid.transform);
            var resourceRowOutput = resourceRow.GetComponent<ResourceRowOutput>();
            resourceRowOutput.ResourceType.text = outputType.ToString();
            resourceRowOutput.Quantity.text = resourceQuantities[outputType].ToString();
            _resourceRows.Add(resourceRowOutput);
        }
    }
    
    public void UpdateResourceQuantities(ProductionBuilding building)
    {
        for (int i = 0; i < _resourceRows.Count; i++)
        {
            _resourceRows[i].UpdateQuantity(building.InventoryWhenCompleted[i].Quantity);
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
        _resourceRows = new List<ResourceRowOutput>();
        productionBuildingView.gameObject.SetActive(false);
    }
}