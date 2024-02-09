using UnityEngine;
using System;
using System.Collections.Generic;
using MyRTSGame.Model;

[CreateAssetMenu]
public class GameEventBuilding : ScriptableObject
{
    private readonly List<Action<Building>> _listeners = new ();

    public void Raise(Building building)
    {
        for(var i = _listeners.Count -1; i >= 0; i--)
            _listeners[i].Invoke(building);
    }

    public void RegisterListener(Action<Building> listener)
    {
        _listeners.Add(listener);
    }

    public void UnregisterListener(Action<Building> listener)
    {
        _listeners.Remove(listener);
    }
}