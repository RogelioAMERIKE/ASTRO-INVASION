using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroFade : MonoBehaviour
{
    [SerializeField] private Image _blackImage;
    [SerializeField] private float _blackTime = 0.5f;
    [SerializeField] private float _fadeTime = 1.5f;
    [SerializeField] private string _firstWindow = "titleui";

    private float _timer;
    private bool _titleChecked;
    private float _verifyTime = -1f;

    private void Awake()
    {
        Time.timeScale = 1f;

        DOTween.Init();
        DOTween.defaultTimeScaleIndependent = true;

        if (_blackImage == null) return;

        SetAlpha(1f);
        _blackImage.gameObject.SetActive(true);
    }

    private void Start()
    {
        UIClickGuard.Lock(_blackTime + _fadeTime);
    }

    private void Update()
    {
        _timer += Time.unscaledDeltaTime;

        if (!_titleChecked && _timer >= Mathf.Max(_blackTime, 0.1f))
        {
            _titleChecked = true;
            ShowFirstWindow();
        }

        if (_verifyTime > 0f && _timer >= _verifyTime)
        {
            _verifyTime = -1f;
            VerifyFirstWindow();
        }

        UpdateFade();
    }

    private void UpdateFade()
    {
        if (_blackImage == null) return;
        if (!_blackImage.gameObject.activeSelf) return;

        float progress = 1f;
        if (_fadeTime > 0f) progress = (_timer - _blackTime) / _fadeTime;
        progress = Mathf.Clamp01(progress);

        SetAlpha(1f - progress);

        if (progress >= 1f) _blackImage.gameObject.SetActive(false);
    }

    private void SetAlpha(float value)
    {
        Color color = _blackImage.color;
        color.a = value;
        _blackImage.color = color;
    }

    private void ShowFirstWindow()
    {
        UIWindow title = UIFlow.FindWindow(_firstWindow);
        if (title == null) return;

        UIWindow other = FindOpenWindow(title);
        if (other != null)
        {
            Debug.LogWarning("IntroFade: no se muestra '" + _firstWindow + "' porque '" + other.Id + "' ya esta abierta");
            return;
        }

        title.CanvasRectTransform.localScale = Vector3.one;
        title.CanvasGroup.transform.localScale = Vector3.zero;

        UIFlow.Show(_firstWindow);
        _verifyTime = _timer + 1.2f;
    }

    private UIWindow FindOpenWindow(UIWindow ignore)
    {
        List<UIWindow> windows = UIManager.Instance.UIWindows;

        for (int i = 0; i < windows.Count; i++)
        {
            UIWindow window = windows[i];
            if (window == null || window == ignore) continue;
            if (window.CanvasRectTransform == null || window.CanvasGroup == null) continue;
            if (!window.CanvasRectTransform.gameObject.activeSelf) continue;
            if (window.CanvasGroup.transform.localScale.x < 0.01f) continue;

            return window;
        }

        return null;
    }

    private void VerifyFirstWindow()
    {
        if (UIFlow.IsBusy) return;

        UIWindow title = UIFlow.FindWindow(_firstWindow);
        if (title == null) return;

        bool visible = title.CanvasRectTransform.gameObject.activeSelf
            && title.CanvasGroup.transform.localScale.x > 0.99f;
        if (visible) return;

        Debug.LogWarning("IntroFade: la animacion de '" + _firstWindow + "' no termino, se fuerza");

        title.CanvasRectTransform.gameObject.SetActive(true);
        title.CanvasGroup.transform.DOKill();
        title.CanvasGroup.transform.localScale = Vector3.one;
    }
}
