using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GridBoard
{
    private const float GridCellSize = 0.86f;
    private const float GridCellInterval = 0.07f;
    private const float GridCellIndexSize = GridCellSize + GridCellInterval;
    private const float GridStartingPointX = -3.69f;
    private const float GridStartingPointY = -6f;
    private const float GridSpriteVerticalOffset = -0.05f;
    public const int GridSize = 8;
    public const float NewGenerateHeightOffset = 1.5f;
    
    private readonly MatchChecker _matchChecker;
    private readonly MatchObjectBlaster _matchObjectBlaster;
    private readonly MatchObjectSpawner _matchObjectSpawner;
    public readonly MatchObject[,] MatchObjectsArray = new MatchObject[GridSize, GridSize];

    public GridBoard(MatchObjectSpawner matchObjectSpawner)
    {
        _matchChecker = new MatchChecker(this);
        _matchObjectBlaster = new MatchObjectBlaster(this);
        _matchObjectSpawner = matchObjectSpawner;
    }

    public static Vector3 GetNewSpawnPosition(int columnIndex, int spawnCount)
    {
        var xPosition = GridCellIndexSize * columnIndex + GridCellSize / 2f + GridStartingPointX;
        var yPosition = GridCellIndexSize * (GridSize - 1 + spawnCount) + GridCellSize / 2f + GridStartingPointY +
                        GridSpriteVerticalOffset + NewGenerateHeightOffset;
        return new Vector3(xPosition, yPosition, 0f);
    }

    public MatchObject GetMatchObjectFromCoordinates(GridCoordinates gridCoordinates)
    {
        return MatchObjectsArray[gridCoordinates.X, gridCoordinates.Y];
    }

    public static GridCoordinates GetGridCoordinatesFromWorldPoint(Vector3 worldPoint)
    {
        var xOffset = worldPoint.x - GridStartingPointX;
        var yOffset = worldPoint.y - GridStartingPointY;
        int xCoordinates = Mathf.FloorToInt(xOffset / GridCellIndexSize);
        int yCoordinates = Mathf.FloorToInt(yOffset / GridCellIndexSize);
        return new GridCoordinates
        {
            X = xCoordinates,
            Y = yCoordinates
        };
    }

    public static GridCoordinates GetGridCoordinatesFromMatchObject(MatchObject matchObject)
    {
        var position = matchObject.transform.position;
        return GetGridCoordinatesFromWorldPoint(position);
    }

    public static bool IsTouchingGrid(Vector3 worldPoint)
    {
        var xOffset = worldPoint.x - GridStartingPointX;
        var yOffset = worldPoint.y - GridStartingPointY;
        if (xOffset < 0f || yOffset < 0f) return false;
        if (xOffset / GridCellIndexSize > GridSize || yOffset / GridCellIndexSize > GridSize) return false;
        if (xOffset % GridCellIndexSize > GridCellSize || yOffset % GridCellIndexSize > GridCellSize) return false;
        return true;
    }

    public static Vector3 GetWorldPositionFromGridCoordinates(GridCoordinates gridCoordinates)
    {
        var xPosition = (GridCellIndexSize * gridCoordinates.X + GridCellSize / 2f) + GridStartingPointX;
        var yPosition = (GridCellIndexSize * gridCoordinates.Y + GridCellSize / 2f) + GridStartingPointY +
                        GridSpriteVerticalOffset;
        return new Vector3(xPosition, yPosition, 0f);
    }

    public async Task SwapMatchObjects(MatchObject firstObject, MatchObject secondObject)
    {
        var swapTask = RunObjectSwapAnimationTask(firstObject, secondObject);
        var firstGridCoordinates = GetGridCoordinatesFromMatchObject(firstObject);
        var secondGridCoordinates = GetGridCoordinatesFromMatchObject(secondObject);
        SwapMatchObjectsInArray(firstGridCoordinates, secondGridCoordinates);
        await swapTask;
        firstObject.SetObjectSelectedState(false);
        secondObject.SetObjectSelectedState(false);

        var matchingObjectsCoordinates = _matchChecker.GetMatchingObjectsCoordinates();
        if (matchingObjectsCoordinates.Count == 0)
        {
            await RunObjectSwapAnimationTask(firstObject, secondObject);
            SwapMatchObjectsInArray(firstGridCoordinates, secondGridCoordinates);
            return;
        }
        
        while (matchingObjectsCoordinates.Count != 0)
        {
            var columnBlastDictionary = await _matchObjectBlaster.BlastMatchingObjectsAndGetBlastDictionary(matchingObjectsCoordinates);
            var existingObjectsFallTask = MakeObjectsFallAfterBlast();
            var spawnedObjectsFallTask = _matchObjectSpawner.GenerateAndPlaceObjectsAfterBlast(columnBlastDictionary);
            await Task.WhenAll(existingObjectsFallTask, spawnedObjectsFallTask);
            matchingObjectsCoordinates = _matchChecker.GetMatchingObjectsCoordinates();
        }
        
    }

    private async Task MakeObjectsFallAfterBlast()
    {
        var fallTasks = new List<Task>();
        for (int i = 0; i < GridSize; i++)
        {
            for (int j = 0; j < GridSize; j++)
            {
                fallTasks.Add(MatchObjectsArray[j, i].Fall());
            }
        }
        await Task.WhenAll(fallTasks);
    }

    private static async Task RunObjectSwapAnimationTask(MatchObject firstObject, MatchObject secondObject)
    {
        var firstObjectPosition = firstObject.transform.position;
        var secondObjectPosition = secondObject.transform.position;
        var firstSwapTask = firstObject.PlaySwapAnimation(secondObjectPosition);
        var secondSwapTask = secondObject.PlaySwapAnimation(firstObjectPosition);
        await Task.WhenAll(firstSwapTask, secondSwapTask);
    }

    private void SwapMatchObjectsInArray(GridCoordinates firstGridCoordinates, GridCoordinates secondGridCoordinates)
    {
        (MatchObjectsArray[firstGridCoordinates.X, firstGridCoordinates.Y],
                MatchObjectsArray[secondGridCoordinates.X, secondGridCoordinates.Y]) =
            (MatchObjectsArray[secondGridCoordinates.X, secondGridCoordinates.Y],
                MatchObjectsArray[firstGridCoordinates.X, firstGridCoordinates.Y]);
    }
}