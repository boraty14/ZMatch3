using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    private MatchObject _firstMatchObject;

    private void Update()
    {
        var mousePos = Input.mousePosition;
        var worldPoint = _camera.ScreenToWorldPoint(mousePos);
        if(!GridBoard.IsTouchingGrid(worldPoint)) return;
        
        var gridCoordinates = GridBoard.GetGridCoordinatesFromWorldPoint(worldPoint);
        var pressedMatchObject = GridBoard.MatchObjectsArray[gridCoordinates.X, gridCoordinates.Y];
        if (Input.GetMouseButtonDown(0))
        {
            if (_firstMatchObject is null)
            {
                _firstMatchObject = pressedMatchObject;
                _firstMatchObject.SetObjectSelectedState(true);
                return;
            }
            CheckSwap(_firstMatchObject,pressedMatchObject);
            return;
        }
        if (Input.GetMouseButton(0))
        {
            CheckSwap(_firstMatchObject,pressedMatchObject);
        }
    }

    private void CheckSwap(MatchObject firstMatchObject,MatchObject lastPressedMatchObject)
    {
        if(firstMatchObject is null || firstMatchObject == lastPressedMatchObject) return;
        lastPressedMatchObject.SetObjectSelectedState(true);
        GridBoard.SwapMatchObjects(firstMatchObject,lastPressedMatchObject);
        _firstMatchObject = null;
        
    }



}