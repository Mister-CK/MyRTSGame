using System.Collections.Generic;
using System.Linq;
using MyRTSGame.Model;
using MyRTSGame.Model.Components;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WorkshopBuildingUIView : CompletedStateBuildingUIView
{
    [SerializeField] private GameObject jobQueueLayoutGrid;
    [SerializeField] private GameObject jobQueueTitlePrefab;
    [SerializeField] private GameObject resourceRowProductionPrefab;
    
    public override void ActivateBuildingView(Building building)
    {        
        base.ActivateBuildingView(building);

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
                resourceRowJobQueue.quantity.text = ResourceQuantities[outputType].ToString();
                ResourceRowProduction.Add(resourceRowJobQueue);
            }
        }
    }
    
    public override void UpdateResourceQuantities(Building building)
    {
        base.UpdateResourceQuantities(building);
        if (building is WorkshopBuilding workshopBuilding)
        {
            foreach (var productionRow in ResourceRowProduction)
            {
                productionRow.UpdateQuantity(workshopBuilding.ProductionJobs.Find(el => el.Output.ResourceType == productionRow.ResourceType).Quantity);
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
        ResourceRowProduction = new List<ResourceRowProduction>();
    }
}