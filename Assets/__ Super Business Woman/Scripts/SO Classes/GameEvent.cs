using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEvent", menuName = "SEvent/GameEvent", order = 1)]
public class GameEvent : ScriptableObject
{
    private List<GameEventListener> listeners = new List<GameEventListener>();

    public void Raise()
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised();
        }
    }

    public void RegisterListenter(GameEventListener listener)
    {
        listeners.Add(listener);
    }

    public void UnRegisterListenter(GameEventListener listener)
    {
        listeners.Remove(listener);
    }
}