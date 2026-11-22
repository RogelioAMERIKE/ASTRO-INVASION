using DG.Tweening;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int _value = 1;
    [SerializeField] private string _collectSound = "coin";

    [Header("Efecto al recolectar")]
    [SerializeField] private ParticleSystem _collectEffect;
    [SerializeField] private float _effectTime = 0.5f;

    [Header("Animacion al recolectar")]
    [SerializeField] private Transform _visual;
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _popScale = 1.6f;
    [SerializeField] private float _popTime = 0.25f;

    private Collider2D[] _colliders;
    private Vector3 _startScale = Vector3.one;
    private Color _startColor = Color.white;
    private bool _collected;

    private void Awake()
    {
        _colliders = GetComponentsInChildren<Collider2D>(true);
        if (_visual != null) _startScale = _visual.localScale;
        if (_sprite != null) _startColor = _sprite.color;
    }

    private void OnEnable()
    {
        _collected = false;

        CancelInvoke();

        if (_visual != null)
        {
            _visual.DOKill();
            _visual.localScale = _startScale;
        }

        if (_sprite != null)
        {
            _sprite.DOKill();
            _sprite.color = _startColor;
        }

        if (_animator != null) _animator.enabled = true;
        if (_collectEffect != null) _collectEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        for (int i = 0; i < _colliders.Length; i++)
        {
            _colliders[i].enabled = true;
        }
    }

    public void Collect()
    {
        if (_collected) return;
        _collected = true;

        if (GameManager.Instance != null) GameManager.Instance.AddCoins(_value);
        GameAudio.PlaySfx(_collectSound);

        for (int i = 0; i < _colliders.Length; i++)
        {
            _colliders[i].enabled = false;
        }

        if (_collectEffect != null) _collectEffect.Play();

        float hideDelay = 0f;

        if (_visual != null && _sprite != null)
        {
            if (_animator != null) _animator.enabled = false;

            _visual.DOScale(_startScale * _popScale, _popTime);
            _sprite.DOFade(0f, _popTime);
            hideDelay = _popTime;
        }

        if (_collectEffect != null) hideDelay = Mathf.Max(hideDelay, _effectTime);

        Invoke(nameof(Hide), hideDelay);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

}
