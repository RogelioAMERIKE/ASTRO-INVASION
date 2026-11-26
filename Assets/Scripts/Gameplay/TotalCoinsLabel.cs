using TMPro;
using UnityEngine;

public class TotalCoinsLabel : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private string _format = "{0}";

    private void OnEnable()
    {
        PlayerData.OnCoinsChanged += Refresh;
        Refresh();
    }

    private void OnDisable()
    {
        PlayerData.OnCoinsChanged -= Refresh;
    }

    private void Refresh()
    {
        if (_text != null) _text.text = string.Format(_format, PlayerData.Coins);
    }
}
