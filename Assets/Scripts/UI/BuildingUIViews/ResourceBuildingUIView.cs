using System.Collections.Generic;
using System.Linq;
using MyRTSGame.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ResourceBuildingUIView : MonoBehaviour
{
    [SerializeField] private Image resourceBuildingView;
    [SerializeField] private TextMeshProUGUI resourceBuildingName;
    [SerializeField] private GameObject outputTitlePrefab;
    [SerializeField] private GameObject columnsPrefab;
    [SerializeField] private GameObject resourceRowOutputPrefab;
    [SerializeField] private GameObject outputLayoutGrid;
    
    private List<ResourceRowOutput> _resourceRowsOutput = new List<ResourceRowOutput>();
    Dictionary<ResourceType, int> resourceQuantities = new Dictionary<ResourceType, int>();

    public void ActivateResourceBuildingView(ResourceBuilding building)
    {
        resourceBuildingView.gameObject.SetActive(true);
        resourceBuildingName.text = building.BuildingType.ToString();
        Instantiate(outputTitlePrefab, outputLayoutGrid.transform);
        Instantiate(columnsPrefab, outputLayoutGrid.transform);
        
        foreach (var res in building.InventoryWhenCompleted)
        {
            resourceQuantities[res.Key] = res.Value;
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
    
    public void UpdateResourceQuantities(ResourceBuilding resourceBuilding)
    {
        for (int i = 0; i < _resourceRowsOutput.Count; i++)
        {
            var resType = _resourceRowsOutput[i].ResourceType;
            var resValue = resourceBuilding.Inventory.FirstOrDefault(res => res.Key == resType).Value;
            _resourceRowsOutput[i].UpdateQuantity(resValue);
            
        }
    }
    
    public void DeactivateResourceBuildingView()
    {
        // Destroy all child objects (rows) of the outputLayoutGrid
        foreach (Transform child in outputLayoutGrid.transform)
        {
            Destroy(child.gameObject);
        }
        _resourceRowsOutput = new List<ResourceRowOutput>();
        resourceBuildingView.gameObject.SetActive(false);
    }
}
