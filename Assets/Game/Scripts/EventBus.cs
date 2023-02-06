using System;
using UnityEngine;

public class EventBus : MonoBehaviour
{
    public static Action<MatchObject> OnBlastObject;
    public static Action<MatchObject> OnReleaseObject;
    public static Action OnLevelStart;
    public static Action OnLevelEnd;
}