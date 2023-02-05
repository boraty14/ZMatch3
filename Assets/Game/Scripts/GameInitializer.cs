using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] private InputHandler _inputHandler;
    [SerializeField] private MatchObjectSpawner _matchObjectSpawner;
    
    private void Start()
    {
        var gridBoard = new GridBoard(_matchObjectSpawner);
        _inputHandler.Initialize(gridBoard);
        _matchObjectSpawner.Initialize(gridBoard);
    }
}