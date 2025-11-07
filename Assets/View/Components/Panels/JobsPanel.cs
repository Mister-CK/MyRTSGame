// csharp
using UnityEngine.UIElements;

namespace View.Components.Panels
{
    public class JobsPanel : HUDPanel
    {
        public JobsPanel(string id = "panel-jobs") : base(id) { }

        public override void Build(VisualElement parent)
        {
            base.Build(parent);

            var header = new Label("JOBS PANEL");
            header.AddToClassList("panel-header");
            Root.Add(header);

            var list = new ScrollView();
            list.Add(new Label("Job 1"));
            list.Add(new Label("Job 2"));
            Root.Add(list);
        }
    }
}
