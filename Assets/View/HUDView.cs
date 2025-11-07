// csharp

using Buildings.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using View.Components.Panels;
using View.Components.Panels.View.Components.Panels;
using View.Extensions;

namespace View
{
    public class HUDView : MonoBehaviour
    {
        [SerializeField] private UIDocument _uiDocument;
        [SerializeField] private StyleSheet _stylesheet;

        private List<Button> _menuButtons;
        private List<HUDPanel> _panels;
        private BuildingPlacer _buildingPlacer;
        private void Start()
        {
            StartCoroutine(InitializeView(4));
            _buildingPlacer = FindObjectOfType<BuildingPlacer>();
        }
        
        public IEnumerator InitializeView(int size = 4)
        {
            var root = _uiDocument.rootVisualElement;
            root.Clear();

            root.styleSheets.Add(_stylesheet);

            CreateContainers(root, out var overlay, out var leftPanel, out var topContainer, out var bottomContainer);
            CreateMenuAndPanels(topContainer, bottomContainer);
            
            yield return null;
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
            var allPanels = new HUDPanel[]
            {
                new BuildPanel("panel-build",  prefabs: null, resourcesPath: "Buildings/BuildingObjects", buildingPlacer: _buildingPlacer),
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

                var btn = CreateMenuButton(i, id, topContainer);
                _menuButtons.Add(btn);

                var panel = allPanels[i];
                panel.Build(bottomContainer);
                _panels.Add(panel);
                panel.Hide();

                BindButtonToPanel(btn, i);
            }

        }

        private static Button CreateMenuButton(int index, string panelId, VisualElement parent)
        {
            var btn = new Button();
            btn.name = $"btn-{index}";
            btn.text = panelId.Replace("panel-", "").ToUpper();
            btn.style.marginRight = 6;
            btn.style.paddingLeft = 6;
            btn.style.paddingRight = 6;
            parent.Add(btn);
            return btn;
        }

        private void BindButtonToPanel(Button btn, int capturedIndex)
        {
            btn.clicked += () => ShowPanel(capturedIndex);
        }

        private void ShowPanel(int index)
        {
            if (index < 0 || index >= _panels.Count) return;

            for (var i = 0; i < _panels.Count; i++)
            {
                if (i == index)
                {
                    _panels[i].Show();
                    _panels[i].OnActivated();
                    if (i < _menuButtons.Count) _menuButtons[i].AddToClassList("active");
                }
                else
                {
                    _panels[i].Hide();
                    _panels[i].OnDeactivated();
                    if (i < _menuButtons.Count) _menuButtons[i].RemoveFromClassList("active");
                }
            }
        }
    }
}