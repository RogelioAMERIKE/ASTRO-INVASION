using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
public class SettingsUI : UIWindow
{
    [Header("SettingsUI")]
    [FormerlySerializedAs("_volumeSlider")]
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;
    public Button _backButton;
    public Button _creditsButton;


    public override void Initialize()
    {
        base.Initialize();

        _musicSlider.SetValueWithoutNotify(GameAudio.MusicVolume);
        _sfxSlider.SetValueWithoutNotify(GameAudio.SfxVolume);

        _musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        _sfxSlider.onValueChanged.AddListener(OnSfxVolumeChanged);
        _backButton.onClick.AddListener(ReturnToMenu);
        _creditsButton.onClick.AddListener(OpenCredits);


    }
    private void ReturnToMenu()
    {
        if (!UIFlow.Begin()) return;

        GameAudio.SaveVolumes();
        UIFlow.Hide("settingsui");
        Invoke(nameof(ShowMenu), 1f);
    }

    private void ShowMenu()
    {
        UIFlow.Show("menuui");
    }

    private void OpenCredits()
    {
        if (!UIFlow.Begin()) return;

        GameAudio.SaveVolumes();
        UIFlow.Hide("settingsui");
        Invoke(nameof(ShowCredits), 1f);
    }

    private void ShowCredits()
    {
        UIFlow.Show("creditsui");
    }

    private void OnMusicVolumeChanged(float value)
    {
        GameAudio.SetMusicVolume(value);
    }

    private void OnSfxVolumeChanged(float value)
    {
        GameAudio.SetSfxVolume(value);
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
