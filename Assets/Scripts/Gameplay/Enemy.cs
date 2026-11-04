using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int _maxHealth = 2;

    private int _health;

    private void OnEnable()
    {
        _health = _maxHealth;
    }

    public void TakeDamage(int amount)
    {
        _health -= amount;
        if (_health <= 0) gameObject.SetActive(false);
    }
}
