using UnityEngine;

public class BigEnemy : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _fireInterval = 2f;
    [SerializeField] private float _bulletSpeed = 8f;
    [SerializeField] private float _bulletLifetime = 6f;

    [Header("Efecto y retroceso al disparar")]
    [SerializeField] private ParticleSystem _shootEffect;
    [SerializeField] private float _recoilDistance = 0.3f;
    [SerializeField] private float _recoilTime = 0.2f;
    [SerializeField] private string _shootSound = "enemy_shoot";

    private ObjectPool _bulletPool;
    private Enemy _enemy;
    private float _timer;
    private float _recoilTimer;
    private float _lastRecoil;

    private void Awake()
    {
        _bulletPool = new ObjectPool(_bulletPrefab, 5);
        _enemy = GetComponent<Enemy>();
    }

    private void OnEnable()
    {
        _timer = _fireInterval;
        _recoilTimer = 0f;
        _lastRecoil = 0f;

        if (_shootEffect != null) _shootEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    private void Update()
    {
        UpdateRecoil();

        if (_enemy != null && _enemy.IsDying) return;

        _timer -= Time.deltaTime;
        if (_timer > 0f) return;

        _timer = _fireInterval;
        Fire();
    }

    private void Fire()
    {
        if (_animator != null) _animator.SetTrigger("Shoot");
        if (_shootEffect != null) _shootEffect.Play();
        GameAudio.PlaySfx(_shootSound);
        if (_recoilTime > 0f) _recoilTimer = _recoilTime;

        Vector3 origin = transform.position;
        if (_firePoint != null) origin = _firePoint.position;

        GameObject bullet = _bulletPool.Get(origin);

        ScrollingObject mover = bullet.GetComponent<ScrollingObject>();
        if (mover != null) mover.Initialize(_bulletSpeed, _bulletLifetime);
    }

    private void UpdateRecoil()
    {
        if (_recoilTimer <= 0f) return;

        _recoilTimer -= Time.deltaTime;

        float offset = 0f;
        if (_recoilTimer > 0f)
        {
            float progress = 1f - _recoilTimer / _recoilTime;
            offset = Mathf.Sin(progress * Mathf.PI) * _recoilDistance;
        }

        transform.Translate(Vector3.right * (offset - _lastRecoil), Space.World);
        _lastRecoil = offset;
    }
}
