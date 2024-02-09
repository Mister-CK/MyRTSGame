using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu]
public class GameEvent : ScriptableObject
{
    private readonly List<Action> _listeners = new ();

    public void Raise()
    {
        for(var i = _listeners.Count -1; i >= 0; i--)
            _listeners[i].Invoke();
    }

    public void RegisterListener(Action listener)
    {
        _listeners.Add(listener);
    }

    public void UnregisterListener(Action listener)
    {
        _listeners.Remove(listener);
    }
}