using System.Collections.Generic;
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
    
    private List<ResourceRowOutput> _resourceRowOutputs = new List<ResourceRowOutput>();
    public void ActivateResourceBuildingView(ResourceBuilding building)
    {
        resourceBuildingView.gameObject.SetActive(true);
        resourceBuildingName.text = building.BuildingType.ToString();
        Instantiate(outputTitlePrefab, outputLayoutGrid.transform);
        Instantiate(columnsPrefab, outputLayoutGrid.transform);

        foreach (var res in building.InventoryWhenCompleted)
        {
            var resourceRow = Instantiate(resourceRowOutputPrefab, outputLayoutGrid.transform);
            var resourceRowOutput = resourceRow.GetComponent<ResourceRowOutput>();
            resourceRow.GetComponent<ResourceRowOutput>().ResourceType.text = res.ResourceType.ToString();
            resourceRow.GetComponent<ResourceRowOutput>().Quantity.text = res.Quantity.ToString();
            _resourceRowOutputs.Add(resourceRowOutput);
        }
    }
    
    public void UpdateResourceQuantities(ResourceBuilding building)
    {
        for (int i = 0; i < _resourceRowOutputs.Count; i++)
        {
            _resourceRowOutputs[i].UpdateQuantity(building.InventoryWhenCompleted[i].Quantity);
        }
    }
    
    public void DeactivateResourceBuildingView()
    {
        // Destroy all child objects (rows) of the outputLayoutGrid
        foreach (Transform child in outputLayoutGrid.transform)
        {
            Destroy(child.gameObject);
        }
        _resourceRowOutputs = new List<ResourceRowOutput>();
        // Deactivate the view
        resourceBuildingView.gameObject.SetActive(false);
    }
}
