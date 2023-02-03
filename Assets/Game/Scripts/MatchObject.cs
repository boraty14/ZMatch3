using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class MatchObject : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    public MatchObjectType _matchObjectType;
    private const float SelectedScaleFactor = 1.5f;
    private const float SwapAnimationDuration = 0.2f;

    public MatchObjectType GetMatchObjectType() => _matchObjectType;
    public bool IsType(MatchObjectType matchObjectType) => _matchObjectType == matchObjectType;

    public void Initialize(MatchObjectType matchObjectType)
    {
        _matchObjectType = matchObjectType;
        _spriteRenderer.sprite = MatchObjectSpriteData.Instance.GetSprite(_matchObjectType);
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
    }
}