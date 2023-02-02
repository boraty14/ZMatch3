using UnityEngine;

public static class GridBoard
{
    private const float GridCellSize = 0.86f;
    private const float GridCellInterval = 0.07f;
    private const float GridCellIndexSize = GridCellSize + GridCellInterval;
    private const float GridStartingPointX = -3.69f;
    private const float GridStartingPointY = -6f;
    private const int GridSize = 8;

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

    public static bool IsTouchingGrid(Vector3 worldPoint)
    {
        var xOffset = worldPoint.x - GridStartingPointX;
        var yOffset = worldPoint.y - GridStartingPointY;
        if (xOffset < 0f || yOffset < 0f) return false;
        if (xOffset / GridCellIndexSize > GridSize || yOffset / GridCellIndexSize > GridSize) return false;

        return true;
    }
}