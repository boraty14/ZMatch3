using System.Collections.Generic;
using UnityEngine;

public class MatchChecker
{
    private readonly GridBoard _gridBoard;
    private MatchObjectType _checkType;
    private List<GridCoordinates> _matchingObjectsCoordinatesList;

    public MatchChecker(GridBoard gridBoard)
    {
        _gridBoard = gridBoard;
        _matchingObjectsCoordinatesList = new List<GridCoordinates>();
    }
    
    public void CheckMatches(MatchObject firstObject, MatchObject secondObject)
    {
        CheckMatchObject(firstObject);
        CheckMatchObject(secondObject);
    }

    private void CheckMatchObject(MatchObject matchObject)
    {
        var matchObjectCoordinates = GridBoard.GetGridCoordinatesFromMatchObject(matchObject);
        _checkType = matchObject.GetMatchObjectType();
        CheckMatch(matchObjectCoordinates);
    }

    private void CheckMatch(GridCoordinates gridCoordinates)
    {
        CheckDirectionForMatch(gridCoordinates, Vector2Int.up);
        CheckDirectionForMatch(gridCoordinates, Vector2Int.down);
        CheckDirectionForMatch(gridCoordinates, Vector2Int.left);
        CheckDirectionForMatch(gridCoordinates, Vector2Int.right);
    }

    private void CheckDirectionForMatch(GridCoordinates gridCoordinates, Vector2Int direction)
    {
        while (true)
        {
            gridCoordinates.ApplyDirection(direction);
            if (!IsIndexValid(gridCoordinates)) return;

            var checkObject = _gridBoard.MatchObjectsArray[gridCoordinates.X, gridCoordinates.Y];
            if (!checkObject.IsType(_checkType)) return;

            _matchingObjectsCoordinatesList.Add(gridCoordinates);
        }
    }

    private static bool IsIndexValid(GridCoordinates gridCoordinates)
    {
        return gridCoordinates.X >= 0 && gridCoordinates.X < GridBoard.GridSize &&
               gridCoordinates.Y >= 0 && gridCoordinates.Y < GridBoard.GridSize;
    }

    private void BlastAllObjects()
    {
        
    }

    private void BlastMatchObject(GridCoordinates gridCoordinates)
    {
        
    }
}