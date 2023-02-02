using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchObjectSpawner : PoolerBase<MatchObject>
{
    [SerializeField] private MatchObject _matchObjectPrefab;

    private void Start()
    {
        InitPool(_matchObjectPrefab);
    }
}
