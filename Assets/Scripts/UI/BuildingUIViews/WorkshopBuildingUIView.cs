using System.Collections.Generic;
using MyRTSGame.Model;
using MyRTSGame.Model.Components;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WorkshopBuildingUIView : MonoBehaviour
{
    [SerializeField] private Image workshopBuildingView;
    [SerializeField] private TextMeshProUGUI workshopBuildingName;
    
    [SerializeField] private GameObject columnsPrefab;
    [SerializeField] private GameObject resourceRowPrefab;
    [SerializeField] private GameObject resourceRowProductionPrefab;
    
    [SerializeField] private GameObject outputLayoutGrid;
    [SerializeField] private GameObject outputTitlePrefab;

    [SerializeField] private GameObject inputLayoutGrid;
    [SerializeField] private GameObject inputTitlePrefab;

    [SerializeField] private GameObject jobQueueLayoutGrid;
    [SerializeField] private GameObject jobQueueTitlePrefab;
    
    private List<ResourceRowOutput> _resourceRows = new List<ResourceRowOutput>();
    private List<ResourceRowProduction> _jobRows = new List<ResourceRowProduction>();

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
            var resourceRow = Instantiate(resourceRowProductionPrefab, jobQueueLayoutGrid.transform);
            var resourceRowJobQueue = resourceRow.GetComponent<ResourceRowProduction>();
            resourceRowJobQueue.ResourceType = outputType;
            resourceRowJobQueue.WorkshopBuilding = building;
            resourceRowJobQueue.resourceTypeText.text = outputType.ToString();
            resourceRowJobQueue.quantity.text = resourceQuantities[outputType].ToString();
            _jobRows.Add(resourceRowJobQueue);
        }
    }
    
    public void UpdateResourceQuantities(WorkshopBuilding building)
    {
        var resourceRowsCount = _resourceRows.Count;
        for (var i = 0; i < resourceRowsCount; i++)
        {
            _resourceRows[i].UpdateQuantity(building.InventoryWhenCompleted[i].Quantity); // this is matched on index, which is not very safe since it relies on the order of the array.
        }
        
        var jobRowsCount = _jobRows.Count;
        for (var i = 0; i < jobRowsCount; i++)
        {
            var resType = _jobRows[i].ResourceType;
            _jobRows[i].UpdateQuantity(building.ProductionJobs.Find(el => el.Output.ResourceType == resType).Quantity);
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
        _jobRows = new List<ResourceRowProduction>();

        workshopBuildingView.gameObject.SetActive(false);
    }
}