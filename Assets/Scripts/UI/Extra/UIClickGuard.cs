using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public static class UIClickGuard
{
    public static void Lock(float time)
    {
        UIClickBlocker.Lock(time);
    }
}
