using DG.Tweening;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUI : UIWindow
{
    [Header("Buttons")]
    public Button _pauseButton;
    public Button _jumpButton;
    public Button _dropButton;
    public Button _shootButton;

    [Header("Texts")]
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _roundCoinsText;

    public override void Initialize()
    {
        base.Initialize();

        _pauseButton.onClick.AddListener(OpenPauseUI);
        _jumpButton.onClick.AddListener(OnJump);
        _dropButton.onClick.AddListener(OnDrop);
        _shootButton.onClick.AddListener(OnShoot);

        GameManager.OnStatsChanged += Refresh;
        Refresh();
    }

    private void OnDestroy()
    {
        GameManager.OnStatsChanged -= Refresh;
    }

    private void Refresh()
    {
        int score = 0;
        int round = 0;

        GameManager gm = GameManager.Instance;
        if (gm != null)
        {
            score = gm.Score;
            round = gm.RoundCoins;
        }

        if (_scoreText != null) _scoreText.text = "Score: " + score;
        if (_roundCoinsText != null) _roundCoinsText.text = "Money: " + round;
    }

    private PlayerController GetPlayer()
    {
        if (GameManager.Instance == null) return null;
        return GameManager.Instance.Player;
    }

    private void OnJump()
    {
        PlayerController player = GetPlayer();
        if (player != null) player.Jump();
    }

    private void OnDrop()
    {
        PlayerController player = GetPlayer();
        if (player != null) player.Drop();
    }

    private void OnShoot()
    {
        PlayerController player = GetPlayer();
        if (player != null) player.Shoot();
    }

    private void OpenPauseUI()
    {
        if (GameManager.Instance == null) return;
        if (!GameManager.Instance.Pause()) return;

        UIManager.Instance.HideWindow("gameplayui");
        DOVirtual.DelayedCall(1f, ShowPauseUI, true);
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
