using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int _maxHealth = 2;

    [Header("Visual")]
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private DroneSway _sway;
    [SerializeField] private float _flashTime = 0.1f;

    [Header("Efectos de muerte")]
    [SerializeField] private ParticleSystem _explosionEffect;
    [SerializeField] private ParticleSystem _smokeEffect;
    [SerializeField] private string _explosionSound = "enemy_explosion";

    [Header("Temblor de pantalla (0 = sin temblor)")]
    [SerializeField] private float _shakeIntensity = 0f;
    [SerializeField] private float _shakeDuration = 0.3f;

    [Header("Caida al morir")]
    [SerializeField] private float _fallDelay = 0.2f;
    [SerializeField] private float _fallTime = 2f;
    [SerializeField] private float _gravity = 12f;
    [SerializeField] private float _maxSpin = 300f;
    [SerializeField] private float _maxSideSpeed = 2f;

    [Header("Orden de dibujo")]
    [SerializeField] private float _depthStep = 0.01f;

    private static int _spawnCounter;

    private int _health;
    private bool _dying;
    private float _delayTimer;
    private float _fallTimer;
    private float _fallSpeed;
    private float _spin;
    private float _sideSpeed;
    private Color _normalColor = Color.white;
    private Quaternion _startRotation;
    private Collider2D[] _colliders;

    public bool IsDying
    {
        get { return _dying; }
    }

    private void Awake()
    {
        _colliders = GetComponentsInChildren<Collider2D>(true);
        _startRotation = transform.rotation;
        if (_sprite != null) _normalColor = _sprite.color;
    }

    private void OnEnable()
    {
        _spawnCounter = (_spawnCounter + 1) % 20;
        Vector3 position = transform.position;
        position.z -= _spawnCounter * _depthStep;
        transform.position = position;

        _health = _maxHealth;
        _dying = false;

        CancelInvoke();
        transform.rotation = _startRotation;

        if (_sprite != null) _sprite.color = _normalColor;
        if (_sway != null) _sway.enabled = true;

        for (int i = 0; i < _colliders.Length; i++)
        {
            _colliders[i].enabled = true;
        }

        StopEffect(_explosionEffect);
        StopEffect(_smokeEffect);
    }

    private void Update()
    {
        if (!_dying) return;

        if (_delayTimer > 0f)
        {
            _delayTimer -= Time.deltaTime;
            return;
        }

        _fallTimer -= Time.deltaTime;
        _fallSpeed += _gravity * Time.deltaTime;

        transform.Translate(new Vector3(_sideSpeed, -_fallSpeed, 0f) * Time.deltaTime, Space.World);
        transform.Rotate(0f, 0f, _spin * Time.deltaTime);

        if (_fallTimer <= 0f) gameObject.SetActive(false);
    }

    public void TakeDamage(int amount)
    {
        if (_dying) return;

        _health -= amount;

        if (_health <= 0)
        {
            Die();
            return;
        }

        Flash();
    }

    private void Flash()
    {
        if (_sprite == null) return;

        _sprite.color = Color.red;
        CancelInvoke(nameof(EndFlash));
        Invoke(nameof(EndFlash), _flashTime);
    }

    private void EndFlash()
    {
        if (_sprite != null) _sprite.color = _normalColor;
    }

    private void Die()
    {
        _dying = true;
        _delayTimer = _fallDelay;
        _fallTimer = _fallTime;
        _fallSpeed = 0f;
        _spin = Random.Range(-_maxSpin, _maxSpin);
        _sideSpeed = Random.Range(-_maxSideSpeed, _maxSideSpeed);

        for (int i = 0; i < _colliders.Length; i++)
        {
            _colliders[i].enabled = false;
        }

        if (_sway != null) _sway.enabled = false;
        if (_explosionEffect != null) _explosionEffect.Play();
        if (_smokeEffect != null) _smokeEffect.Play();
        GameAudio.PlaySfx(_explosionSound);

        if (_shakeIntensity > 0f && CameraShake.Instance != null)
        {
            CameraShake.Instance.Shake(_shakeIntensity, _shakeDuration);
        }
    }

    private void StopEffect(ParticleSystem effect)
    {
        if (effect != null) effect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }
}
