// language: csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public class PanelMapping
{
    public string buttonName;
    public string panelName;
    [HideInInspector] public Button ButtonElement;
    [HideInInspector] public VisualElement PanelElement;
}

public class PanelController : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;
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

        Debug.Log($"PanelController: initializing. Root name: '{root.name}'  (children count: {root.childCount})");
        Debug.Log(DumpElementTree(root, maxDepth: 4));

        var templateContainers = root.Query<TemplateContainer>().ToList();
        var tcNames = templateContainers.Select(t => string.IsNullOrEmpty(t.name) ? "<unnamed>" : t.name);
        Debug.Log($"PanelController: found {templateContainers.Count} TemplateContainer(s). Names: {string.Join(", ", tcNames)}");

        foreach (var mapping in panelMappings)
        {
            VisualElement found = FindPanelElement(root, templateContainers, mapping.panelName);
            mapping.PanelElement = found;

            if (mapping.PanelElement == null)
            {
                var available = CollectAllNamesIncludingTemplates(root, templateContainers);
                Debug.LogError($"Mapping failed for panel '{mapping.panelName}'. Available named elements: {string.Join(", ", available)}");
                continue;
            }

            mapping.ButtonElement = root.Q<Button>(mapping.buttonName);
            if (mapping.ButtonElement == null)
            {
                Debug.LogError($"Mapping failed for button '{mapping.buttonName}'.");
                continue;
            }

            var captured = mapping;
            mapping.ButtonElement.clicked += () => SwitchPanel(captured);
        }

        if (panelMappings.Count > 0)
            SwitchPanel(panelMappings[0], instant: true);
    }

    private VisualElement FindPanelElement(VisualElement root, List<TemplateContainer> tcs, string name)
    {
        // 1) exact name on any VisualElement
        var exact = root.Q<VisualElement>(name);
        if (exact != null)
        {
            Debug.Log($"Found '{name}' via root.Q<VisualElement>(name) -> type={exact.GetType().Name} children={exact.childCount}");
            if (exact is TemplateContainer tcExact && tcExact.childCount == 0)
            {
                // try to find a child with that name across template containers
                foreach (var tc in tcs)
                {
                    var child = tc.Q<VisualElement>(name);
                    if (child != null)
                    {
                        Debug.Log($"Resolved '{name}' inside TemplateContainer '{(string.IsNullOrEmpty(tc.name) ? "<unnamed>" : tc.name)}' (type={child.GetType().Name})");
                        return child;
                    }
                }

                Debug.Log($"Returning TemplateContainer '{(string.IsNullOrEmpty(tcExact.name) ? "<unnamed>" : tcExact.name)}' as fallback for '{name}'");
                return tcExact;
            }

            return exact;
        }

        // 2) TemplateContainer with matching name
        var tcByName = tcs.FirstOrDefault(t => t.name == name);
        if (tcByName != null)
        {
            Debug.Log($"Found '{name}' as TemplateContainer with same name -> children={tcByName.childCount}");
            var inner = tcByName.Q<VisualElement>(name);
            if (inner != null)
            {
                Debug.Log($"Found inner element '{name}' inside TemplateContainer '{name}'");
                return inner;
            }
            return tcByName;
        }

        // 3) Search inside each TemplateContainer for a child with the name
        foreach (var tc in tcs)
        {
            var child = tc.Q<VisualElement>(name);
            if (child != null)
            {
                Debug.Log($"Found '{name}' inside TemplateContainer '{(string.IsNullOrEmpty(tc.name) ? "<unnamed>" : tc.name)}'");
                return child;
            }
        }

        // 4) Case-insensitive search across all named elements (fallback)
        var ci = FindByNameCaseInsensitive(root, name);
        if (ci != null)
        {
            Debug.Log($"Found '{name}' via case-insensitive match (actual name: '{ci.name}')");
            return ci;
        }

        Debug.LogWarning($"Panel '{name}' not found by any strategy.");
        return null;
    }

    private VisualElement FindByNameCaseInsensitive(VisualElement root, string name)
    {
        var stack = new Stack<VisualElement>();
        stack.Push(root);
        while (stack.Count > 0)
        {
            var cur = stack.Pop();
            if (!string.IsNullOrEmpty(cur.name) && cur.name.Equals(name, System.StringComparison.OrdinalIgnoreCase))
                return cur;
            for (int i = 0; i < cur.childCount; i++) stack.Push(cur[i]);
        }
        return null;
    }

    private IEnumerable<string> CollectAllNames(VisualElement root)
    {
        var names = new List<string>();
        var stack = new Stack<VisualElement>();
        stack.Push(root);
        while (stack.Count > 0)
        {
            var cur = stack.Pop();
            if (!string.IsNullOrEmpty(cur.name)) names.Add(cur.name);
            for (int i = 0; i < cur.childCount; i++) stack.Push(cur[i]);
        }
        return names;
    }

    private IEnumerable<string> CollectAllNamesIncludingTemplates(VisualElement root, List<TemplateContainer> tcs)
    {
        var names = new HashSet<string>(CollectAllNames(root));
        foreach (var tc in tcs)
        {
            if (!string.IsNullOrEmpty(tc.name)) names.Add(tc.name);
            foreach (var n in CollectAllNames(tc))
                names.Add(n);
        }
        return names;
    }

    private string DumpElementTree(VisualElement root, int maxDepth = 3)
    {
        var sb = new StringBuilder();

        void Rec(VisualElement el, int depth)
        {
            if (depth > maxDepth) return;
            var classes = string.Join(",", el.GetClasses());
            sb.Append(new string(' ', depth * 2));
            sb.Append($"- ({el.GetType().Name}) name='{(el.name ?? "<null>")}' classes='{classes}' children={el.childCount}");
            sb.AppendLine();
            for (int i = 0; i < el.childCount; i++) Rec(el[i], depth + 1);
        }

        Rec(root, 0);
        return sb.ToString();
    }

    private void SwitchPanel(PanelMapping mappingToActivate, bool instant = false)
    {
        if (_activeMapping == mappingToActivate) return;

        if (_activeMapping != null)
        {
            _activeMapping.PanelElement.AddToClassList(INACTIVE_PANEL_CLASS);
            _activeMapping.ButtonElement.RemoveFromClassList(ACTIVE_CLASS);
        }

        mappingToActivate.PanelElement.RemoveFromClassList(INACTIVE_PANEL_CLASS);
        mappingToActivate.ButtonElement.AddToClassList(ACTIVE_CLASS);

        _activeMapping = mappingToActivate;
    }
}
