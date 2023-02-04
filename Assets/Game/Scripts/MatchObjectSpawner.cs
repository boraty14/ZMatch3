using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MatchObjectSpawner : PoolerBase<MatchObject>
{
    [SerializeField] private MatchObject _matchObjectPrefab;
    private GridBoard _gridBoard;
    private int _matchTypeCount;
    private readonly Dictionary<int, int> _columnSpawnCountDictionary = new Dictionary<int, int>();

    public void Initialize(GridBoard gridBoard)
    {
        _gridBoard = gridBoard;
        _matchTypeCount = Enum.GetNames(typeof(MatchObjectType)).Length;
        ResetSpawnColumnList();
        InitPool(_matchObjectPrefab,GridBoard.GridSize * GridBoard.GridSize);
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
            _columnSpawnCountDictionary[gridCoordinates.X]++;
        }
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
                matchObject.Initialize(GetRandomMatchType());
                matchObject.transform.name = $"{j},{i}";
                matchObject.transform.position =
                    GridBoard.GetWorldPositionFromGridCoordinates(new GridCoordinates { X = j, Y = i });
            }
        }
    }

    private void ResetSpawnColumnList()
    {
        for (int i = 0; i < GridBoard.GridSize; i++)
        {
            _columnSpawnCountDictionary[i] = 0;
        }
    }
}