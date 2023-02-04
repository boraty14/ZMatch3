using System;
using System.Collections.Generic;
using UnityEngine;

public class EventBus : MonoBehaviour
{
    public static Action<List<GridCoordinates>> OnBlastObjects;
}
