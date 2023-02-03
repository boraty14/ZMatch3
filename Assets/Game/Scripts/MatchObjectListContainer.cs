using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchObjectListContainer
{
    public readonly List<GridCoordinates> _verticalMatchList = new List<GridCoordinates>();
    public readonly List<GridCoordinates> _horizontalMatchList = new List<GridCoordinates>();

    public void ResetLists()
    {
        _verticalMatchList.Clear();
        _horizontalMatchList.Clear();
    }
}