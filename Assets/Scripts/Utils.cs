using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static Coroutine SetTimer(this MonoBehaviour monoBehaviour, float duration, Action action)
    {
        return monoBehaviour.StartCoroutine(SetTimerCoroutine(duration, action));
    }

    static IEnumerator SetTimerCoroutine(float duration, Action action)
    {
        yield return new WaitForSeconds(duration);
        action();
    }
}
