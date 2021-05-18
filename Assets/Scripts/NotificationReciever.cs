using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class NotificationReciever : INotificationReceiver
{
    public void OnNotify(Playable origin, INotification notification, object context)
    {
        if (notification != null)
        {
            double time = origin.IsValid() ? origin.GetTime() : 0.0;
            Debug.LogFormat("Received notification of type {0} at time {1}", notification.GetType(), time);
        }
    }
}
