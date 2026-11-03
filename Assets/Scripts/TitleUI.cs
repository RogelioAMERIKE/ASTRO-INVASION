using UnityEngine;
using NaughtyAttributes;
using UnityEngine.UI;

public class TitleUI : UIWindow
{
    public Button button;

    public override void Initialize()
    {
        base.Initialize();
        button.onClick.LoadScene(sceneName);
    }

    private void LoadScene(string sceneName)
    {

        SceneManager.LoadScene(sceneName);
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
