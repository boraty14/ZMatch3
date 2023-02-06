using System;
using UnityEngine;

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
    }

    private void OnDisable()
    {
        EventBus.OnLevelStart -= EventBus_OnLevelStart;
        EventBus.OnLevelEnd -= EventBus_OnLevelEnd;
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