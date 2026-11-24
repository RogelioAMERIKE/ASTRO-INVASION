using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseUI : UIWindow
{
    public Button _resumeButton;
    public Button _retryButton;
    public Button _menuButton;

    public override void Initialize()
    {
        base.Initialize();

        _resumeButton.onClick.AddListener(ReturnToGameplay);
        _retryButton.onClick.AddListener(Retry);
        _menuButton.onClick.AddListener(ReturnToMenu);
    }

    private void ReturnToGameplay()
    {
        if (!UIFlow.Begin()) return;

        UIFlow.Hide("pauseui");
        DOVirtual.DelayedCall(1f, ShowGameplayUI, true);
    }

    private void ShowGameplayUI()
    {
        UIFlow.Show("gameplayui");
        if (GameManager.Instance != null) GameManager.Instance.Resume();
    }

    private void Retry()
    {
        if (!UIFlow.Begin()) return;

        UIFlow.Hide("pauseui");
        DOVirtual.DelayedCall(1f, ReloadGameplay, true);
    }

    private void ReloadGameplay()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Gameplay");
        UIFlow.Show("gameplayui");
    }

    private void ReturnToMenu()
    {
        if (!UIFlow.Begin()) return;

        UIFlow.Hide("pauseui");
        DOVirtual.DelayedCall(1f, LoadIntroAndOpenMenu, true);
    }

    private void LoadIntroAndOpenMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Intro");
        UIFlow.Show("menuui");
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
