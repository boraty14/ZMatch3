using Game.Scripts.Core;
using Game.Scripts.Handlers;
using TMPro;
using UnityEngine;

namespace Game.Scripts.UI
{
    public class CurrentScorePanel : MonoBehaviour
    {
        [SerializeField] private ScoreHandler _scoreHandler;
        [SerializeField] private TextMeshProUGUI _currentScoreText;

        private void OnEnable()
        {
            EventBus.OnLevelStart += EventBus_OnLevelStart;
            EventBus.OnLevelEnd += EventBus_OnLevelEnd;
            _scoreHandler.OnCurrentScoreChange += ScoreHandler_OnCurrentScoreChange;
        }

        private void OnDisable()
        {
            EventBus.OnLevelStart -= EventBus_OnLevelStart;
            EventBus.OnLevelEnd -= EventBus_OnLevelEnd;
            _scoreHandler.OnCurrentScoreChange -= ScoreHandler_OnCurrentScoreChange;
        }

        private void ScoreHandler_OnCurrentScoreChange()
        {
            _currentScoreText.text = $"score\n{_scoreHandler.CurrentScore}";
        }

        private void EventBus_OnLevelStart()
        {
            _currentScoreText.gameObject.SetActive(true);
            _currentScoreText.SetText("score\n0");
        }

        private void EventBus_OnLevelEnd()
        {
            _currentScoreText.gameObject.SetActive(false);
        }
    }
}