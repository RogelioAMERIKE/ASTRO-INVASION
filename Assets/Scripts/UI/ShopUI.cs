using DG.Tweening;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : UIWindow
{
    [Header("Buttons")]
    public Button _backButton;
    public Button _upgradeButton;

    [Header("Texts")]
    [SerializeField] private TMP_Text _totalCoinsText;
    [SerializeField] private TMP_Text _priceText;
    [SerializeField] private GameObject _coinIcon;
    [SerializeField] private string _soldOutText = "AGOTADO";
    [SerializeField] private Color _unavailableColor = new Color(0.6f, 0.6f, 0.6f, 1f);

    [Header("Compra")]
    [SerializeField] private ParticleSystem _buyEffect;
    [SerializeField] private string _buySound = "shop_buy";
    [SerializeField] private string _deniedSound = "shop_denied";

    [Header("Barra de mejora")]
    [SerializeField] private Image _barImage;
    [SerializeField] private Sprite[] _barSprites;
    [SerializeField] private float _barFadeTime = 0.2f;

    private Sequence _barSequence;

    public override void Initialize()
    {
        base.Initialize();

        _backButton.onClick.AddListener(ReturnToMenu);
        _upgradeButton.onClick.AddListener(OnUpgrade);

        PrepareUpgradeButton();

        PlayerData.OnCoinsChanged += Refresh;
        PlayerData.OnUpgradeChanged += OnUpgradeChanged;

        Refresh();
        ShowBar(false);
    }

    private void OnDestroy()
    {
        PlayerData.OnCoinsChanged -= Refresh;
        PlayerData.OnUpgradeChanged -= OnUpgradeChanged;

        if (_barSequence != null) _barSequence.Kill();
    }

    private void PrepareUpgradeButton()
    {
        _upgradeButton.interactable = true;

        ButtonSound sound = _upgradeButton.GetComponent<ButtonSound>();
        if (sound == null) sound = _upgradeButton.gameObject.AddComponent<ButtonSound>();

        sound.SetSoundName("");
        sound.SetLockUI(false);
    }

    private void OnUpgrade()
    {
        if (PlayerData.BuyDamageUpgrade())
        {
            GameAudio.PlaySfx(_buySound);
            if (_buyEffect != null) _buyEffect.Play();
            return;
        }

        GameAudio.PlaySfx(_deniedSound);
    }

    private void OnUpgradeChanged()
    {
        Refresh();
        ShowBar(true);
    }

    private void Refresh()
    {
        if (_totalCoinsText != null) _totalCoinsText.text = "Total Money: " + PlayerData.Coins;

        if (PlayerData.IsMaxUpgrade)
        {
            if (_priceText != null) _priceText.text = _soldOutText;
            if (_coinIcon != null) _coinIcon.SetActive(false);
            SetUpgradeColor(false);
            return;
        }

        int price = PlayerData.NextUpgradePrice;

        if (_priceText != null) _priceText.text = price.ToString();
        if (_coinIcon != null) _coinIcon.SetActive(true);
        SetUpgradeColor(PlayerData.Coins >= price);
    }

    private void SetUpgradeColor(bool canBuy)
    {
        if (_upgradeButton.image == null) return;

        if (canBuy) _upgradeButton.image.color = Color.white;
        else _upgradeButton.image.color = _unavailableColor;
    }

    private void ShowBar(bool animate)
    {
        if (_barImage == null) return;
        if (_barSprites == null || _barSprites.Length == 0) return;

        int index = Mathf.Clamp(PlayerData.DamageLevel, 0, _barSprites.Length - 1);
        Sprite sprite = _barSprites[index];

        if (_barSequence != null) _barSequence.Kill();

        if (!animate)
        {
            _barImage.sprite = sprite;
            SetBarAlpha(1f);
            return;
        }

        _barSequence = DOTween.Sequence();
        _barSequence.Append(DOVirtual.Float(1f, 0f, _barFadeTime, SetBarAlpha));
        _barSequence.AppendCallback(() => _barImage.sprite = sprite);
        _barSequence.Append(DOVirtual.Float(0f, 1f, _barFadeTime, SetBarAlpha));
        _barSequence.SetUpdate(true);
    }

    private void SetBarAlpha(float value)
    {
        Color color = _barImage.color;
        color.a = value;
        _barImage.color = color;
    }

    private void ReturnToMenu()
    {
        if (!UIFlow.Begin()) return;

        UIFlow.Hide("shopui");
        Invoke(nameof(OpenMenu), 1f);
    }

    private void OpenMenu()
    {
        UIFlow.Show("menuui");
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
