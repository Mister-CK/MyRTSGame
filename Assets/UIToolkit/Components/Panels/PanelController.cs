using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

// Defines the relationship between a button and a panel
[System.Serializable]
public class PanelMapping
{
    // The C# name of the button element in UXML (e.g., "btn-build")
    public string buttonName; 
    
    // The C# name of the panel element in UXML (e.g., "panel-build")
    public string panelName;
    
    [HideInInspector] public Button ButtonElement;
    [HideInInspector] public VisualElement PanelElement;
}

public class PanelController : MonoBehaviour
{
    // Drag the UIDocument root here, or query it from HUDController
    [SerializeField] private UIDocument uiDocument;
    
    // The list of mappings, configurable in the Inspector
    [SerializeField] private List<PanelMapping> panelMappings = new();

    private const string ACTIVE_CLASS = "active";
    private const string INACTIVE_PANEL_CLASS = "inactive-panel";
    private PanelMapping _activeMapping;

    public void Initialize(VisualElement root)
    {
        if (root == null)
        {
            Debug.LogError("PanelController failed to initialize: UXML root is null.");
            return;
        }

        // 1. Query and map all elements based on the Inspector list
        foreach (var mapping in panelMappings)
        {
            mapping.ButtonElement = root.Q<Button>(mapping.buttonName);
            mapping.PanelElement = root.Q<VisualElement>(mapping.panelName);

            if (mapping.ButtonElement == null || mapping.PanelElement == null)
            {
                Debug.LogError($"Mapping failed for button '{mapping.buttonName}' or panel '{mapping.panelName}'. Check UXML names.");
                continue;
            }

            // 2. Bind the click event to the generic switch method
            mapping.ButtonElement.clicked += () => SwitchPanel(mapping);
        }

        // 3. Set the default panel
        if (panelMappings.Count > 0)
        {
            SwitchPanel(panelMappings[0], instant: true);
        }
    }

    private void SwitchPanel(PanelMapping mappingToActivate, bool instant = false)
    {
        // Ignore if already active
        if (_activeMapping == mappingToActivate) return;

        // 1. Deactivate current panel/button
        if (_activeMapping != null)
        {
            _activeMapping.PanelElement.AddToClassList(INACTIVE_PANEL_CLASS);
            _activeMapping.ButtonElement.RemoveFromClassList(ACTIVE_CLASS);
        }

        // 2. Activate new panel/button
        mappingToActivate.PanelElement.RemoveFromClassList(INACTIVE_PANEL_CLASS);
        mappingToActivate.ButtonElement.AddToClassList(ACTIVE_CLASS);

        _activeMapping = mappingToActivate;
    }
}
