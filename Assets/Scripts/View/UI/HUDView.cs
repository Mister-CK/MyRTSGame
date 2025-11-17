using Buildings.Model;
using Data;
using Interface;
using System.Collections.Generic;
using Terrains;
using UnityEngine;
using UnityEngine.UIElements;
using View.Components.Panels;
using View.Components.Panels.View.Components.Panels;
using View.Extensions;

namespace View
{
    public class HUDView
    {
        private static BuildingPanelData _buildPanelData;
        private List<Button> _menuButtons;
        private List<HUDPanel> _panels;
        private readonly BuildingPlacer _buildingPlacer;
        private readonly TerrainPlacer _terrainPlacer;
        private readonly HUDController _hudController;
        public HUDView(HUDController hudController, BuildingPlacer buildingPlacer, TerrainPlacer terrainPlacer)
        {
            _hudController = hudController;
            _buildingPlacer = buildingPlacer;
            _terrainPlacer = terrainPlacer;
        }
        
        public void Initialize(UIDocument uiDocument, StyleSheet stylesheet)
        {
            var root = uiDocument.rootVisualElement;
            root.Clear();
            root.styleSheets.Add(stylesheet);

            CreateContainers(root, out var overlay, out var leftPanel, out var topContainer, out var bottomContainer);
            CreateMenuAndPanels(topContainer, bottomContainer);
            CreateSelectionPanel(bottomContainer, _hudController);
        }

        private static void CreateContainers(VisualElement root, out VisualElement overlay, out VisualElement leftPanel, out VisualElement topContainer, out VisualElement bottomContainer)
        {
            overlay = root.CreateChild("overlay", "overlay");
            leftPanel = overlay.CreateChild("left-panel", "left-panel");
            topContainer = leftPanel.CreateChild("top-container", "top-container");
            bottomContainer = leftPanel.CreateChild("bottom-container", "bottom-container");
        }

        private void CreateMenuAndPanels(VisualElement topContainer, VisualElement bottomContainer)
        { 
            if (_buildPanelData == null)
            {
                _buildPanelData = Resources.Load<BuildingPanelData>("UI/ScriptableObjects/BuildPanelData");
            }
            
            var allPanels = new HUDPanel[]
            {
                new BuildPanel("panel-build", _buildPanelData, _buildingPlacer, _terrainPlacer),
                new JobsPanel("panel-jobs"),
                new StatsPanel("panel-stats"),
                new MenuPanel("panel-menu"),
            };

            _menuButtons = new List<Button>(allPanels.Length);
            _panels = new List<HUDPanel>(allPanels.Length);
            
            var panelIds = new[] { "panel-build", "panel-jobs", "panel-stats", "panel-menu" };

            for (var i = 0; i < allPanels.Length; i++) 
            {
                var id = panelIds.Length > i ? panelIds[i] : $"panel-{i}";


                var panel = allPanels[i];
                panel.Build(bottomContainer);
                _panels.Add(panel);
                panel.Hide();

                var btn = CreateMenuButton(i, id, topContainer);
                _menuButtons.Add(btn);
                BindButtonToPanel(btn, i);
            }
        }

        private static Button CreateMenuButton(int index, string panelId, VisualElement parent)
        {
            var btn = parent.CreateChild<Button>("button");
            btn.name = $"btn-{index}";
            btn.text = panelId.Replace("panel-", "").ToUpper();
            return btn;
        }

        private void BindButtonToPanel(Button btn, int capturedIndex)
        {
            btn.clicked += () => ShowPanel(capturedIndex);
        }
        
        public void HideAllPanels()
        {
            foreach (var panel in _panels)
            {
                panel.Hide();
            }
            foreach (var btn in _menuButtons)
            {
                btn.RemoveFromClassList("active");
            }
        }

        private void ShowPanel(int index)
        {
            if (index < 0 || index >= _panels.Count) return;

            for (var i = 0; i < _panels.Count; i++)
            {
                if (i == index)
                {
                    _panels[i].Show();
                    if (i < _menuButtons.Count) _menuButtons[i].AddToClassList("active");
                }
                else
                {
                    _panels[i].Hide();
                    if (i < _menuButtons.Count) _menuButtons[i].RemoveFromClassList("active");
                }
            }
        }

        public void ShowSelectionPanel(ISelectable selectable)
        {
            for (var i = 0; i < _panels.Count; i++)
            {
                _panels[i].Hide();
                if (i < _menuButtons.Count) _menuButtons[i].RemoveFromClassList("active");
            }
            var found = _panels.Find(p => p is SelectionPanel);
            if (found is not SelectionPanel selectionPanel) return;
            selectionPanel.Show();
            selectionPanel.SetView(selectable);
        }
        
        private void CreateSelectionPanel(VisualElement bottomContainer, HUDController hudController)
        {
            var selectionPanel = new SelectionPanel(hudController, "panel-selectio");
            selectionPanel.Build(bottomContainer);
            _panels.Add(selectionPanel);
            selectionPanel.Hide();
        }
        
        public void UpdateSelectionPanel()
        {
            var found = _panels.Find(p => p is SelectionPanel);
            if (found is not SelectionPanel selectionPanel) return;
            selectionPanel.UpdateView();
        }
    }
}