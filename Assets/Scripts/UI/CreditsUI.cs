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
        if (!UIFlow.Begin()) return;

        UIFlow.Hide("creditsui");
        Invoke(nameof(OpenSettings), 1f);
    }

    private void OpenSettings()
    {
        UIFlow.Show("settingsui");
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
