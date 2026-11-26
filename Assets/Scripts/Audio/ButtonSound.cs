using UnityEngine;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
    [SerializeField] private string _soundName = "button";
    [SerializeField] private bool _lockUI = false;
    [SerializeField] private float _lockTime = 0f;

    public void SetLockUI(bool value)
    {
        _lockUI = value;
    }

    public void SetSoundName(string value)
    {
        _soundName = value;
    }

    private void Awake()
    {
        Button button = GetComponent<Button>();
        if (button != null) button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        GameAudio.PlaySfx(_soundName);

        if (_lockUI) UIClickGuard.Lock(_lockTime);
    }
}
