using UnityEngine;

public class MatchObjectSpawner : PoolerBase<MatchObject>
{
    [SerializeField] private MatchObject _matchObjectPrefab;
    private GridBoard _gridBoard;

    public void Initialize(GridBoard gridBoard)
    {
        _gridBoard = gridBoard;
        InitPool(_matchObjectPrefab,GridBoard.GridSize * GridBoard.GridSize);
        InitializeMatchObjects();
    }

    private void InitializeMatchObjects()
    {
        for (int i = 0; i < GridBoard.GridSize; i++)
        {
            for (int j = 0; j < GridBoard.GridSize; j++)
            {
                var randomTypeIndex = Random.Range(0, 5);
                var matchObject = GetItemFromPool();
                _gridBoard.MatchObjectsArray[j, i] = matchObject;
                matchObject.Initialize((MatchObjectType)randomTypeIndex);
                matchObject.transform.name = $"{j},{i}";
                matchObject.transform.position =
                    GridBoard.GetWorldPositionFromGridCoordinates(new GridCoordinates { X = j, Y = i });
            }
        }
    }
}