using System;
using Game.Scripts.Helpers;
using UnityEngine;
using UnityEngine.U2D;

namespace Game.Scripts.Matches
{
    [CreateAssetMenu]
    public class MatchObjectSpriteData : SingletonScriptableObject<MatchObjectSpriteData>
    {
        [SerializeField] private string _blueSpriteName; 
        [SerializeField] private string _redSpriteName;
        [SerializeField] private string _purpleSpriteName;
        [SerializeField] private string _greenSpriteName;
        [SerializeField] private string _yellowSpriteName;
        [SerializeField] private SpriteAtlas _matchObjectsSpriteAtlas;
    
        public Sprite GetSprite(MatchObjectType matchObjectType)
        {
            return matchObjectType switch
            {
                MatchObjectType.Blue => _matchObjectsSpriteAtlas.GetSprite(_blueSpriteName),
                MatchObjectType.Purple => _matchObjectsSpriteAtlas.GetSprite(_purpleSpriteName),
                MatchObjectType.Red => _matchObjectsSpriteAtlas.GetSprite(_redSpriteName),
                MatchObjectType.Green => _matchObjectsSpriteAtlas.GetSprite(_greenSpriteName),
                MatchObjectType.Yellow => _matchObjectsSpriteAtlas.GetSprite(_yellowSpriteName),
                _ => throw new ArgumentOutOfRangeException(nameof(matchObjectType), matchObjectType, null)
            };
        }
    }
}