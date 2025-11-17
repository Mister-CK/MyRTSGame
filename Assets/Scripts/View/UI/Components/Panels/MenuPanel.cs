using View.Extensions;

namespace View.Components.Panels
{
    // csharp
    using UnityEngine.UIElements;

    namespace View.Components.Panels
    {
        public class MenuPanel : HUDPanel
        {
            public MenuPanel(string id = "panel-menu") : base(id) { }

            public override void Build(VisualElement parent)
            {
                base.Build(parent);
                
                var header = Root.CreateChild<Label>("panel-header", "panel-header");
                header.text = "Menu PANEL";
            }
        }
    }

}
