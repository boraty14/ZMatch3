using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class MatchObjectSpawner : PoolerBase<MatchObject>
{
    [SerializeField] private MatchObject _matchObjectPrefab;
    private GridBoard _gridBoard;
    private int _matchTypeCount;

    public void Initialize(GridBoard gridBoard)
    {
        _gridBoard = gridBoard;
        _matchTypeCount = Enum.GetNames(typeof(MatchObjectType)).Length;
        InitPool(_matchObjectPrefab, GridBoard.GridSize * GridBoard.GridSize);
        InitializeMatchObjects();
    }

    protected override void GetSetup(MatchObject obj)
    {
        base.GetSetup(obj);
        obj.transform.localScale = Vector3.one;
        obj.SetObjectSelectedState(false);
    }

    private void OnEnable()
    {
        EventBus.OnBlastObjects += EventBus_OnBlastObject;
    }

    private void OnDisable()
    {
        EventBus.OnBlastObjects -= EventBus_OnBlastObject;
    }

    private void EventBus_OnBlastObject(List<GridCoordinates> gridCoordinatesList)
    {
        foreach (var gridCoordinates in gridCoordinatesList)
        {
            var blastObject = _gridBoard.GetMatchObjectFromCoordinates(gridCoordinates);
            Release(blastObject);
        }
    }

    public async Task GenerateAndPlaceObjectsAfterBlast(Dictionary<int,int> columnBlastDictionary)
    {
        var placementTasks = new List<Task>();
        for (int i = 0; i < GridBoard.GridSize; i++)
        {
            var columnBlastCount = columnBlastDictionary[i];
            for (int j = 0; j < columnBlastCount; j++)
            {
                var spawnPosition = GridBoard.GetNewSpawnPosition(i, j);
                var matchObject = GetItemFromPool();
                matchObject.transform.position = spawnPosition;
                var spawnCoordinates = new GridCoordinates { X = i, Y = GridBoard.GridSize - columnBlastCount + j };
                _gridBoard.MatchObjectsArray[spawnCoordinates.X, spawnCoordinates.Y] = matchObject;
                matchObject.Initialize(GetRandomMatchType(), spawnCoordinates, _gridBoard);
                placementTasks.Add(matchObject.PlaceAfterSpawning());
            }
        }
        await Task.WhenAll(placementTasks);
    }

    private MatchObjectType GetRandomMatchType()
    {
        var randomTypeIndex = Random.Range(0, _matchTypeCount);
        return (MatchObjectType)randomTypeIndex;
    }

    private void InitializeMatchObjects()
    {
        for (int i = 0; i < GridBoard.GridSize; i++)
        {
            for (int j = 0; j < GridBoard.GridSize; j++)
            {
                var matchObject = GetItemFromPool();
                _gridBoard.MatchObjectsArray[j, i] = matchObject;
                var coordinates = new GridCoordinates { X = j, Y = i };
                matchObject.Initialize(GetRandomMatchType(), coordinates, _gridBoard);
                matchObject.transform.position =
                    GridBoard.GetWorldPositionFromGridCoordinates(coordinates);
            }
        }
    }
}