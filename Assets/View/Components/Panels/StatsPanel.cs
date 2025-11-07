using UnityEngine.UIElements;

namespace View.Components.Panels
{
    public class StatsPanel: HUDPanel
    {
        public StatsPanel(string id = "panel-stats") : base(id) { }

        public override void Build(VisualElement parent)
        {
            base.Build(parent);

            var header = new Label("Stats PANEL");
            header.AddToClassList("panel-header");
            Root.Add(header);

            var list = new ScrollView();
            list.Add(new Label("stat 1"));
            Root.Add(list);
        }
    
    }
}
