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
    private static readonly bool[,] CheckedCoordinatesList = new bool[GridBoard.GridSize, GridBoard.GridSize];
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

    private bool IsThereAnyMatchInAxis(GridCoordinates gridCoordinates, Vector2Int[] axisDirections)
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

    public List<GridCoordinates> GetMatchingObjectsCoordinates()
    {
        ResetCheckStates();
        for (int i = 0; i < GridBoard.GridSize; i++)
        {
            for (int j = 0; j < GridBoard.GridSize; j++)
            {
                if(CheckedCoordinatesList[j,i]) continue;
                var objectCoordinates = new GridCoordinates { X = j, Y = i };
                if (IsObjectCreatingMatch(objectCoordinates))
                {
                    SetMatchesOfObject(objectCoordinates);
                }
            }
        }
        return _matchingCoordinatesList;
    }

    private void SetMatchesOfObject(GridCoordinates gridCoordinates)
    {
        var matchObject = _gridBoard.GetMatchObjectFromCoordinates(gridCoordinates);
        _checkType = matchObject.GetMatchObjectType();
        var isVerticalMatch = IsThereAnyMatchInAxis(gridCoordinates, VerticalAxisDirections);
        var isHorizontalMatch = IsThereAnyMatchInAxis(gridCoordinates, HorizontalAxisDirections);
        if (!isVerticalMatch && !isHorizontalMatch) return;
        
        if(isVerticalMatch) SetMatchesInAxis(gridCoordinates, VerticalAxisDirections);
        if(isHorizontalMatch) SetMatchesInAxis(gridCoordinates, HorizontalAxisDirections);
        AddCoordinatesToMatchList(gridCoordinates);
    }

    private void SetMatchesInAxis(GridCoordinates gridCoordinates, Vector2Int[] axisDirections)
    {
        var startingCoordinates = gridCoordinates;
        foreach (var direction in axisDirections)
        {
            gridCoordinates = startingCoordinates;
            while (true)
            {
                gridCoordinates.ApplyDirection(direction);
                if (!IsObjectAtCoordinatesMatching(gridCoordinates)) break;
                AddCoordinatesToMatchList(gridCoordinates);
            }
        }
    }

    private void AddCoordinatesToMatchList(GridCoordinates gridCoordinates)
    {
        if (CheckedCoordinatesList[gridCoordinates.X, gridCoordinates.Y]) return;
        var matchObject = _gridBoard.GetMatchObjectFromCoordinates(gridCoordinates);
        matchObject.ResetFallCount();
        _matchingCoordinatesList.Add(gridCoordinates);
        CheckedCoordinatesList[gridCoordinates.X, gridCoordinates.Y] = true;
    }

    private void ResetCheckStates()
    {
        for (int i = 0; i < GridBoard.GridSize; i++)
        {
            for (int j = 0; j < GridBoard.GridSize; j++)
            {
                CheckedCoordinatesList[j, i] = false;
            }   
        }
        _matchingCoordinatesList.Clear();
        
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