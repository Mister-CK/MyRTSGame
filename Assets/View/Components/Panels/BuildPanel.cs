using Buildings.Model;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace View.Components.Panels
{
    public class BuildPanel : HUDPanel
    {
        private GameObject[] _prefabs;
        private readonly string _resourcesPath;
        private BuildingPlacer _buildingPlacer;

        public BuildPanel(
            string id = "panel-build",
            GameObject[] prefabs = null,
            string resourcesPath = "Buildings/BuildingObjects",
            BuildingPlacer buildingPlacer = null)
            : base(id)
        {
            _prefabs = prefabs;
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

            var header = new Label("BUILD");
            header.AddToClassList("panel-header");
            Root.Add(header);

            var list = new VisualElement();
            list.AddToClassList("build-list");

            if (_prefabs == null || _prefabs.Length == 0)
            {
                var msg = new Label("No build prefabs found.");
                msg.AddToClassList("panel-message");
                Root.Add(msg);
                Root.Add(list);
                return;
            }

            foreach (var prefab in _prefabs)
            {
                if (prefab == null) continue;
                var p = prefab;

                var btn = new Button(() =>
                {
                    var buildingComponent = p.GetComponent<Building>();
                    if (_buildingPlacer != null)
                    {
                        if (buildingComponent != null)
                            _buildingPlacer.StartPlacingBuildingFoundation(buildingComponent);
                        else
                            Object.Instantiate(p);
                    }
                    else
                    {
                        Object.Instantiate(p);
                    }
                })
                {
                    text = p.name
                };

                btn.AddToClassList("build-button");
                btn.tooltip = p.name;
                list.Add(btn);
            }

            Root.Add(list);
        }
    }
}
