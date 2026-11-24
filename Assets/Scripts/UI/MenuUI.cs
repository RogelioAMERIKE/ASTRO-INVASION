using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using NaughtyAttributes;

public class MenuUI : UIWindow
{
    public Button _playButton;
    public Button _shopButton;
    public Button _settingsButton;
    public Button _quitButton;

    public override void Initialize()
    {
        base.Initialize();

        _playButton.onClick.AddListener(PlayGame);
        _shopButton.onClick.AddListener(OpenShop);
        _settingsButton.onClick.AddListener(OpenSettings);
        _quitButton.onClick.AddListener(QuitGame);
    }

    private void PlayGame()
    {
        if (!UIFlow.Begin()) return;

        UIFlow.Hide("menuui");
        Invoke(nameof(LoadGameplay), 1f);
    }

    private void LoadGameplay()
    {
        SceneManager.LoadScene("Gameplay");
        UIFlow.Show("gameplayui");
    }

    private void OpenShop()
    {
        if (!UIFlow.Begin()) return;

        UIFlow.Hide("menuui");
        Invoke(nameof(ShowShop), 1f);
    }

    private void ShowShop()
    {
        UIFlow.Show("shopui");
    }

    private void OpenSettings()
    {
        if (!UIFlow.Begin()) return;

        UIFlow.Hide("menuui");
        Invoke(nameof(ShowSettings), 1f);
    }

    private void ShowSettings()
    {
        UIFlow.Show("settingsui");
    }

    private void QuitGame()
    {
        Application.Quit();
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
