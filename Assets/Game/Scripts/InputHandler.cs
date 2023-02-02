using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    private MatchObject _firstMatchObject;

    private void Update()
    {
        var mousePos = Input.mousePosition;
        var worldPoint = _camera.ScreenToWorldPoint(mousePos);
        Debug.Log("start");
        if(!GridBoard.IsTouchingGrid(worldPoint)) return;
        Debug.Log("touches grid");
        
        var gridCoordinates = GridBoard.GetGridCoordinatesFromWorldPoint(worldPoint);
        var pressedMatchObject = GridBoard.MatchObjectsArray[gridCoordinates.X, gridCoordinates.Y];
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("a");
            if (_firstMatchObject is null)
            {
                Debug.Log(1);
                _firstMatchObject = pressedMatchObject;
                _firstMatchObject.SetObjectSelectedState(true);
                return;
            }
            Debug.Log("b");
            CheckSwap(ref pressedMatchObject);
            return;
        }
        if (Input.GetMouseButton(0))
        {
            CheckSwap(ref pressedMatchObject);
        }
    }

    private void CheckSwap(ref MatchObject lastPressedMatchObject)
    {
        if(_firstMatchObject is null || _firstMatchObject == lastPressedMatchObject) return;
        lastPressedMatchObject.SetObjectSelectedState(true);
        GridBoard.SwapMatchObjects(ref _firstMatchObject,ref lastPressedMatchObject);
        _firstMatchObject = null;
    }



}