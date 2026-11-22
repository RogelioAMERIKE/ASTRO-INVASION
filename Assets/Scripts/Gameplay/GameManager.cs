using DG.Tweening;
using System;
using UnityEngine;
using NaughtyAttributes;

public enum GameState { Playing, Paused, GameOver }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public static event Action OnStatsChanged;
    public static event Action<int, int> OnGameOver;

    [SerializeField] private PlayerController _player;
    [SerializeField] private float _uiDelay = 1f;
    [SerializeField] private float _slowDownTime = 3f;

    public PlayerController Player
    {
        get { return _player; }
    }

    public GameState State { get; private set; }

    public int Score
    {
        get { return Mathf.FloorToInt(_elapsed); }
    }

    public int RoundCoins { get; private set; }

    public int TotalCoins
    {
        get
        {
            if (_roundSaved) return PlayerData.Coins;
            return PlayerData.Coins + RoundCoins;
        }
    }

    private float _elapsed;
    private bool _roundSaved;
    private Tween _slowTween;

    private void Awake()
    {
        Instance = this;
        State = GameState.Playing;
        Time.timeScale = 1f;

        DOTween.Init();
        DOTween.defaultTimeScaleIndependent = true;
    }

    private void Start()
    {
        NotifyStats();
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
        if (_slowTween != null) _slowTween.Kill();
        Time.timeScale = 1f;
    }

    private void Update()
    {
        if (State != GameState.Playing) return;

        int before = Score;
        _elapsed += Time.deltaTime;
        if (Score != before) NotifyStats();
    }

    public void AddCoins(int amount)
    {
        if (State != GameState.Playing) return;

        RoundCoins += amount;
        NotifyStats();
    }

    public bool Pause()
    {
        if (State != GameState.Playing) return false;

        State = GameState.Paused;
        Time.timeScale = 0f;
        GameAudio.PauseMusic();
        return true;
    }

    public void Resume()
    {
        if (State != GameState.Paused) return;

        State = GameState.Playing;
        Time.timeScale = 1f;
        GameAudio.ResumeMusic();
    }

    public void PlayerDied()
    {
        if (State != GameState.Playing) return;

        State = GameState.GameOver;

        GameAudio.FadeOutMusic(_slowDownTime);
        _slowTween = DOVirtual.Float(1f, 0f, _slowDownTime, SetTimeScale).SetUpdate(true);

        PlayerData.AddCoins(RoundCoins);
        _roundSaved = true;

        NotifyStats();
        if (OnGameOver != null) OnGameOver(Score, RoundCoins);

        if (UIManager.Instance == null) return;
        UIManager.Instance.HideWindow("gameplayui");
        DOVirtual.DelayedCall(_uiDelay, ShowScoreUI, true);
    }

    private void SetTimeScale(float value)
    {
        Time.timeScale = value;
    }

    private void ShowScoreUI()
    {
        UIManager.Instance.ShowWindow("scoreui");
    }

    private void NotifyStats()
    {
        if (OnStatsChanged != null) OnStatsChanged();
    }
}
