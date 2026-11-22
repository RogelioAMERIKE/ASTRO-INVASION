using UnityEngine;

public class BigEnemy : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _fireInterval = 2f;
    [SerializeField] private float _bulletSpeed = 8f;
    [SerializeField] private float _bulletLifetime = 6f;

    private ObjectPool _bulletPool;
    private Enemy _enemy;
    private float _timer;

    private void Awake()
    {
        _bulletPool = new ObjectPool(_bulletPrefab, 5);
        _enemy = GetComponent<Enemy>();
    }

    private void OnEnable()
    {
        _timer = _fireInterval;
    }

    private void Update()
    {
        if (_enemy != null && _enemy.IsDying) return;

        _timer -= Time.deltaTime;
        if (_timer > 0f) return;

        _timer = _fireInterval;
        Fire();
    }

    private void Fire()
    {
        if (_animator != null) _animator.SetTrigger("Shoot");

        Vector3 origin = transform.position;
        if (_firePoint != null) origin = _firePoint.position;

        GameObject bullet = _bulletPool.Get(origin);

        ScrollingObject mover = bullet.GetComponent<ScrollingObject>();
        if (mover != null) mover.Initialize(_bulletSpeed, _bulletLifetime);
    }
}
