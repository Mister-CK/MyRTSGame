using UnityEngine.UIElements;
using View.Extensions;

namespace View.Components.Panels
{
    public abstract class HUDPanel
    {
        public string Id { get; }
        protected VisualElement Root { get; private set; }

        protected HUDPanel(string id)
        {
            Id = id;
        }

        public virtual void Build(VisualElement parent)
        {
            Root = parent.CreateChild(Id, "panel");
        }

        public virtual void Show()
        {
            if (Root != null) Root.style.display = DisplayStyle.Flex;
        }

        public virtual void Hide()
        {
            if (Root != null) Root.style.display = DisplayStyle.None;
        }

        // Optional hook for when the panel becomes active (override as needed)
        public virtual void OnActivated() { }

        // Optional hook for when the panel is deactivated
        public virtual void OnDeactivated() { }
    }
}
