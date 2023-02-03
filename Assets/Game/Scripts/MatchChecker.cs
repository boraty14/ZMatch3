using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MatchChecker
{
    private readonly GridBoard _gridBoard;
    private MatchObjectType _checkType;
    private readonly List<GridCoordinates> _matchingCoordinatesList = new List<GridCoordinates>();
    private static readonly Vector2Int[] VerticalAxisDirections = { Vector2Int.up, Vector2Int.down };
    private static readonly Vector2Int[] HorizontalAxisDirections = { Vector2Int.left, Vector2Int.right };
    private const int MinimumMatchCount = 3;

    public MatchChecker(GridBoard gridBoard)
    {
        _gridBoard = gridBoard;
    }

    public bool IsThereAnyMatch(GridCoordinates firstCoordinates, GridCoordinates secondCoordinates)
    {
        return IsObjectCreatingMatch(firstCoordinates) || IsObjectCreatingMatch(secondCoordinates);
    }

    private bool IsObjectCreatingMatch(GridCoordinates gridCoordinates)
    {
        var matchObject = _gridBoard.GetMatchObjectFromCoordinates(gridCoordinates);
        _checkType = matchObject.GetMatchObjectType();
        var isVerticalMatch = IsThereAnyMatchInAxis(gridCoordinates, VerticalAxisDirections);
        var isHorizontalMatch = IsThereAnyMatchInAxis(gridCoordinates, HorizontalAxisDirections);
        return isVerticalMatch || isHorizontalMatch;
    }

    private bool IsThereAnyMatchInAxis(GridCoordinates gridCoordinates, IEnumerable<Vector2Int> axisDirections)
    {
        int matchCount = 0;
        var startingCoordinates = gridCoordinates;
        foreach (var direction in axisDirections)
        {
            gridCoordinates = startingCoordinates;
            while (true)
            {
                gridCoordinates.ApplyDirection(direction);
                if (!IsObjectAtCoordinatesMatching(gridCoordinates)) break;
                matchCount++;
            }
        }
        return matchCount >= MinimumMatchCount - 1;
    }

    public async void BlastMatchingObjects(GridCoordinates firstCoordinates, GridCoordinates secondCoordinates)
    {
        SetMatchesOfObject(firstCoordinates);
        SetMatchesOfObject(secondCoordinates);
        var blastTasks = new List<Task>();
        foreach (var matchingCoordinates in _matchingCoordinatesList)
        {
            var matchingObject = _gridBoard.GetMatchObjectFromCoordinates(matchingCoordinates);
            blastTasks.Add(matchingObject.Blast());
        }
        await Task.WhenAll(blastTasks);
    }

    private void SetMatchesOfObject(GridCoordinates gridCoordinates)
    {
        var matchObject = _gridBoard.GetMatchObjectFromCoordinates(gridCoordinates);
        _checkType = matchObject.GetMatchObjectType();
        SetMatchesInAxis(gridCoordinates, VerticalAxisDirections);
        SetMatchesInAxis(gridCoordinates, HorizontalAxisDirections);
        _matchingCoordinatesList.Add(gridCoordinates);
    }

    private void SetMatchesInAxis(GridCoordinates gridCoordinates, IEnumerable<Vector2Int> axisDirections)
    {
        var startingCoordinates = gridCoordinates;
        foreach (var direction in axisDirections)
        {
            gridCoordinates = startingCoordinates;
            while (true)
            {
                gridCoordinates.ApplyDirection(direction);
                if (!IsObjectAtCoordinatesMatching(gridCoordinates)) break;
                _matchingCoordinatesList.Add(gridCoordinates);
            }
        }
    }

    private bool IsObjectAtCoordinatesMatching(GridCoordinates gridCoordinates)
    {
        if (!IsIndexValid(gridCoordinates)) return false;

        var checkObject = _gridBoard.GetMatchObjectFromCoordinates(gridCoordinates);
        return checkObject.IsType(_checkType);
    }

    private static bool IsIndexValid(GridCoordinates gridCoordinates)
    {
        return gridCoordinates.X >= 0 && gridCoordinates.X < GridBoard.GridSize &&
               gridCoordinates.Y >= 0 && gridCoordinates.Y < GridBoard.GridSize;
    }
}