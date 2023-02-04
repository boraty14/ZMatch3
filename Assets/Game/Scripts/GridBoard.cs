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
    private readonly MatchChecker _matchChecker;
    private readonly Dictionary<int, int> _columnBlastCountDictionary = new Dictionary<int, int>();
    public readonly MatchObject[,] MatchObjectsArray = new MatchObject[GridSize, GridSize];

    public GridBoard()
    {
        _matchChecker = new MatchChecker(this);
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

        await BlastMatchingObjects(matchingObjectsCoordinates);
        await MakeObjectsFallAfterBlast();




        /*if (_matchChecker.IsThereAnyMatch())
        {
            var matchingObjectsCoordinates =
                _matchChecker.GetMatchingObjectsCoordinates();
            await BlastMatchingObjects(matchingObjectsCoordinates);
            return;
        }

        await RunObjectSwapAnimationTask(firstObject, secondObject);
        SwapMatchObjectsInArray(firstGridCoordinates, secondGridCoordinates);*/
    }

    private async Task BlastMatchingObjects(List<GridCoordinates> matchingCoordinatesList)
    {
        ResetBlastColumnDictionary();
        var blastTasks = new List<Task>();
        foreach (var matchingCoordinates in matchingCoordinatesList)
        {
            _columnBlastCountDictionary[matchingCoordinates.Y]++;
            var matchingObject = GetMatchObjectFromCoordinates(matchingCoordinates);
            blastTasks.Add(matchingObject.Blast());
        }
        await Task.WhenAll(blastTasks);
    }

    private async Task MakeObjectsFallAfterBlast()
    {
        
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

    private void ResetBlastColumnDictionary()
    {
        for (int i = 0; i < GridSize; i++)
        {
            _columnBlastCountDictionary[i] = 0;
        }
    }
}