using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] private float _speed = 15f;
    [SerializeField] private float _lifetime = 2f;
    [SerializeField] private ParticleSystem _hitEffect;
    [SerializeField] private float _effectTime = 0.5f;
    [SerializeField] private string _hitSound = "player_bullet_hit";

    private SpriteRenderer _sprite;
    private Collider2D _collider;
    private float _lifeLeft;
    private bool _hit;

    private void Awake()
    {
        _sprite = GetComponentInChildren<SpriteRenderer>();
        _collider = GetComponentInChildren<Collider2D>();
    }

    private void OnEnable()
    {
        _lifeLeft = _lifetime;
        _hit = false;

        CancelInvoke();

        if (_sprite != null) _sprite.enabled = true;
        if (_collider != null) _collider.enabled = true;
        if (_hitEffect != null) _hitEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    private void Update()
    {
        if (_hit) return;

        transform.Translate(Vector2.right * _speed * Time.deltaTime, Space.World);

        _lifeLeft -= Time.deltaTime;
        if (_lifeLeft <= 0f) gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_hit) return;

        Enemy enemy = other.GetComponentInParent<Enemy>();
        if (enemy == null) return;

        enemy.TakeDamage(PlayerData.BulletDamage);
        Explode();
    }

    private void Explode()
    {
        _hit = true;

        if (_sprite != null) _sprite.enabled = false;
        if (_collider != null) _collider.enabled = false;
        if (_hitEffect != null) _hitEffect.Play();
        GameAudio.PlaySfx(_hitSound);

        Invoke(nameof(Deactivate), _effectTime);
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }

}
