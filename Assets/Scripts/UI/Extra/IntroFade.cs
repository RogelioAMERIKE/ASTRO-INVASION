using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class IntroFade : MonoBehaviour
{
    [Header("Imagen negra vieja (opcional, se apaga sola)")]
    [SerializeField] private Image _blackImage;

    [Header("Tiempos")]
    [SerializeField] private float _blackTime = 2f;
    [SerializeField] private float _fadeTime = 2f;
    [SerializeField] private string _firstWindow = "titleui";

    private const float MaxFrameTime = 0.1f;
    private const int OverlayOrder = 31999;

    private GameObject _overlay;
    private Image _overlayImage;
    private float _timer;
    private bool _titleChecked;
    private float _verifyTime = -1f;

    private void Awake()
    {
        Time.timeScale = 1f;

        DOTween.Init();
        DOTween.defaultTimeScaleIndependent = true;

        if (_blackImage != null) _blackImage.gameObject.SetActive(false);

        CreateOverlay();
    }

    private void Start()
    {
        UIClickGuard.Lock(_blackTime + _fadeTime);
    }

    private void Update()
    {
        _timer += Mathf.Min(Time.unscaledDeltaTime, MaxFrameTime);

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

    private void CreateOverlay()
    {
        _overlay = new GameObject("IntroBlackOverlay");

        Canvas canvas = _overlay.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = OverlayOrder;

        GameObject imageObject = new GameObject("Black");
        imageObject.transform.SetParent(_overlay.transform, false);

        _overlayImage = imageObject.AddComponent<Image>();
        _overlayImage.color = Color.black;
        _overlayImage.raycastTarget = false;

        RectTransform rect = _overlayImage.rectTransform;
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
    }

    private void UpdateFade()
    {
        if (_overlay == null) return;

        float progress = 1f;
        if (_fadeTime > 0f) progress = (_timer - _blackTime) / _fadeTime;
        progress = Mathf.Clamp01(progress);

        Color color = _overlayImage.color;
        color.a = 1f - progress;
        _overlayImage.color = color;

        if (progress >= 1f)
        {
            Destroy(_overlay);
            _overlay = null;
        }
    }

    private void ShowFirstWindow()
    {
        UIWindow title = UIFlow.FindWindow(_firstWindow);
        if (title == null) return;

        CloseOtherWindows(title);

        title.CanvasRectTransform.localScale = Vector3.one;
        title.CanvasGroup.transform.localScale = Vector3.zero;

        UIFlow.Show(_firstWindow);
        _verifyTime = _timer + 1.5f;
    }

    private void CloseOtherWindows(UIWindow ignore)
    {
        List<UIWindow> windows = UIManager.Instance.UIWindows;

        for (int i = 0; i < windows.Count; i++)
        {
            UIWindow window = windows[i];
            if (window == null || window == ignore) continue;
            if (window.CanvasRectTransform == null || window.CanvasGroup == null) continue;
            if (!window.CanvasRectTransform.gameObject.activeSelf) continue;

            window.CanvasGroup.transform.DOComplete();
            window.Hide(true);
        }
    }

    private void VerifyFirstWindow()
    {
        if (UIFlow.IsBusy) return;

        UIWindow title = UIFlow.FindWindow(_firstWindow);
        if (title == null) return;

        bool visible = title.CanvasRectTransform.gameObject.activeSelf
            && title.CanvasGroup.transform.localScale.x > 0.99f;
        if (visible) return;

        title.CanvasGroup.transform.DOComplete();
        title.CanvasRectTransform.gameObject.SetActive(true);
        title.CanvasGroup.transform.localScale = Vector3.one;
    }
}
