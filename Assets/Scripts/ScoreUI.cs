using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreUI : UIWindow
{
    public Button _menuButton;

    public override void Initialize()
    {
        base.Initialize();
        _menuButton.onClick.AddListener(ReturnToMenu);
    }

    private void ReturnToMenu()
    {
        UIManager.Instance.HideWindow("scoreui");
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
