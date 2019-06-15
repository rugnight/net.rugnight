using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace rc
{
    /// <summary>
    /// 各使用者がこれを継承したイベントを作成します
    /// </summary>
    public interface IEvent { }

    /// <summary>
    /// イベントを受け取るクラスがこれを継承してイベント受け取り関数を実装します
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IEventReciver<T> where T : IEvent
    {
        void OnEventRecive(T evt);
    }

    /// <summary>
    /// イベント登録されたときに static 領域が確保されて専用のバスが作成されます
    /// </summary>
    /// <typeparam name="T"></typeparam>
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
}
