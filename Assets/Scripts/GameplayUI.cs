using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

public class GameplayUI : UIWindow
{
    public Button _pauseButton;

    public override void Initialize()
    {
        base.Initialize();
        _pauseButton.onClick.AddListener(OpenPauseUI);
    }

    private void OpenPauseUI()
    {
        UIManager.Instance.HideWindow("gameplayui");
        Invoke(nameof(ShowPauseUI), 1f);
    }

    private void ShowPauseUI()
    {
        UIManager.Instance.ShowWindow("pauseui");
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
