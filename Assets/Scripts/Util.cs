using System;
using System.Collections;
using UnityEngine;

public static class Util
{
    public static IEnumerator DelayedCall(float time, Action func)
    {
        yield return new WaitForSeconds(time);
        func?.Invoke();
    }

    // onUpdate param: t (0 to 1)
    public static IEnumerator ContinuousCall(float time, Action<float> onUpdate, Action onEnd)
    {
        float startTime = Time.time;
        while (Time.time < startTime + time)
        {
            onUpdate?.Invoke((Time.time - startTime) / time);
            yield return new WaitForEndOfFrame();
        }
        onEnd?.Invoke();
    }
}