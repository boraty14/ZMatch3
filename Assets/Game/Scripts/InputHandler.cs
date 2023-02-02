using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    

    private void Update()
    {
        if(!Input.GetMouseButtonDown(0)) return;
        var mousePos = Input.mousePosition;
        var worldPoint = _camera.ScreenToWorldPoint(mousePos);
        Debug.Log(worldPoint);
        var isTouching = GridBoard.IsTouchingGrid(worldPoint);
        Debug.Log(isTouching);
        if (!isTouching) return;
        Debug.Log(GridBoard.GetGridCoordinatesFromWorldPoint(worldPoint));
    }
}
