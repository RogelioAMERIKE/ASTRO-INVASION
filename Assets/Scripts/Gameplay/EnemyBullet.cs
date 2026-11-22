using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private ParticleSystem _hitEffect;
    [SerializeField] private float _effectTime = 0.5f;
    [SerializeField] private string _hitSound = "enemy_bullet_hit";

    private SpriteRenderer _sprite;
    private Collider2D _collider;
    private ScrollingObject _mover;
    private bool _hit;

    private void Awake()
    {
        _sprite = GetComponentInChildren<SpriteRenderer>();
        _collider = GetComponentInChildren<Collider2D>();
        _mover = GetComponent<ScrollingObject>();
    }

    private void OnEnable()
    {
        _hit = false;

        CancelInvoke();

        if (_sprite != null) _sprite.enabled = true;
        if (_collider != null) _collider.enabled = true;
        if (_mover != null) _mover.enabled = true;
        if (_hitEffect != null) _hitEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_hit) return;
        if (other.GetComponentInParent<PlayerController>() == null) return;

        Explode();
    }

    private void Explode()
    {
        _hit = true;

        if (_sprite != null) _sprite.enabled = false;
        if (_collider != null) _collider.enabled = false;
        if (_mover != null) _mover.enabled = false;
        if (_hitEffect != null) _hitEffect.Play();
        GameAudio.PlaySfx(_hitSound);

        Invoke(nameof(Deactivate), _effectTime);
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
