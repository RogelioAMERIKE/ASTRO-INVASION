using Dino.Utility.Audio;
using UnityEngine;
using AudioType = Dino.Utility.Audio.AudioType;

public class GameAudio : MonoBehaviour
{
    private const string MusicKey = "MusicVolume01";
    private const string SfxKey = "SfxVolume01";
    private const float DefaultVolume = 0.8f;

    private static string _currentMusic;

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

        AudioManager.Instance.PlaySound(soundName);
    }

    public static void PlayMusic(string musicName)
    {
        if (string.IsNullOrEmpty(musicName)) return;
        if (AudioManager.Instance == null) return;

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
