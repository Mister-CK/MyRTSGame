using Buildings.Model;
using UnityEngine;
using UnityEngine.UIElements;
using View.Extensions;

namespace View.Components.Panels
{
    public class BuildPanel : HUDPanel
    {
        private static GameObject[] _prefabs;
        private static BuildingPlacer _buildingPlacer;
        private static StyleSheet _styleSheetButtonContainer;
        private readonly string _resourcesPath;
        public BuildPanel(
            string id = "panel-build",
            string resourcesPath = "Buildings/BuildingObjects",
            BuildingPlacer buildingPlacer = null)
            : base(id)
        {
            _resourcesPath = resourcesPath;
            _buildingPlacer = buildingPlacer;
            
        }

        public override void Build(VisualElement parent)
        {
            base.Build(parent);

            if (_prefabs == null || _prefabs.Length == 0)
            {
                _prefabs = Resources.LoadAll<GameObject>(_resourcesPath);
            }

            if (_buildingPlacer == null)
            {
                _buildingPlacer = Object.FindObjectOfType<BuildingPlacer>();
            }
            if (_styleSheetButtonContainer == null)
            {
                _styleSheetButtonContainer = Resources.Load<StyleSheet>("UI/Styling/BuildPanel");
            }
            var header = new Label("BUILD");
            header.AddToClassList("panel-header");
            Root.Add(header);

            var buttonContainer = Root.CreateChild("button-container", "button-container");
            buttonContainer.styleSheets.Add(_styleSheetButtonContainer);

            foreach (var prefab in _prefabs)
            {
                if (prefab == null) continue;
                var p = prefab;

                var btn = new Button(() =>
                {
                    var buildingComponent = p.GetComponent<Building>();
                    _buildingPlacer.StartPlacingBuildingFoundation(buildingComponent);
                })
                {
                    text = p.name
                };

                btn.AddToClassList("build-button");
                btn.tooltip = p.name;
                buttonContainer.Add(btn);
            }

        }
    }
}
