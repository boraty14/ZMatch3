using System.Collections.Generic;
using UnityEngine;

public class MatchChecker
{
    private readonly GridBoard _gridBoard;
    private MatchObjectType _checkType;
    private const int MinimumMatchCount = 3;

    private static readonly Vector2Int[] VerticalDirections = new Vector2Int[] { Vector2Int.up, Vector2Int.down };
    private static readonly Vector2Int[] HorizontalDirections = new Vector2Int[] { Vector2Int.left, Vector2Int.right };

    public MatchChecker(GridBoard gridBoard)
    {
        _gridBoard = gridBoard;
    }

    public void CheckMatches(GridCoordinates firstCoordinates, GridCoordinates secondCoordinates)
    {
        CheckMatchObject(firstCoordinates);
        CheckMatchObject(secondCoordinates);
    }

    private void CheckMatchObject(GridCoordinates gridCoordinates)
    {
        var matchObject = _gridBoard.MatchObjectsArray[gridCoordinates.X, gridCoordinates.Y];
        _checkType = matchObject.GetMatchObjectType();
        var verticalMatchList =  GetDirectionMatchList(gridCoordinates,VerticalDirections);
        var horizontalMatchList =  GetDirectionMatchList(gridCoordinates,HorizontalDirections);

        var isVerticalMatch = verticalMatchList.Count >= MinimumMatchCount - 1;
        var isHorizontalMatch = horizontalMatchList.Count >= MinimumMatchCount - 1;
        
        if(!isVerticalMatch && !isHorizontalMatch)  return;
        
        if (isVerticalMatch) BlastObjectsInDirection(verticalMatchList);
        if (isHorizontalMatch) BlastObjectsInDirection(horizontalMatchList);
        BlastSingleObject(gridCoordinates);

    }

    private List<GridCoordinates> GetDirectionMatchList(GridCoordinates gridCoordinates, IEnumerable<Vector2Int> directions)
    {
        var matchCoordinatesList = new List<GridCoordinates>();
        var startingCoordinates = gridCoordinates;
        foreach (var direction in directions)
        {
            gridCoordinates = startingCoordinates;
            while (true)
            {
                Debug.Log("before " + gridCoordinates);
                gridCoordinates.ApplyDirection(direction);
                Debug.Log("after " + gridCoordinates);
                if (!IsObjectAtCoordinatesMatching(gridCoordinates)) break;
                Debug.Log("added " + gridCoordinates);
                matchCoordinatesList.Add(gridCoordinates);
            }
        }
        return matchCoordinatesList;
    }

    private bool IsObjectAtCoordinatesMatching(GridCoordinates gridCoordinates)
    {
        if (!IsIndexValid(gridCoordinates)) return false;
        
        var checkObject = _gridBoard.MatchObjectsArray[gridCoordinates.X, gridCoordinates.Y];
        return checkObject.IsType(_checkType);
    }

    private static bool IsIndexValid(GridCoordinates gridCoordinates)
    {
        return gridCoordinates.X >= 0 && gridCoordinates.X < GridBoard.GridSize &&
               gridCoordinates.Y >= 0 && gridCoordinates.Y < GridBoard.GridSize;
    }

    private void BlastObjectsInDirection(IEnumerable<GridCoordinates> gridCoordinatesList)
    {
        foreach (var gridCoordinates in gridCoordinatesList)
        {
            Debug.Log(11);
            BlastSingleObject(gridCoordinates);
        }
    }

    private void BlastSingleObject(GridCoordinates gridCoordinates)
    {
        EventBus.OnBlastObject?.Invoke(gridCoordinates);
    }
}