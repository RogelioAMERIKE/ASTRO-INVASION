using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : UIWindow
{
    public Button _backButton;
    public Button _upgradeButton;

    [SerializeField] private TMP_Text _totalCoinsText;
    [SerializeField] private TMP_Text _priceText;
    [SerializeField] private GameObject _coinIcon;
    [SerializeField] private string _soldOutText = "AGOTADO";

    public override void Initialize()
    {
        base.Initialize();

        _backButton.onClick.AddListener(ReturnToMenu);
        _upgradeButton.onClick.AddListener(OnUpgrade);

        PlayerData.OnCoinsChanged += Refresh;
        PlayerData.OnUpgradeChanged += Refresh;
        Refresh();
    }

    private void OnDestroy()
    {
        PlayerData.OnCoinsChanged -= Refresh;
        PlayerData.OnUpgradeChanged -= Refresh;
    }

    private void OnUpgrade()
    {
        PlayerData.BuyDamageUpgrade();
    }

    private void Refresh()
    {
        if (_totalCoinsText != null) _totalCoinsText.text = "Total Money: " + PlayerData.Coins;

        if (PlayerData.IsMaxUpgrade)
        {
            if (_priceText != null) _priceText.text = _soldOutText;
            if (_coinIcon != null) _coinIcon.SetActive(false);
            _upgradeButton.interactable = false;
            return;
        }

        int price = PlayerData.NextUpgradePrice;

        if (_priceText != null) _priceText.text = price.ToString();
        if (_coinIcon != null) _coinIcon.SetActive(true);
        _upgradeButton.interactable = PlayerData.Coins >= price;
    }

    private void ReturnToMenu()
    {
        UIManager.Instance.HideWindow("shopui");
        Invoke(nameof(OpenMenu), 1f);
    }

    private void OpenMenu()
    {
        UIManager.Instance.ShowWindow("menuui");
    }

    #region Test Methods
    [Button("Test Show")]
    private void TestShow()
    {
        Show();
    }

    [Button("Test Hide")]
    private void TestHide()
    {
        Hide();
    }
    #endregion
}
