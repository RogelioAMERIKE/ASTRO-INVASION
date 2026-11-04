using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] private float _speed = 15f;
    [SerializeField] private float _lifetime = 2f;
    private float _lifeLeft;

    private void OnEnable()
    {
        _lifeLeft = _lifetime;
    }

    private void Update()
    {
        transform.Translate(Vector2.right * _speed * Time.deltaTime, Space.World);

        _lifeLeft -= Time.deltaTime;
        if (_lifeLeft <= 0f) gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Enemy enemy = other.GetComponentInParent<Enemy>();
        if (enemy == null) return;

        enemy.TakeDamage(PlayerData.BulletDamage);
        gameObject.SetActive(false);
    }

}
