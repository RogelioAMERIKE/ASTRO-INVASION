using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

public class SettingsUI : UIWindow
{
    [Header("SettingsUI")]
    [SerializeField] private Slider _volumeSlider;
    public Button _backButton;
    public Button _creditsButton;


    public override void Initialize()
    {
        base.Initialize();
        _volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        _backButton.onClick.AddListener(ReturnToMenu);
        _creditsButton.onClick.AddListener(OpenCredits);


    }
    private void ReturnToMenu()
    {
        UIManager.Instance.HideWindow("settingsui");
        Invoke(nameof(ShowMenu), 1f);
    }

    private void ShowMenu()
    {
        UIManager.Instance.ShowWindow("menuui");
    }

    private void OpenCredits()
    {
        UIManager.Instance.HideWindow("settingsui");
        Invoke(nameof(ShowCredits), 1f);
    }

    private void ShowCredits()
    {
        UIManager.Instance.ShowWindow("creditsui");
    }
    private void OnVolumeChanged(float value)
    {
        Debug.Log($"Volume changed to: {value}");
    }

    #region Test Methods
    [Button("Test Show")]
    private void TestShow()
    {
        Show();
    }

    [Button("Test Hide")]
    private void TestHide()
    {
        Hide();
    }
    #endregion
}
