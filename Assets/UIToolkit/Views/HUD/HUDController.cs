using UnityEngine;
using UnityEngine.UIElements;

public class HUDController : MonoBehaviour
{
    [Tooltip("Drag the UIDocument component here.")]
    [SerializeField] private UIDocument uiDocument;

    [Tooltip("Drag the PanelController script attached to this GameObject here.")]
    [SerializeField] private PanelController panelController;

    private void Awake()
    {
        if (uiDocument == null || panelController == null)
        {
            Debug.LogError("HUDController requires UIDocument and PanelController references.");
            return;
        }

        // Pass the root VisualElement to the PanelController for initialization
        panelController.Initialize(uiDocument.rootVisualElement);
    }
}
