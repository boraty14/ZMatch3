using System;
using Game.Scripts.Core;
using Game.Scripts.Matches;
using UnityEngine;

namespace Game.Scripts.Handlers
{
    public class ScoreHandler : MonoBehaviour
    {
        public const string BestScoreKey = "BestScore";
        private int _currentScore;
        public Action OnCurrentScoreChange;

        public int CurrentScore
        {
            get => _currentScore;
            private set
            {
                _currentScore = value;
                OnCurrentScoreChange?.Invoke();
            }
        }

        private void OnEnable()
        {
            EventBus.OnLevelStart += EventBus_OnLevelStart;
            EventBus.OnLevelEnd += EventBus_OnLevelEnd;
            EventBus.OnBlastObject += EventBus_OnBlastObject;
        }

        private void OnDisable()
        {
            EventBus.OnLevelStart -= EventBus_OnLevelStart;
            EventBus.OnLevelEnd -= EventBus_OnLevelEnd;
            EventBus.OnBlastObject -= EventBus_OnBlastObject;
        }

        private void EventBus_OnBlastObject(MatchObject obj)
        {
            CurrentScore++;
        }

        private void EventBus_OnLevelStart()
        {
            CurrentScore = 0;
        }

        private void EventBus_OnLevelEnd()
        {
            if(CurrentScore > PlayerPrefs.GetInt(BestScoreKey)) PlayerPrefs.SetInt(BestScoreKey,CurrentScore);
        }
    }
}