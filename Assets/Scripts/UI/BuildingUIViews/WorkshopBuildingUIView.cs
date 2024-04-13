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
    [SerializeField] private GameObject resourceRowInputPrefab;
    [SerializeField] private GameObject resourceRowOutputPrefab;
    [SerializeField] private GameObject resourceRowProductionPrefab;
    
    [SerializeField] private GameObject outputLayoutGrid;
    [SerializeField] private GameObject outputTitlePrefab;

    [SerializeField] private GameObject inputLayoutGrid;
    [SerializeField] private GameObject inputTitlePrefab;

    [SerializeField] private GameObject jobQueueLayoutGrid;
    [SerializeField] private GameObject jobQueueTitlePrefab;
    
    private List<ResourceRowInput> _resourceRowsInput = new List<ResourceRowInput>();
    private List<ResourceRowOutput> _resourceRowsOutput = new List<ResourceRowOutput>();
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
        var resourceRowsOutputCount = _resourceRowsOutput.Count;
        for (var i = 0; i < resourceRowsOutputCount; i++)
        {
            var resType = _resourceRowsOutput[i].ResourceType;
            foreach (var res in building.Inventory)
            {
                if (res.ResourceType == resType)
                {
                    _resourceRowsOutput[i].UpdateQuantity(res.Quantity);
                    break;
                }
            }
            
            foreach (var res in building.OutgoingResources)
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
                if (res.ResourceType == resType)
                {
                    _resourceRowsInput[i].UpdateQuantity(res.Quantity);
                    break;
                }
            }
            
            foreach (var res in building.IncomingResources)
            {
                if (res.ResourceType == resType)
                {
                    _resourceRowsInput[i].UpdateInIncomingJobs(res.Quantity);
                    break;
                }
            }
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
        _resourceRowsOutput = new List<ResourceRowOutput>();
        _jobRows = new List<ResourceRowProduction>();

        workshopBuildingView.gameObject.SetActive(false);
    }
}