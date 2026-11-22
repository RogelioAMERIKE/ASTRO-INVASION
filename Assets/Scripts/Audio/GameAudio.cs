using DG.Tweening;
using Dino.Utility.Audio;
using UnityEngine;
using AudioType = Dino.Utility.Audio.AudioType;

public class GameAudio : MonoBehaviour
{
    private const string MusicKey = "MusicVolume01";
    private const string SfxKey = "SfxVolume01";
    private const float DefaultVolume = 0.8f;

    private static string _currentMusic;
    private static Tween _fadeTween;
    private static AudioSource _fadeSource;
    private static float _fadePitch;
    private static float _fadeVolume;

    public static float MusicVolume
    {
        get { return PlayerPrefs.GetFloat(MusicKey, DefaultVolume); }
    }

    public static float SfxVolume
    {
        get { return PlayerPrefs.GetFloat(SfxKey, DefaultVolume); }
    }

    public static void PlaySfx(string soundName)
    {
        if (string.IsNullOrEmpty(soundName)) return;
        if (AudioManager.Instance == null) return;

        string[] names = soundName.Split(',');
        for (int i = 0; i < names.Length; i++)
        {
            string name = names[i].Trim();
            if (name != "") AudioManager.Instance.PlaySound(name);
        }
    }

    public static void PlayMusic(string musicName)
    {
        if (string.IsNullOrEmpty(musicName)) return;
        if (AudioManager.Instance == null) return;

        CancelFade();
        StopMusic();

        _currentMusic = musicName;
        AudioManager.Instance.PlaySound(musicName);

        AudioSource source = FindMusicSource();
        if (source != null)
        {
            source.outputAudioMixerGroup = AudioManager.Instance.AudioManagerData.MusicAudioMixerGroup;
        }
    }

    public static void StopMusic()
    {
        if (string.IsNullOrEmpty(_currentMusic)) return;
        if (AudioManager.Instance == null) return;

        AudioManager.Instance.StopSound(_currentMusic);
    }

    public static void FadeOutMusic(float duration)
    {
        AudioSource source = FindMusicSource();
        if (source == null) return;

        CancelFade();

        _fadeSource = source;
        _fadePitch = source.pitch;
        _fadeVolume = source.volume;

        _fadeTween = DOVirtual.Float(1f, 0f, duration, SetFade).SetUpdate(true).OnComplete(FinishFade);
    }

    public static void PauseMusic()
    {
        AudioSource source = FindMusicSource();
        if (source != null) source.Pause();
    }

    public static void ResumeMusic()
    {
        AudioSource source = FindMusicSource();
        if (source != null) source.UnPause();
    }

    public static void SetMusicVolume(float value)
    {
        PlayerPrefs.SetFloat(MusicKey, value);
        PlayerPrefs.Save();

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.UpdateAudioMixerGroupVolume(value, AudioType.Music);
        }
    }

    public static void SetSfxVolume(float value)
    {
        PlayerPrefs.SetFloat(SfxKey, value);
        PlayerPrefs.Save();

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.UpdateAudioMixerGroupVolume(value, AudioType.SFX);
        }
    }

    public static void ApplySavedVolumes()
    {
        if (AudioManager.Instance == null) return;

        AudioManager.Instance.UpdateAudioMixerGroupVolume(MusicVolume, AudioType.Music);
        AudioManager.Instance.UpdateAudioMixerGroupVolume(SfxVolume, AudioType.SFX);
    }

    private static void SetFade(float value)
    {
        _fadeSource.pitch = _fadePitch * value;
        _fadeSource.volume = _fadeVolume * value;
    }

    private static void FinishFade()
    {
        _fadeTween = null;
        _fadeSource.Stop();
        RestoreFade();
    }

    private static void CancelFade()
    {
        if (_fadeTween == null) return;

        _fadeTween.Kill();
        _fadeTween = null;
        RestoreFade();
    }

    private static void RestoreFade()
    {
        if (_fadeSource == null) return;

        _fadeSource.pitch = _fadePitch;
        _fadeSource.volume = _fadeVolume;
    }

    private static AudioSource FindMusicSource()
    {
        if (string.IsNullOrEmpty(_currentMusic)) return null;
        if (AudioManager.Instance == null) return null;

        AudioData data = AudioManager.Instance.AudioManagerData.GetAudioData(_currentMusic);
        if (data == null) return null;

        AudioSource[] sources = AudioManager.Instance.GetComponentsInChildren<AudioSource>(true);
        for (int i = 0; i < sources.Length; i++)
        {
            if (sources[i].clip == data.clip) return sources[i];
        }

        return null;
    }
}
