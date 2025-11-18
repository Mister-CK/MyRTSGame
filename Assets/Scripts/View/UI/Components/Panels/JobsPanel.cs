using UnityEngine.UIElements;
using View.Extensions;

namespace View.Components.Panels
{
    public class JobsPanel : HUDPanel
    {
        public JobsPanel(string id = "panel-jobs") : base(id) { }

        public override void Build(VisualElement parent)
        {
            base.Build(parent);

            var header = Root.CreateChild<Label>("panel-header");
            header.text = "JOBS PANEL";       
        }
    }
}
