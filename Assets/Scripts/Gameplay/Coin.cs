using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int _value = 1;
    [SerializeField] private string _collectSound = "coin";

    public void Collect()
    {
        if (GameManager.Instance != null) GameManager.Instance.AddCoins(_value);
        GameAudio.PlaySfx(_collectSound);
        gameObject.SetActive(false);
    }
}
