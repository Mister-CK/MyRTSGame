using Buildings.Model;
using Terrains;
using UnityEngine;
using UnityEngine.UIElements;
using UI.Controller;

namespace View
{

    public class HUDController : MonoBehaviour
    {
        [SerializeField] private UIDocument uiDocument;
        [SerializeField] private StyleSheet stylesheet;

        private HUDView _hudView;
        [SerializeField] private SelectionController selectionController;

        private void Awake()
        {
            var buildingPlacer = FindObjectOfType<BuildingPlacer>();
            var terrainPlacer = FindObjectOfType<TerrainPlacer>();
            
            _hudView  = new HUDView(buildingPlacer, terrainPlacer);
            
            if (selectionController == null) selectionController = FindFirstObjectByType<SelectionController>();

            if (_hudView == null || selectionController == null)
            {
                Debug.LogError("HUDController failed to find required dependencies (HUDView, SelectionService). Check scene setup.");
            }
        }

        private void Start()
        {
            _hudView.Initialize(uiDocument, stylesheet);
            // var panelsMap = SelectionPanelRegistry.CreateAndBuildPanels(bottomContainer);
            // var purePresenter = new SelectionPresenter(bottomContainer, panelsMap);


            Debug.Log("HUD System fully initialized and dependencies wired.");
        }
    }
}