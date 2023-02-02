using UnityEngine;

public class MatchObjectSpawner : PoolerBase<MatchObject>
{
    [SerializeField] private MatchObject _matchObjectPrefab;
    private readonly MatchObject[,] _matchObjectArray = new MatchObject[GridBoard.GridSize, GridBoard.GridSize];

    private void Start()
    {
        InitPool(_matchObjectPrefab,GridBoard.GridSize * GridBoard.GridSize);
        GenerateMatchObjectOnStart();
    }

    private void GenerateMatchObjectOnStart()
    {
        for (int i = 0; i < GridBoard.GridSize; i++)
        {
            for (int j = 0; j < GridBoard.GridSize; j++)
            {
                var randomTypeIndex = Random.Range(0, 5);
                var matchObject = GetItemFromPool();
                _matchObjectArray[j, i] = matchObject;
                matchObject.Initialize((MatchObjectType)randomTypeIndex);
                matchObject.transform.name = $"{j},{i}";
                matchObject.transform.position =
                    GridBoard.GetWorldPositionFromGridCoordinates(new GridCoordinates { X = j, Y = i });
            }
        }
    }
}
