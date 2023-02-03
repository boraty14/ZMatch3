using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    private MatchObject _firstMatchObject;
    private GridBoard _gridBoard;

    public void Initialize(GridBoard gridBoard)
    {
        _gridBoard = gridBoard;
    }

    private void Update()
    {
        var isJustPressed = Input.GetMouseButtonDown(0);
        var isPressing = Input.GetMouseButton(0);
        if(!isJustPressed && !isPressing) return;
        
        var mousePos = Input.mousePosition;
        var worldPoint = _camera.ScreenToWorldPoint(mousePos);
        if(!GridBoard.IsTouchingGrid(worldPoint)) return;
        
        var gridCoordinates = GridBoard.GetGridCoordinatesFromWorldPoint(worldPoint);
        var pressedMatchObject = _gridBoard.MatchObjectsArray[gridCoordinates.X, gridCoordinates.Y];
        
        if (isJustPressed)
        {
            if (_firstMatchObject is null)
            {
                _firstMatchObject = pressedMatchObject;
                _firstMatchObject.SetObjectSelectedState(true);
                return;
            }

            if (_firstMatchObject == pressedMatchObject)
            {
                ReleaseFirstMatchObject();
                return;
            }
            CheckSwap(_firstMatchObject,pressedMatchObject);
            return;
        }
        
        if (isPressing)
        {
            if(_firstMatchObject is null || _firstMatchObject == pressedMatchObject) return;
            CheckSwap(_firstMatchObject,pressedMatchObject);
        }
    }

    private void CheckSwap(MatchObject firstMatchObject,MatchObject secondMatchObject)
    {
        var firstGridCoordinates = GridBoard.GetGridCoordinatesFromMatchObject(firstMatchObject);
        var secondGridCoordinates = GridBoard.GetGridCoordinatesFromMatchObject(secondMatchObject);
        if (GridCoordinates.GetTotalDistance(firstGridCoordinates, secondGridCoordinates) != 1)
        {
            ReleaseFirstMatchObject();
            return;
        }
        Swap(firstMatchObject,secondMatchObject);
    }

    private void Swap(MatchObject firstMatchObject,MatchObject secondMatchObject)
    {
        secondMatchObject.SetObjectSelectedState(true);
        _gridBoard.SwapMatchObjects(firstMatchObject,secondMatchObject);
        _firstMatchObject = null;
    }

    private void ReleaseFirstMatchObject()
    {
        _firstMatchObject.SetObjectSelectedState(false);
        _firstMatchObject = null;
    }



}