using System.Collections.Generic;
using System.Linq;
using MyRTSGame.Model;
using MyRTSGame.Model.Components;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CompletedStateBuildingUIView : MonoBehaviour
{
    [SerializeField] private Image buildingView;
    [SerializeField] private TextMeshProUGUI buildingName;
    [SerializeField] private GameObject columnsPrefab;
    
    [SerializeField] private GameObject inputLayoutGrid;
    [SerializeField] private GameObject inputTitlePrefab;
    [SerializeField] private GameObject resourceRowInputPrefab;
    
    [SerializeField] private GameObject outputLayoutGrid;
    [SerializeField] private GameObject outputTitlePrefab;
    [SerializeField] private GameObject resourceRowOutputPrefab;
    
    [SerializeField] private GameObject trainingJobLayoutGrid;
    [SerializeField] private GameObject trainingJobTitlePrefab;
    [SerializeField] private GameObject resourceRowTrainingPrefab;
    
    [SerializeField] private GameObject jobQueueLayoutGrid;
    [SerializeField] private GameObject jobQueueTitlePrefab;
    [SerializeField] private GameObject resourceRowProductionPrefab;
    
    private readonly Dictionary<ResourceType, int> _resourceQuantities = new Dictionary<ResourceType, int>();
    
    private List<ResourceRowInput> _resourceRowsInput = new ();
    private List<ResourceRowOutput> _resourceRowsOutput = new ();
    private List<ResourceRowProduction> _resourceRowProduction = new ();
    private List<ResourceRowTraining> _resourceRowsTraining = new ();

    public void ActivateBuildingView(Building building)
    {
        buildingView.gameObject.SetActive(true);
        buildingName.text = building.BuildingType.ToString();
        
        foreach (var res in building.Inventory)
        {
            _resourceQuantities[res.Key] = res.Value.Current;
        }
        
        if (building.InputTypesWhenCompleted.Length > 0)
        {
            Instantiate(inputTitlePrefab, inputLayoutGrid.transform);
            Instantiate(columnsPrefab, inputLayoutGrid.transform);
            foreach (var inputType in building.InputTypesWhenCompleted)
            {
                var resourceRow = Instantiate(resourceRowInputPrefab, inputLayoutGrid.transform);
                var resourceRowInput = resourceRow.GetComponent<ResourceRowInput>();
                resourceRowInput.ResourceType = inputType;
                resourceRowInput.resourceTypeText.text = inputType.ToString();
                resourceRowInput.quantity.text = _resourceQuantities[inputType].ToString();
                _resourceRowsInput.Add(resourceRowInput);
            }
        }
        
        if (building.OutputTypesWhenCompleted.Length > 0)
        {
            Instantiate(outputTitlePrefab, outputLayoutGrid.transform);
            Instantiate(columnsPrefab, outputLayoutGrid.transform);
            foreach (var outputType in building.OutputTypesWhenCompleted)
            {
                var resourceRow = Instantiate(resourceRowOutputPrefab, outputLayoutGrid.transform);
                var resourceRowOutput = resourceRow.GetComponent<ResourceRowOutput>();
                resourceRowOutput.ResourceType = outputType;
                resourceRowOutput.resourceTypeText.text = outputType.ToString();
                resourceRowOutput.quantity.text = _resourceQuantities[outputType].ToString();
                _resourceRowsOutput.Add(resourceRowOutput);
            }
        }
        
        if (building is TrainingBuilding trainingBuilding)
        {
            Instantiate(trainingJobTitlePrefab, trainingJobLayoutGrid.transform);
            Instantiate(columnsPrefab, trainingJobLayoutGrid.transform);
            foreach (var trainingJob in trainingBuilding.TrainingJobs)
            {
                var resourceRow = Instantiate(resourceRowTrainingPrefab, trainingJobLayoutGrid.transform);
                var resourceRowJobQueue = resourceRow.GetComponent<ResourceRowTraining>();
                resourceRowJobQueue.UnitType = trainingJob.UnitType;
                resourceRowJobQueue.TrainingBuilding = trainingBuilding;
                resourceRowJobQueue.unitTypeText.text = trainingJob.UnitType.ToString();
                resourceRowJobQueue.quantity.text = trainingJob.Quantity.ToString();
                _resourceRowsTraining.Add(resourceRowJobQueue);
            }
        }
        
        if (building is WorkshopBuilding workshopBuilding)
        {
            Instantiate(jobQueueTitlePrefab, jobQueueLayoutGrid.transform);
            Instantiate(columnsPrefab, jobQueueLayoutGrid.transform);
            foreach (var outputType in building.OutputTypesWhenCompleted)
            {
                var resourceRow = Instantiate(resourceRowProductionPrefab, jobQueueLayoutGrid.transform);
                var resourceRowJobQueue = resourceRow.GetComponent<ResourceRowProduction>();
                resourceRowJobQueue.ResourceType = outputType;
                resourceRowJobQueue.WorkshopBuilding = workshopBuilding;
                resourceRowJobQueue.resourceTypeText.text = outputType.ToString();
                resourceRowJobQueue.quantity.text = _resourceQuantities[outputType].ToString();
                _resourceRowProduction.Add(resourceRowJobQueue);
            }
        }
    }


    public void DeactivateBuildingView()
    {
        buildingView.gameObject.SetActive(false);
        
        if (outputLayoutGrid)
        {
            foreach (Transform child in outputLayoutGrid.transform)
            {
                Destroy(child.gameObject);
            }
        }

        if (inputLayoutGrid)
        {
            foreach (Transform child in inputLayoutGrid.transform)
            {
                Destroy(child.gameObject);
            }
        }
        
        if (trainingJobLayoutGrid)
        {
            foreach (Transform child in trainingJobLayoutGrid.transform)
            {
                Destroy(child.gameObject);
            }
        }
        
        if (jobQueueLayoutGrid)
        {
            foreach (Transform child in jobQueueLayoutGrid.transform)
            {
                Destroy(child.gameObject);
            }
        }

        _resourceRowProduction = new List<ResourceRowProduction>();
        _resourceRowsInput = new List<ResourceRowInput>();
        _resourceRowsOutput = new List<ResourceRowOutput>();
        _resourceRowsTraining = new List<ResourceRowTraining>();
    }

    public void UpdateResourceQuantities(Building building)
    {
        foreach (var outputRow in _resourceRowsOutput)
        {
            var resType = outputRow.ResourceType;
            var resValue = building.Inventory.FirstOrDefault(res => res.Key == resType).Value;
            outputRow.UpdateQuantity(resValue.Current);
            outputRow.UpdateInOutGoingJobs(resValue.Outgoing); 
        }
        
        foreach (var inputRow in _resourceRowsInput)
        {
            var resType = inputRow.ResourceType;
            var resValue = building.Inventory.FirstOrDefault(res => res.Key == resType).Value;
            inputRow.UpdateQuantity(resValue.Current);
            inputRow.UpdateInIncomingJobs(resValue.Incoming);
        }
        
        if (building is TrainingBuilding trainingBuilding)
        {
            foreach (var jobRow in _resourceRowsTraining)
            {
                jobRow.UpdateQuantity(trainingBuilding.TrainingJobs.Find(el => el.UnitType == jobRow.UnitType).Quantity);
            }
        }
        
        if (building is WorkshopBuilding workshopBuilding)
        {
            foreach (var productionRow in _resourceRowProduction)
            {
                productionRow.UpdateQuantity(workshopBuilding.ProductionJobs.Find(el => el.Output.ResourceType == productionRow.ResourceType).Quantity);
            }
        }
    }

}