using UnityEngine;
using NaughtyAttributes;
using UnityEngine.UI;

public class TitleUI : UIWindow
{
    public Button button;

    public override void Initialize()
    {
        base.Initialize();
        button.onClick.AddListener(CloseTitle);
    }

    private void CloseTitle()
    {
        UIManager.Instance.HideWindow("titleui");
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
