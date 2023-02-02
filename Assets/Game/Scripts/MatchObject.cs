using UnityEngine;

public class MatchObject : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private MatchObjectType _matchObjectType;

    public bool IsType(MatchObjectType matchObjectType) => _matchObjectType == matchObjectType;

    public void Initialize(MatchObjectType matchObjectType)
    {
        _matchObjectType = matchObjectType;
    }
}


