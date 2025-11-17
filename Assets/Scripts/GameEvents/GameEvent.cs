using Interface;
using UnityEngine;
using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Domain;

[CreateAssetMenu]
public class GameEvent : ScriptableObject
{
    private readonly List<Action<IGameEventArgs>> _listeners = new ();

    public void Raise([CanBeNull] IGameEventArgs args)
    {
        for(var i = _listeners.Count -1; i >= 0; i--)
            _listeners[i].Invoke(args);
    }

    public void RegisterListener(Action<IGameEventArgs> listener)
    {
        _listeners.Add(listener);
    }

    public void UnregisterListener(Action<IGameEventArgs> listener)
    {
        _listeners.Remove(listener);
    }
}