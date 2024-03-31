using System.Collections.Generic;
using System.Linq;
using MyRTSGame.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WorkshopBuildingUIView : MonoBehaviour
{
    [SerializeField] private Image workshopBuildingView;
    [SerializeField] private TextMeshProUGUI workshopBuildingName;
    
    [SerializeField] private GameObject columnsPrefab;
    [SerializeField] private GameObject resourceRowPrefab;
    
    [SerializeField] private GameObject outputLayoutGrid;
    [SerializeField] private GameObject outputTitlePrefab;

    [SerializeField] private GameObject inputLayoutGrid;
    [SerializeField] private GameObject inputTitlePrefab;

    [SerializeField] private GameObject jobQueueLayoutGrid;
    [SerializeField] private GameObject jobQueueTitlePrefab;
    
    private List<ResourceRowOutput> _resourceRows = new List<ResourceRowOutput>();
    private List<ResourceRowOutput> _jobQueueRows = new List<ResourceRowOutput>();

    Dictionary<ResourceType, int> resourceQuantities = new Dictionary<ResourceType, int>();

    public void ActivateWorkshopBuildingView(WorkshopBuilding building)
    {
        workshopBuildingView.gameObject.SetActive(true);
        workshopBuildingName.text = building.BuildingType.ToString();

        Instantiate(inputTitlePrefab, inputLayoutGrid.transform);
        Instantiate(columnsPrefab, inputLayoutGrid.transform);

        Instantiate(outputTitlePrefab, outputLayoutGrid.transform);
        Instantiate(columnsPrefab, outputLayoutGrid.transform);
        
        Instantiate(jobQueueTitlePrefab, jobQueueLayoutGrid.transform);
        Instantiate(columnsPrefab, jobQueueLayoutGrid.transform);
        
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
        
        foreach (var outputType in building.OutputTypesWhenCompleted)
        {
            var resourceRow = Instantiate(resourceRowPrefab, jobQueueLayoutGrid.transform);
            var resourceRowJobQueue = resourceRow.GetComponent<ResourceRowOutput>();
            resourceRowJobQueue.ResourceType.text = outputType.ToString();
            resourceRowJobQueue.Quantity.text = resourceQuantities[outputType].ToString();
            _jobQueueRows.Add(resourceRowJobQueue);
        }
    }
    
    public void UpdateResourceQuantities(WorkshopBuilding building)
    {
        var resourceRowsCount = _resourceRows.Count;
        for (int i = 0; i < resourceRowsCount; i++)
        {
            _resourceRows[i].UpdateQuantity(building.InventoryWhenCompleted[i].Quantity);
        }
    }
    
    public void DeactivateWorkshopBuildingView()
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
        foreach (Transform child in jobQueueLayoutGrid.transform)
        {
            Destroy(child.gameObject);
        }
        _resourceRows = new List<ResourceRowOutput>();
        workshopBuildingView.gameObject.SetActive(false);
    }
}