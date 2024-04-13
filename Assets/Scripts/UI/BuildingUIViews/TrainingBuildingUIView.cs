using System.Collections.Generic;
using System.Linq;
using MyRTSGame.Model;
using MyRTSGame.Model.Components;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class TrainingBuildingUIView : MonoBehaviour
{
    [SerializeField] private Image trainingBuildingView;
    [SerializeField] private TextMeshProUGUI trainingBuildingName;
    
    [SerializeField] private GameObject columnsPrefab;
    [SerializeField] private GameObject resourceRowInputPrefab;
    [SerializeField] private GameObject resourceRowTrainingPrefab;

    [SerializeField] private GameObject inputTitlePrefab;
    [SerializeField] private GameObject inputLayoutGrid;
    
    [SerializeField] private GameObject jobQueueLayoutGrid;
    [SerializeField] private GameObject jobQueueTitlePrefab;
    
    private TrainingBuilding _currentTrainingBuilding;
    
    private List<ResourceRowInput> _resourceRowsInput = new List<ResourceRowInput>();
    private List<ResourceRowTraining> _jobRows = new List<ResourceRowTraining>();
    
    Dictionary<ResourceType, int> resourceQuantities = new Dictionary<ResourceType, int>();

    
    public void ActivateTrainingBuildingView(TrainingBuilding trainingBuilding)
    {
        _currentTrainingBuilding = trainingBuilding;
        trainingBuildingView.gameObject.SetActive(true);
        trainingBuildingName.text = trainingBuilding.BuildingType.ToString();
        Instantiate(inputTitlePrefab, inputLayoutGrid.transform);
        Instantiate(columnsPrefab, inputLayoutGrid.transform);
        
        Instantiate(jobQueueTitlePrefab, jobQueueLayoutGrid.transform);
        Instantiate(columnsPrefab, jobQueueLayoutGrid.transform);
        
        foreach (var res in trainingBuilding.InventoryWhenCompleted)
        {
            resourceQuantities[res.Key] = res.Value;
        }
        
        foreach (var inputType in trainingBuilding.InputTypesWhenCompleted)
        {
            var resourceRow = Instantiate(resourceRowInputPrefab, inputLayoutGrid.transform);
            var resourceRowInput = resourceRow.GetComponent<ResourceRowInput>();
            resourceRowInput.ResourceType = inputType;
            resourceRowInput.resourceTypeText.text = inputType.ToString();
            resourceRowInput.quantity.text = resourceQuantities[inputType].ToString();
            _resourceRowsInput.Add(resourceRowInput);
        }

        foreach (var trainingJob in trainingBuilding.TrainingJobs)
        {
            var resourceRow = Instantiate(resourceRowTrainingPrefab, jobQueueLayoutGrid.transform);
            var resourceRowJobQueue = resourceRow.GetComponent<ResourceRowTraining>();
            resourceRowJobQueue.UnitType = trainingJob.UnitType;
            resourceRowJobQueue.TrainingBuilding = trainingBuilding;
            resourceRowJobQueue.unitTypeText.text = trainingJob.UnitType.ToString();
            resourceRowJobQueue.quantity.text = trainingJob.Quantity.ToString();
            _jobRows.Add(resourceRowJobQueue);
        }
    }
    
    public void UpdateResourceQuantities(TrainingBuilding trainingBuilding)
    {
        for (var i = 0; i < _resourceRowsInput.Count; i++)
        {
            var resType = _resourceRowsInput[i].ResourceType;
            foreach (var res in trainingBuilding.Inventory)
            {
                if (res.Key == resType)
                {
                    _resourceRowsInput[i].UpdateQuantity(res.Value);
                    break;
                }
            }
            
            foreach (var res in trainingBuilding.GetIncomingResources())
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
            var unitType = _jobRows[i].UnitType;
            _jobRows[i].UpdateQuantity(trainingBuilding.TrainingJobs.Find(el => el.UnitType == unitType).Quantity);
        }
    }
    
    public void DeactivateTrainingBuildingView()
    {
        // Destroy all child objects (rows) of the outputLayoutGrid
        foreach (Transform child in inputLayoutGrid.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in jobQueueLayoutGrid.transform)
        {
            Destroy(child.gameObject);
        }
        _resourceRowsInput = new List<ResourceRowInput>();
        _jobRows = new List<ResourceRowTraining>();
        trainingBuildingView.gameObject.SetActive(false);
    }
}
