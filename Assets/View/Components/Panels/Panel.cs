using UnityEngine.UIElements;

namespace View.Components.Panels
{
    public abstract class HUDPanel
    {
        public string Id { get; }
        public VisualElement Root { get; private set; }

        protected HUDPanel(string id)
        {
            Id = id;
        }

        public virtual void Build(VisualElement parent)
        {
            Root = new VisualElement { name = Id };
            Root.AddToClassList("panel");
            parent.Add(Root);
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
