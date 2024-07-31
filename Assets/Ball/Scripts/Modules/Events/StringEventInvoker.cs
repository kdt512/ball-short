using UnityEngine;
using UnityEngine.Events;

public class StringEventInvoker : MonoBehaviour
{
    UnityEvent<string> unityEvent = new UnityEvent<string>();

    public UnityEvent<string> UnityEvents
    {
        get { return unityEvent; }
    }

    public void AddListener(StringEventName eventName, UnityAction<string> listener)
    {
        unityEvent.AddListener(listener);
    }

    public void CallEvent(string value)
    {
        unityEvent.Invoke(value);
    }
}
