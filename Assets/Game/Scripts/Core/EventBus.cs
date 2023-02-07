using System;
using Game.Scripts.Matches;
using UnityEngine;

namespace Game.Scripts.Core
{
    public class EventBus : MonoBehaviour
    {
        public static Action<MatchObject> OnBlastObject;
        public static Action<MatchObject> OnReleaseObject;
        public static Action OnLevelStart;
        public static Action OnLevelEnd;
    }
}