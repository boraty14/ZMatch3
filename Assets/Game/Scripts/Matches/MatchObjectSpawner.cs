using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Game.Scripts.Core;
using Game.Scripts.Grids;
using Game.Scripts.Helpers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Scripts.Matches
{
    public class MatchObjectSpawner : PoolerBase<MatchObject>
    {
        [SerializeField] private MatchObject _matchObjectPrefab;
        private GridBoard _gridBoard;
        private MatchChecker _matchChecker;
        private int _matchTypeCount;

        public void Initialize(GridBoard gridBoard)
        {
            _gridBoard = gridBoard;
            _matchChecker = _gridBoard.matchChecker;
            _matchTypeCount = Enum.GetNames(typeof(MatchObjectType)).Length;
            InitPool(_matchObjectPrefab, GridBoard.GridSize * GridBoard.GridSize);
        }

        protected override void GetSetup(MatchObject obj)
        {
            base.GetSetup(obj);
            obj.transform.localScale = Vector3.one;
            obj.SetObjectSelectedState(false);
            obj.ResetFallCount();
        }

        private void OnEnable()
        {
            EventBus.OnBlastObject += EventBus_OnBlastObject;
            EventBus.OnReleaseObject += EventBus_OnReleaseObject;
            EventBus.OnLevelStart += EventBus_OnLevelStart;
        }

        private void OnDisable()
        {
            EventBus.OnBlastObject -= EventBus_OnBlastObject;
            EventBus.OnLevelStart -= EventBus_OnLevelStart;
            EventBus.OnReleaseObject -= EventBus_OnReleaseObject;
        }

        private void EventBus_OnReleaseObject(MatchObject obj)
        {
            Release(obj);
        }


        private void EventBus_OnLevelStart()
        {
            InitializeMatchObjects();
        }

        private void EventBus_OnBlastObject(MatchObject blastObject)
        {
            Release(blastObject);
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

        public void InitializeMatchObjects()
        {
            for (int i = 0; i < GridBoard.GridSize; i++)
            {
                for (int j = 0; j < GridBoard.GridSize; j++)
                {
                    var matchObject = GetItemFromPool();
                    _gridBoard.MatchObjectsArray[j, i] = matchObject;
                    var coordinates = new GridCoordinates { X = j, Y = i };
                    matchObject.Initialize(GetRandomMatchType(), coordinates, _gridBoard);
                    while (_matchChecker.IsObjectCreatingMatch(coordinates))
                    {
                        matchObject.Initialize(GetRandomMatchType(), coordinates, _gridBoard);
                    }
                    matchObject.transform.position =
                        GridBoard.GetWorldPositionFromGridCoordinates(coordinates);
                }
            }
        }
    }
}