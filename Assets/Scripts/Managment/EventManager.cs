using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager
{
    public static Action<List<Vector3>> OnStartMovementMan;
    public static Action OnManEndPointMovement;
    public static Action OnManFinish;
    public static Action OnRestartLevel;

    public static Action OnCoinTake;
    public static Action<int> OnCoinTextUpdate;
}
