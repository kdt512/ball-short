using System;
using System.Collections.Generic;

public class EventDispatcher
{
    private static Dictionary<EventId, List<Action<object>>> _dictionaryEvents = new();


    public static void RegisterListener(EventId eventId, Action<object> callback)
    {
        if (_dictionaryEvents.ContainsKey(eventId))
        {
            if (!_dictionaryEvents[eventId].Contains(callback))
            {
                _dictionaryEvents[eventId].Add(callback);
            }
        }
        else
        {
            var listCallback = new List<Action<object>> { callback };
            _dictionaryEvents.Add(eventId, listCallback);
        }
    }


    public static void RemoveListener(EventId eventId, Action<object> callback)
    {
        if (_dictionaryEvents.ContainsKey(eventId) && _dictionaryEvents[eventId].Contains(callback))
        {
            _dictionaryEvents[eventId].Remove(callback);
        }
    }


    public static void PostEvent(EventId eventId, object param = null)
    {
        if (_dictionaryEvents.ContainsKey(eventId))
        {
            for (var i = 0; i < _dictionaryEvents[eventId].Count; i++)
            {
                _dictionaryEvents[eventId][i]?.Invoke(param);
            }
        }
    }
}

public enum EventId
{
    UPDATE_COIN,
    UPDATE_STAR,
    UPDATE_BACKGROUND,
    UPDATE_Bottle,
    UPDATE_Item,
    UPDATE_Bottle_BLINK,
}