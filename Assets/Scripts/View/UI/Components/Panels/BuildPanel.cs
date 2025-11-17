using Buildings.Model;
using Data;
using System.Collections.Generic;
using Terrains;
using UnityEngine;
using UnityEngine.UIElements;
using View.Extensions;
using Object = UnityEngine.Object;
using Terrain = Terrains.Model.Terrain;

namespace View.Components.Panels
{
    public class BuildPanel : HUDPanel
    {
        private readonly BuildingPanelData _buildPanelData;
        private static BuildingPlacer _buildingPlacer;
        private static TerrainPlacer _terrainPlacer;

        private static StyleSheet _styleSheetButtonContainer;
        
        public BuildPanel(
            string id,
            BuildingPanelData panelData, 
            BuildingPlacer buildingPlacer,
            TerrainPlacer terrainPlacer)
            : base(id)
        {
            _buildPanelData = panelData;
            _buildingPlacer = buildingPlacer;
            _terrainPlacer = terrainPlacer;
            if (_buildingPlacer == null)
            {
                _buildingPlacer = Object.FindFirstObjectByType<BuildingPlacer>();
            }
            if (_terrainPlacer == null)
            {
                _terrainPlacer = Object.FindFirstObjectByType<TerrainPlacer>();
            }
        }

        public override void Build(VisualElement parent)
        {
            base.Build(parent);

            if (_styleSheetButtonContainer == null)
            {
                _styleSheetButtonContainer = Resources.Load<StyleSheet>("UI/Styling/BuildPanel"); 
            }
            
            var header = new Label("BUILD");
            header.AddToClassList("panel-header");
            Root.Add(header);
            
            var buttonContainer = Root.CreateChild("button-container","button-container");
            buttonContainer.styleSheets.Add(_styleSheetButtonContainer);
            AddButtons(buttonContainer, _buildPanelData.terrains, "terrain");
            AddButtons(buttonContainer, _buildPanelData.buildings, "building");

        }

        private void AddButtons(VisualElement buttonContainer, List<BuildableConfig> terrains, string configType)
        {
            foreach (var config in terrains)
            {
                var prefab = config.prefab;

                if (prefab == null) continue;
                
                var btn = new Button(() =>
                {
                    switch(configType) 
                    {  
                        case "terrain" :
                            var terrainComponent = prefab.GetComponent<Terrain>();
                            _buildingPlacer.StopPlacingBuildingFoundation();
                            _terrainPlacer.StartPlacingTerrainFoundation(terrainComponent);
                            return;
                        case "building" :
                            var buildingComponent = prefab.GetComponent<Building>();
                            _terrainPlacer.StopPlacingTerrainFoundation();
                            _buildingPlacer.StartPlacingBuildingFoundation(buildingComponent);
                            return;
                    }
                })
                {
                    text = config.displayName?.Length == 0 ? prefab.name : config.displayName
                };

                btn.AddToClassList("build-button");
                btn.tooltip = config.displayName?.Length == 0 ? prefab.name : config.displayName;
                buttonContainer.Add(btn);
            }
        }
    }
}
