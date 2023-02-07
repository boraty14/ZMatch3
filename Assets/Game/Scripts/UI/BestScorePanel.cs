using Game.Scripts.Core;
using Game.Scripts.Handlers;
using TMPro;
using UnityEngine;

namespace Game.Scripts.UI
{
    public class BestScorePanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _bestScoreText;
    
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
            _bestScoreText.gameObject.SetActive(true);
            _bestScoreText.text = $"best\n{PlayerPrefs.GetInt(ScoreHandler.BestScoreKey)}";

        }

        private void EventBus_OnLevelEnd()
        {
            _bestScoreText.gameObject.SetActive(false);
        }
    }
}