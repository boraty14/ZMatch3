using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] private InputHandler _inputHandler;
    [SerializeField] private MatchObjectSpawner _matchObjectSpawner;
    [SerializeField] private Image _fadeImage;
    
    private void Start()
    {
        Application.targetFrameRate = 60;
        var gridBoard = new GridBoard(_matchObjectSpawner);
        _inputHandler.Initialize(gridBoard);
        _matchObjectSpawner.Initialize(gridBoard);
        _fadeImage.DOFade(0f, 1f).SetEase(Ease.InSine).OnComplete(() => EventBus.OnLevelStart?.Invoke());
    }
}