using UnityEngine;
using UnityEngine.Events;

public class NormalEventInvoker : MonoBehaviour
{
    UnityEvent unityEvent = new UnityEvent();

    public UnityEvent Event
    {
        get { return unityEvent; }
    }

    public void AddListener(UnityAction listener)
    {
        unityEvent.AddListener(listener);
    }
    
    public void CallEvent()
    {
        unityEvent.Invoke();
    }
}
