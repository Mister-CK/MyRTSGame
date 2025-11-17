using Interface;
using System;
using Domain;

public class SelectionEventArgs : EventArgs, IGameEventArgs
{
    public ISelectable SelectedObject { get; }

    public SelectionEventArgs(ISelectable selectedObject)
    {
        SelectedObject = selectedObject;
    }
}