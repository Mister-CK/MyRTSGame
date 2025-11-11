using UnityEngine.UIElements;
using View.Extensions;

namespace View.Components.Panels
{
    public class StatsPanel: HUDPanel
    {
        public StatsPanel(string id = "panel-stats") : base(id) { }

        public override void Build(VisualElement parent)
        {
            base.Build(parent);

            var header = Root.CreateChild<Label>("panel-header");
            header.text = "STATS PANEL";
        }
    
    }
}
