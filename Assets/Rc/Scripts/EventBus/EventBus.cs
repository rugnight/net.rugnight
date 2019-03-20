using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IEvent { }

public interface IEventReciver<T> where T : IEvent
{
    void OnEventRecive(T evt);
}

public class EventBus<T> where T : IEvent
{
    static List<IEventReciver<T>> eventRecivers = new List<IEventReciver<T>>();

    public void Notify(T evt)
    {
        for (int i = 0; i < eventRecivers.Count; ++i)
        {
            eventRecivers[i].OnEventRecive(evt);
        }
    }

    public void Subscribe(IEventReciver<T> eventReciver)
    {
        eventRecivers.Add(eventReciver);
    }

    public void Unsubscribe(IEventReciver<T> eventReciver)
    {
        eventRecivers.Remove(eventReciver);
    }
}
