using System;
using UnityEngine;

public class EventBus : MonoBehaviour
{
    public static Action<GridCoordinates> OnBlastObject;
}
