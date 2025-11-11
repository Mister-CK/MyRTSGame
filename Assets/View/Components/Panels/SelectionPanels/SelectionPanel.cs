using Interface;
using UnityEngine.UIElements;

namespace View.Components.Panels.SelectionPanels
{
    public abstract class SelectionPanel : HUDPanel
    {
        protected ISelectable CurrentSelectedObject { get; private set; }

        public SelectionPanel(string id) : base(id) { }
        
        public override void Build(VisualElement parent)
        {
            base.Build(parent);
            OnBuild(); 
            Hide();
        }
        
        protected abstract void OnBuild();
        
        public void SetView(ISelectable selectable)
        {
            CurrentSelectedObject = selectable;
            Show(); 
            OnActivateLogic(selectable);
        }

        protected abstract void OnActivateLogic(ISelectable selectable);
        public abstract void UpdateView(ISelectable selectable);
        
        public void ClearView()
        {
            CurrentSelectedObject = null;
            Hide(); 
        }
    }
}