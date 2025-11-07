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

                var header = new Label("Menu PANEL");
                header.AddToClassList("panel-header");
                Root.Add(header);

                var list = new ScrollView();
                list.Add(new Label("option 1"));
                Root.Add(list);
            }
        }
    }

}
