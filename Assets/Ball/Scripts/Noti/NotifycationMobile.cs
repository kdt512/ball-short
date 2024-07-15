using System;
using System.Collections.Generic;
#if UNITY_IOS
using Unity.Notifications.iOS;
#endif
#if UNITY_ANDROID
using Unity.Notifications.Android;
#endif
using UnityEngine;

public class NotifyData
{
    public int hours;
    public int minutes;

    public NotifyData(int hour, int minute)
    {
        this.hours = hour;
        this.minutes = minute;
    }
}

public class NotifycationMobile : MonoBehaviour
{
    [SerializeField] string THREAD;
    [SerializeField] string NOTIFY_NAME;
    [SerializeField] string NOTIFY_CATEGORY;
    [SerializeField] string NOTIFY_ID;
    List<List<string>> notifyDesc;
    [SerializeField] NotifyData[] notifyData;

    private void Awake()
    {
        notifyData = new NotifyData[]
        {
            new NotifyData(22, 55),
            new NotifyData(7, 15),
            new NotifyData(11, 50),
            new NotifyData(19, 55)
        };
        notifyDesc = new List<List<string>>();
        notifyDesc.Add(new List<string>(new string[]
            {
                "Get ready for a newday? Charge your enery with some sorting puzzles NOW!",
                "Hurry up! More challenging puzzles are waiting for you!",
                "Looking for something new today? Try our new theme NOW!",
                "You missed a few stars yesterday! Return and achieve FULL 3 STARS per level NOW!",
                "Still sleepy? Here is some gifts for you to start you new day! Claim now!"
            })
        );
        notifyDesc.Add(new List<string>(new string[]
            {
                "It's lunch time! And we are hungry to see you perform another incredible moves! Play now!",
                "Don't forget to break some puzzles today! PLAY NOW and reach more exciting levels!",
                "Refresh your mind and solve exclusive puzzles! Play now!",
            })
        );
        notifyDesc.Add(new List<string>(new string[]
            {
                "Looking for something relax after a tired day? PLAY Pool Ball Sort and chill!",
                "New levels are here! Play and Unlock them now!",
                "You get FREE Gifts! Claim them and Challenge more puzzles NOW!"
            })
        );
    }

    private void Start()
    {
#if UNITY_ANDROID
        InitCallback();
        Notification();
#elif UNITY_IOS
        StartCoroutine(RequestAuthorization());
#endif
    }
#if UNITY_ANDROID

    public void InitCallback()
    {
        AndroidNotificationCenter.OnNotificationReceived += OnReceiveNotifcation;
    }

    public void Notification()
    {
        AndroidNotificationCenter.CancelAllNotifications();
        var channel = new AndroidNotificationChannel()
        {
            Name = NOTIFY_NAME,
            Id = NOTIFY_ID,
            Description = "Generic notifications",
            Importance = Importance.Default,
        };

        AndroidNotificationCenter.RegisterNotificationChannel(channel);

        for (int i = 0; i < notifyDesc.Count; i++)
        {
            var notification = new AndroidNotification();
            notification.Title = NOTIFY_NAME;
            int idx = UnityEngine.Random.Range(0, notifyDesc[i].Count);
            notification.Text = notifyDesc[i][idx];

            notification.SmallIcon = "icon_0";
            notification.LargeIcon = "icon_1";

            DateTime notificationTime = GetNotificationTime(notifyData[i]);
            notification.FireTime = notificationTime;
            notification.RepeatInterval = new TimeSpan(TimeSpan.TicksPerDay);
            AndroidNotificationCenter.SendNotification(notification, NOTIFY_ID);
        }
    }


    public void OnReceiveNotifcation(AndroidNotificationIntentData data)
    {
        var msg = "Notification received : " + data.Id + "\n";
        msg += "\n Notification received: ";
        msg += "\n .Title: " + data.Notification.Title;
        msg += "\n .Body: " + data.Notification.Text;
        msg += "\n .Channel: " + data.Channel;
    }
#endif

#if UNITY_IOS
    public void SendIOSNotification()
    {
        iOSNotificationCenter.RemoveAllScheduledNotifications();
        for (int i = 0; i < notifyTimes.Length; i ++)
        {
            var timeTrigger = new iOSNotificationCalendarTrigger()
            {
                Hour = notifyTimes[i].hour,
                Minute = notifyTimes[i].minute,
                Repeats = true
            };

            int idx = UnityEngine.Random.Range(0, notifyMessages[i].Count);

            var notification = new iOSNotification()
            {
                // You can specify a custom identifier which can be used to manage the notification later.
                // If you don't provide one, a unique string will be generated automatically.
                Identifier = NOTIFICATION_ID,
                Title = NOTIFICATION_NAME,
                Body = notifyMessages[i][idx],
                ShowInForeground = true,
                ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
                CategoryIdentifier = NOTIFICATION_CATEGORY,
                ThreadIdentifier = THREAD_IDENTIFIER,
                Trigger = timeTrigger,
            };

            iOSNotificationCenter.ScheduleNotification(notification);
        }
    }

    IEnumerator RequestAuthorization()
    {
        var authorizationOption = AuthorizationOption.Alert | AuthorizationOption.Badge;
        using (var req = new AuthorizationRequest(authorizationOption, true))
        {
            while (!req.IsFinished)
            {
                yield return null;
            };

            string res = "\n RequestAuthorization:";
            res += "\n finished: " + req.IsFinished;
            res += "\n granted :  " + req.Granted;
            res += "\n error:  " + req.Error;
            res += "\n deviceToken:  " + req.DeviceToken;
            SendIOSNotification();
        }
    }
#endif

    DateTime GetNotificationTime(NotifyData t)
    {
        DateTime notifyTimes = System.DateTime.Now;
        int MINUTE = t.minutes;
        int HOUR = t.hours;

        if (notifyTimes.Hour * 60 + notifyTimes.Minute < HOUR * 60 + MINUTE)
        {
            notifyTimes = notifyTimes.AddMinutes(HOUR * 60 + MINUTE - (notifyTimes.Hour * 60 + notifyTimes.Minute));
        }
        else
        {
            notifyTimes =
                notifyTimes.AddMinutes(HOUR * 60 + MINUTE + 24 * 60 - (notifyTimes.Hour * 60 + notifyTimes.Minute));
        }

        return notifyTimes;
    }
}