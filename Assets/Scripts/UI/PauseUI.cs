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
        UIManager.Instance.HideWindow("pauseui");
        DOVirtual.DelayedCall(1f, ShowGameplayUI, true);
    }

    private void ShowGameplayUI()
    {
        UIManager.Instance.ShowWindow("gameplayui");
        if (GameManager.Instance != null) GameManager.Instance.Resume();
    }

    private void Retry()
    {
        UIManager.Instance.HideWindow("pauseui");
        DOVirtual.DelayedCall(1f, ReloadGameplay, true);
    }

    private void ReloadGameplay()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Gameplay");
        UIManager.Instance.ShowWindow("gameplayui");
    }

    private void ReturnToMenu()
    {
        UIManager.Instance.HideWindow("pauseui");
        DOVirtual.DelayedCall(1f, LoadIntroAndOpenMenu, true);
    }

    private void LoadIntroAndOpenMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Intro");
        UIManager.Instance.ShowWindow("menuui");
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
