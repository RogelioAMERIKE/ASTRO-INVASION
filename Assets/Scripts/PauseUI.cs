using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseUI : UIWindow
{
    public Button _resumeButton;
    public Button _menuButton;

    public override void Initialize()
    {
        base.Initialize();

        _resumeButton.onClick.AddListener(ReturnToGameplay);
        _menuButton.onClick.AddListener(ReturnToMenu);
    }

    private void ReturnToGameplay()
    {
        UIManager.Instance.HideWindow("pauseui");
        Invoke(nameof(ShowGameplayUI), 1f);
    }

    private void ShowGameplayUI()
    {
        UIManager.Instance.ShowWindow("gameplayui");
    }

    private void ReturnToMenu()
    {
        UIManager.Instance.HideWindow("pauseui");
        Invoke(nameof(LoadIntroAndOpenMenu), 1f);
    }

    private void LoadIntroAndOpenMenu()
    {
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
