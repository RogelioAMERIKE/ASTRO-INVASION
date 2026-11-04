using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

public class CreditsUI : UIWindow
{
    public Button _backButton;

    public override void Initialize()
    {
        base.Initialize();
        _backButton.onClick.AddListener(ReturnToSettings);
    }

    private void ReturnToSettings()
    {
        UIManager.Instance.HideWindow("creditsui");
        Invoke(nameof(OpenSettings), 1f);
    }

    private void OpenSettings()
    {
        UIManager.Instance.ShowWindow("settingsui");
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
