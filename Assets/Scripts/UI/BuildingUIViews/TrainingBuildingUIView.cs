using System.Collections.Generic;
using System.Linq;
using MyRTSGame.Model;
using MyRTSGame.Model.Components;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class TrainingBuildingUIView : CompletedStateBuildingUIView
{
    [SerializeField] private GameObject jobQueueLayoutGrid;
    [SerializeField] private GameObject jobQueueTitlePrefab;
    [SerializeField] private GameObject resourceRowTrainingPrefab;
    
    private List<ResourceRowTraining> _jobRows = new List<ResourceRowTraining>();
    
    public override void ActivateBuildingView(Building building)
    {
        base.ActivateBuildingView(building);
        
        if (building is TrainingBuilding trainingBuilding)
        {
            Instantiate(jobQueueTitlePrefab, jobQueueLayoutGrid.transform);
            Instantiate(columnsPrefab, jobQueueLayoutGrid.transform);
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
    }
    
    public override void UpdateResourceQuantities(Building building)
    {
        base.UpdateResourceQuantities(building);

        if (building is TrainingBuilding trainingBuilding)
        {
            foreach (var jobRow in _jobRows)
            {
                var unitType = jobRow.UnitType;
                jobRow.UpdateQuantity(trainingBuilding.TrainingJobs.Find(el => el.UnitType == unitType).Quantity);
            }
        }

    }
    
    public override void DeactivateBuildingView()
    {
        base.DeactivateBuildingView();

        foreach (Transform child in jobQueueLayoutGrid.transform)
        {
            Destroy(child.gameObject);
        }
        _jobRows = new List<ResourceRowTraining>();
    }
}
