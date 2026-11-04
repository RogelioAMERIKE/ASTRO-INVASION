using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

public class ShopUI : UIWindow
{
    public Button _backButton;
    public override void Initialize()
    {
        base.Initialize();
        _backButton.onClick.AddListener(ReturnToMenu);
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
