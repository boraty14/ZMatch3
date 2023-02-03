using System;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class TryMe : MonoBehaviour
{
    private async void Start()
    {
        var wait = TryIt();
        Debug.Log(2);
        await wait;
        Debug.Log(4);
    }

    private async Task TryIt()
    {
        await transform.DOMove(Vector3.zero, 1f).AsyncWaitForCompletion();
        Debug.Log(3);
    }
}
