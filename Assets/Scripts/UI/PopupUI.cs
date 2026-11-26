using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class PopupUI : UIWindow
{

    #region Test

    [Button]
    private void ShowTest()
    {
        Show();
    }

    [Button]
    private void HideTest()
    {
        Hide();
    }


    #endregion
}
