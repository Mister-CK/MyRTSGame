using System.Collections.Generic;
using System.Linq;
using MyRTSGame.Model;
using MyRTSGame.Model.Components;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class CompletedStateBuildingUIView : MonoBehaviour
{
    [SerializeField] protected Image buildingView;
    [SerializeField] protected TextMeshProUGUI buildingName;
    [SerializeField] protected GameObject columnsPrefab;
    
    [SerializeField] protected GameObject inputLayoutGrid;
    [SerializeField] protected GameObject inputTitlePrefab;
    [SerializeField] protected GameObject resourceRowInputPrefab;
    
    [SerializeField] protected GameObject outputLayoutGrid;
    [SerializeField] protected GameObject outputTitlePrefab;
    [SerializeField] protected GameObject resourceRowOutputPrefab;
    
    protected Dictionary<ResourceType, int> ResourceQuantities = new Dictionary<ResourceType, int>();
    
    protected List<ResourceRowInput> ResourceRowsInput = new List<ResourceRowInput>();
    protected List<ResourceRowOutput> ResourceRowsOutput = new List<ResourceRowOutput>();
    protected List<ResourceRowProduction> ResourceRowProduction = new List<ResourceRowProduction>();
    
    public virtual void ActivateBuildingView(Building building)
    {
        buildingView.gameObject.SetActive(true);
        buildingName.text = building.BuildingType.ToString();
        
        foreach (var res in building.InventoryWhenCompleted)
        {
            ResourceQuantities[res.Key] = res.Value;
        }
        
        if (building.InputTypesWhenCompleted != null)
        {
            Instantiate(inputTitlePrefab, inputLayoutGrid.transform);
            Instantiate(columnsPrefab, inputLayoutGrid.transform);
            foreach (var inputType in building.InputTypesWhenCompleted)
            {
                var resourceRow = Instantiate(resourceRowInputPrefab, inputLayoutGrid.transform);
                var resourceRowInput = resourceRow.GetComponent<ResourceRowInput>();
                resourceRowInput.ResourceType = inputType;
                resourceRowInput.resourceTypeText.text = inputType.ToString();
                resourceRowInput.quantity.text = ResourceQuantities[inputType].ToString();
                ResourceRowsInput.Add(resourceRowInput);
            }
        }
        
        if (building.OutputTypesWhenCompleted != null)
        {
            Instantiate(outputTitlePrefab, outputLayoutGrid.transform);
            Instantiate(columnsPrefab, outputLayoutGrid.transform);
            foreach (var outputType in building.OutputTypesWhenCompleted)
            {
                var resourceRow = Instantiate(resourceRowOutputPrefab, outputLayoutGrid.transform);
                var resourceRowOutput = resourceRow.GetComponent<ResourceRowOutput>();
                resourceRowOutput.ResourceType = outputType;
                resourceRowOutput.resourceTypeText.text = outputType.ToString();
                resourceRowOutput.quantity.text = ResourceQuantities[outputType].ToString();
                ResourceRowsOutput.Add(resourceRowOutput);
            }
        }
    }


    public virtual void DeactivateBuildingView()
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
        
        ResourceRowsInput = new List<ResourceRowInput>();
        ResourceRowsOutput = new List<ResourceRowOutput>();
    }

    public virtual void UpdateResourceQuantities(Building building)
    {
        foreach (var outputRow in ResourceRowsOutput)
        {
            var resType = outputRow.ResourceType;
            var resValue = building.Inventory.FirstOrDefault(res => res.Key == resType).Value;
            outputRow.UpdateQuantity(resValue);
            
            foreach (var res in building.GetOutgoingResources())
            {
                if (res.ResourceType == resType)
                {
                    outputRow.UpdateInOutGoingJobs(res.Quantity);
                    break;
                }
            }
        }
        
        foreach (var inputRow in ResourceRowsInput)
        {
            var resType = inputRow.ResourceType;
            var resValue = building.Inventory.FirstOrDefault(res => res.Key == resType).Value;
            inputRow.UpdateQuantity(resValue);
            
            foreach (var res in building.GetIncomingResources())
            {
                if (res.ResourceType == resType)
                {
                    inputRow.UpdateInIncomingJobs(res.Quantity);
                    break;
                }
            }
        }
    }

}