using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    public GameEvent Event;
    public UnityEvent Response;

    private void OnEnable()
    {
        Event.RegisterListenter(this);

    }

    private void OnDisable()
    {
        Event.UnRegisterListenter(this);
    }

    public void OnEventRaised()
    {
        Response.Invoke();
    }
}
