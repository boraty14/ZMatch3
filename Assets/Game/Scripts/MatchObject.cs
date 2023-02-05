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
    private const float FallDurationFactor = 0.2f;

    public void AddFallCount() => _fallCount++;
    public MatchObjectType GetMatchObjectType() => _matchObjectType;
    public bool IsType(MatchObjectType matchObjectType) => _matchObjectType == matchObjectType;

    public void Initialize(MatchObjectType matchObjectType,GridCoordinates objectCoordinates,GridBoard gridBoard)
    {
        _objectCoordinates = objectCoordinates;
        _gridBoard = gridBoard;
        _matchObjectType = matchObjectType;
        _spriteRenderer.sprite = MatchObjectSpriteData.Instance.GetSprite(_matchObjectType);
    }

    private float GetFallDuration(Vector3 targetPosition) =>
        Mathf.Abs(transform.position.y - targetPosition.y) * FallDurationFactor;

    public async Task PlaceAfterSpawning()
    {
        var fallFactor = GridBoard.GridSize - _objectCoordinates.Y;
        var placementPosition = GridBoard.GetWorldPositionFromGridCoordinates(_objectCoordinates);
        await transform.DOMove(placementPosition, GetFallDuration(placementPosition)).SetEase(Ease.Linear)
            .AsyncWaitForCompletion();
    }

    public async Task Fall()
    {
        if(_fallCount == 0) return;
        
        _objectCoordinates.Y -= _fallCount;
        var fallPosition = GridBoard.GetWorldPositionFromGridCoordinates(_objectCoordinates);
        await transform.DOMove(fallPosition,GetFallDuration(fallPosition)).SetEase(Ease.Linear)
            .AsyncWaitForCompletion();
        _gridBoard.MatchObjectsArray[_objectCoordinates.X, _objectCoordinates.Y] = this; 
        _fallCount = 0;
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
        await transform.DOScale(Vector3.zero, 0.2f).AsyncWaitForCompletion();
        _fallCount = 0;
    }
}