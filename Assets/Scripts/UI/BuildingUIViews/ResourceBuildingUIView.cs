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
    public void ActivateResourceBuildingView(ResourceBuilding building)
    {
        resourceBuildingView.gameObject.SetActive(true);
        resourceBuildingName.text = building.BuildingType.ToString();
        Instantiate(outputTitlePrefab, outputLayoutGrid.transform);
        Instantiate(columnsPrefab, outputLayoutGrid.transform);

        foreach (var res in building.InventoryWhenCompleted)
        {
            var resourceRow = Instantiate(resourceRowOutputPrefab, outputLayoutGrid.transform);
            resourceRow.GetComponent<ResourceRowOutput>().ResourceType.text = res.ResourceType.ToString();
            resourceRow.GetComponent<ResourceRowOutput>().Quantity.text = res.Quantity.ToString();
        }
    }
    
    public void DeactivateResourceBuildingView()
    {
        // Destroy all child objects (rows) of the outputLayoutGrid
        foreach (Transform child in outputLayoutGrid.transform)
        {
            Destroy(child.gameObject);
        }

        // Deactivate the view
        resourceBuildingView.gameObject.SetActive(false);
    }
}
