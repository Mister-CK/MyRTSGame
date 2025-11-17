using UnityEngine.UIElements;
using View.Extensions;

namespace View.Components.Panels
{
    public abstract class HUDPanel
    {
        private string Id { get; }
        protected VisualElement Root { get; private set; }

        protected HUDPanel(string id)
        {
            Id = id;
        }

        public virtual void Build(VisualElement parent)
        {
            Root = parent.CreateChild(Id, "panel");
        }

        public void Show()
        {
            if (Root != null) Root.style.display = DisplayStyle.Flex;
        }

        public void Hide()
        {
            if (Root != null) Root.style.display = DisplayStyle.None;
        }
    }
}
