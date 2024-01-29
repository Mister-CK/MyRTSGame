using MyRTSGame.Interface;
using MyRTSGame.Model;
using TMPro;
using UnityEngine;

public class SelectionManager: MonoBehaviour
{
    public static SelectionManager Instance;
    private ISelectable CurrentSelectedObject { get; set; }
    
    [SerializeField] private TextMeshProUGUI textComponent;

    private void Awake()
    {
        // Ensure there is only one instance of SelectionManager
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void SelectObject(ISelectable newObject)
    {
        Debug.Log("selected object");
        if (CurrentSelectedObject != null)
        {
            // You can add code here to hide the details of the previously selected object
        }

        CurrentSelectedObject = newObject;
    }
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            CurrentSelectedObject = null;
        }
        
        if (CurrentSelectedObject == null)
        {
            textComponent.text = "";
            return;
        }
        if (CurrentSelectedObject is Building building)
        {
            string text = building.BuildingType.ToString()  + "\n"+ 
                          GetTextForInventory(building.GetInventory()) +  "\n" + 
                          building.GetState();
            textComponent.text = text;
            return;
        }
        
        if (CurrentSelectedObject is Villager villager)
        {
            textComponent.text = "Villager";
            return;
        }
    }
    
    private static string GetTextForInventory(Resource[] inventory) {
        string inventoryText = "";
        foreach (Resource resource in inventory) {
            inventoryText += resource.ResourceType + ":" + resource.Quantity + " ";
        }
        return inventoryText;
    }
}