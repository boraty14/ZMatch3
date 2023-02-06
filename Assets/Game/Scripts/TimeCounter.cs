using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeCounter : MonoBehaviour
{
    [SerializeField] private GameObject _timerPanel;
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private Image _timeImage;
    [SerializeField] private Image _fadeImage;
    private const float StartingTime = 6f;
    private float _currentTimer;
    private bool _isPlaying;

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

    private void EventBus_OnLevelEnd()
    {
        _timerPanel.SetActive(false);
    }

    private void EventBus_OnLevelStart()
    {
        _timerPanel.SetActive(true);
        _currentTimer = StartingTime;
        _isPlaying = true;
    }

    void Update()
    {
        if(!_isPlaying) return;
        _currentTimer -= Time.deltaTime;
        if (_currentTimer < 0f)
        {
            _timeImage.fillAmount = 0f;
            _isPlaying = false;
            _fadeImage.DOFade(1f, 2f).SetEase(Ease.InSine).OnComplete(LevelResetRoutine);
            return;
        }
        
        var timerText = Mathf.FloorToInt(_currentTimer).ToString().ToCharArray();
        _timerText.SetCharArray(timerText);
        _timeImage.fillAmount = _currentTimer / StartingTime;
        
    }

    private void LevelResetRoutine()
    {
        EventBus.OnLevelEnd.Invoke();
        _fadeImage.DOFade(0f, 2f).SetEase(Ease.InSine).OnComplete(() => EventBus.OnLevelStart?.Invoke());
    }
}