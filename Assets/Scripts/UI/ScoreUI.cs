using DG.Tweening;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreUI : UIWindow
{
    public Button _retryButton;
    public Button _menuButton;

    [Header("Texts")]
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _roundCoinsText;

    public override void Initialize()
    {
        base.Initialize();

        _retryButton.onClick.AddListener(Retry);
        _menuButton.onClick.AddListener(ReturnToMenu);

        GameManager.OnGameOver += ShowResults;
    }

    private void OnDestroy()
    {
        GameManager.OnGameOver -= ShowResults;
    }

    private void ShowResults(int score, int roundCoins)
    {
        if (_scoreText != null) _scoreText.text = score.ToString();
        if (_roundCoinsText != null) _roundCoinsText.text = roundCoins.ToString();
    }

    private void Retry()
    {
        if (!UIFlow.Begin()) return;

        UIFlow.Hide("scoreui");
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

        UIFlow.Hide("scoreui");
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
