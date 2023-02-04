using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private MatchObjectSpawner _matchObjectSpawner;
    private MatchObject _firstMatchObject;
    private GridBoard _gridBoard;
    private bool _isSwapping;

    public void Initialize(GridBoard gridBoard)
    {
        _gridBoard = gridBoard;
    }

    private void Update()
    {
        if(_isSwapping) return;
        
        var isJustPressed = Input.GetMouseButtonDown(0);
        var isPressing = Input.GetMouseButton(0);
        if(!isJustPressed && !isPressing) return;
        
        var mousePos = Input.mousePosition;
        var worldPoint = _camera.ScreenToWorldPoint(mousePos);
        if(!GridBoard.IsTouchingGrid(worldPoint)) return;
        
        var gridCoordinates = GridBoard.GetGridCoordinatesFromWorldPoint(worldPoint);
        var pressedMatchObject = _gridBoard.GetMatchObjectFromCoordinates(gridCoordinates);
        
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
        secondMatchObject.SetObjectSelectedState(true);
        Swap(firstMatchObject,secondMatchObject);
    }

    private async void Swap(MatchObject firstMatchObject,MatchObject secondMatchObject)
    {
        _isSwapping = true;
        await _gridBoard.SwapMatchObjects(firstMatchObject,secondMatchObject);
        //TODO also wait for the fall
        _isSwapping = false;
        _firstMatchObject = null;
    }

    private void ReleaseFirstMatchObject()
    {
        _firstMatchObject.SetObjectSelectedState(false);
        _firstMatchObject = null;
    }
}