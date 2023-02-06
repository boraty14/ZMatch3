using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class MatchObject : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    public MatchObjectType _matchObjectType;
    private int _fallCount;
    private GridCoordinates _objectCoordinates;
    private GridBoard _gridBoard;
    private const float SelectedScaleFactor = 1.5f;
    private const float SwapAnimationDuration = 0.2f;
    private const float BlastAnimationDuration = 0.5f;
    private const float FallDurationFactor = 0.12f;

    public void AddFallCount() => _fallCount++;
    public void ResetFallCount() => _fallCount = 0;
    public void SetCoordinates(GridCoordinates gridCoordinates) => _objectCoordinates = gridCoordinates;
    public MatchObjectType GetMatchObjectType() => _matchObjectType;
    public bool IsType(MatchObjectType matchObjectType) => _matchObjectType == matchObjectType;

    public void Initialize(MatchObjectType matchObjectType,GridCoordinates objectCoordinates,GridBoard gridBoard)
    {
        _objectCoordinates = objectCoordinates;
        _gridBoard = gridBoard;
        _matchObjectType = matchObjectType;
        _spriteRenderer.sprite = MatchObjectSpriteData.Instance.GetSprite(_matchObjectType);
    }

    private void OnEnable()
    {
        EventBus.OnLevelEnd += EventBus_OnLevelEnd;
    }

    private void OnDisable()
    {
        EventBus.OnLevelEnd -= EventBus_OnLevelEnd;
    }

    private void EventBus_OnLevelEnd()
    {
        EventBus.OnReleaseObject?.Invoke(this);
    }

    private float GetFallDuration(Vector3 targetPosition) =>
        Mathf.Abs(transform.position.y - targetPosition.y) * FallDurationFactor;

    public async Task PlaceAfterSpawning()
    {
        var placementPosition = GridBoard.GetWorldPositionFromGridCoordinates(_objectCoordinates);
        await transform.DOMove(placementPosition, GetFallDuration(placementPosition)).SetEase(Ease.Linear)
            .AsyncWaitForCompletion();
    }

    public async Task Fall()
    {
        if(_fallCount == 0) return;
        
        _objectCoordinates.Y -= _fallCount;
        ResetFallCount();
        _gridBoard.MatchObjectsArray[_objectCoordinates.X, _objectCoordinates.Y] = this; 
        var fallPosition = GridBoard.GetWorldPositionFromGridCoordinates(_objectCoordinates);
        await transform.DOMove(fallPosition,GetFallDuration(fallPosition)).SetEase(Ease.Linear)
            .AsyncWaitForCompletion();
    }

    public void SetObjectSelectedState(bool isSelected)
    {
        var scaleFactor = isSelected ? SelectedScaleFactor : 1f;
        transform.localScale = Vector3.one * scaleFactor;
    }

    public async Task PlaySwapAnimation(Vector3 swapTarget)
    {
        await transform.DOMove(swapTarget, SwapAnimationDuration).SetEase(Ease.InSine).AsyncWaitForCompletion();
    }

    public async Task Blast()
    {
        await transform.DOScale(Vector3.zero, BlastAnimationDuration).SetEase(Ease.InOutSine).AsyncWaitForCompletion();
        EventBus.OnBlastObject?.Invoke(this);
        ResetFallCount();
    }
}