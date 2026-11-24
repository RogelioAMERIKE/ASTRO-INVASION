using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class UIFlow : MonoBehaviour
{
    private const float BusyTimeout = 3f;
    private const float ShowLockTime = 0.7f;
    private const float HideRetryTime = 0.55f;

    private static float _busyUntil;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStatics()
    {
        _busyUntil = 0f;
    }

    public static bool IsBusy
    {
        get { return Time.unscaledTime < _busyUntil; }
    }

    public static bool Begin()
    {
        if (IsBusy) return false;

        _busyUntil = Time.unscaledTime + BusyTimeout;
        return true;
    }

    public static UIWindow FindWindow(string id)
    {
        if (UIManager.Instance == null) return null;

        List<UIWindow> windows = UIManager.Instance.UIWindows;
        for (int i = 0; i < windows.Count; i++)
        {
            if (windows[i] != null && windows[i].Id == id) return windows[i];
        }

        Debug.LogError("UIFlow: no existe la ventana '" + id + "'");
        return null;
    }

    public static void Show(string id)
    {
        UIWindow window = FindWindow(id);
        if (window == null) return;

        window.CanvasGroup.transform.DOKill();
        window.Show();

        _busyUntil = Time.unscaledTime + ShowLockTime;
        UIClickGuard.Lock(ShowLockTime);
    }

    public static void Hide(string id)
    {
        UIWindow window = FindWindow(id);
        if (window == null) return;

        window.CanvasGroup.transform.DOComplete();
        window.Hide();
        DOVirtual.DelayedCall(HideRetryTime, () => HideAgain(id), true);
    }

    private static void HideAgain(string id)
    {
        UIWindow window = FindWindow(id);
        if (window == null) return;

        window.CanvasGroup.transform.DOComplete();
        window.Hide();
    }
}
