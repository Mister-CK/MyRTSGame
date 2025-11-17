using Interface;
using System;

public class SelectionEventArgs : EventArgs, IGameEventArgs
{
    public ISelectable SelectedObject { get; }

    public SelectionEventArgs(ISelectable selectedObject)
    {
        SelectedObject = selectedObject;
    }
}