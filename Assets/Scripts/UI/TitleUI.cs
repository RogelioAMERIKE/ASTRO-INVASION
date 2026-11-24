using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
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
        if (!UIFlow.Begin()) return;

        UIFlow.Hide("titleui");
        DOVirtual.DelayedCall(1f, OpenMenu, true);
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
