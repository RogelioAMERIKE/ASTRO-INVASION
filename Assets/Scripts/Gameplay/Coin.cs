using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int _value = 1;

    public void Collect()
    {
        if (GameManager.Instance != null) GameManager.Instance.AddCoins(_value);
        gameObject.SetActive(false);
    }
}
