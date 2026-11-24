using DG.Tweening;
using Dino.Utility.Audio;
using UnityEngine;

public class ObstacleSound : MonoBehaviour
{
    [SerializeField] private string _soundName = "obstacle";
    [SerializeField] private float _fadeTime = 2f;

    private GameObject _holder;
    private AudioSource _source;
    private Tween _fadeTween;
    private float _baseVolume = 1f;
    private float _basePitch = 1f;
    private int _startFrame;

    private void Awake()
    {
        _holder = new GameObject("ObstacleSound");
        _source = _holder.AddComponent<AudioSource>();
        _source.playOnAwake = false;
        _source.loop = true;
    }

    private void OnEnable()
    {
        KillFade();
        _startFrame = Time.frameCount;

        if (string.IsNullOrEmpty(_soundName)) return;
        if (AudioManager.Instance == null) return;

        AudioData data = AudioManager.Instance.AudioManagerData.GetAudioData(_soundName);
        if (data == null) return;

        _baseVolume = data.volume;
        _basePitch = data.pitch;

        _source.clip = data.clip;
        _source.volume = _baseVolume;
        _source.pitch = _basePitch;
        _source.outputAudioMixerGroup = AudioManager.Instance.AudioManagerData.SfxAudioMixerGroup;
        _source.Play();
    }

    private void OnDisable()
    {
        if (_source == null) return;
        if (!_source.isPlaying) return;

        if (Time.frameCount == _startFrame)
        {
            _source.Stop();
            return;
        }

        _fadeTween = DOVirtual.Float(1f, 0f, _fadeTime, SetFade).SetUpdate(true).SetLink(_holder).OnComplete(FinishFade);
    }

    private void OnDestroy()
    {
        KillFade();
        if (_holder != null) Destroy(_holder);
    }

    private void Update()
    {
        if (_source == null) return;

        _source.pitch = _basePitch * Time.timeScale;
    }

    private void SetFade(float value)
    {
        if (_source == null) return;

        _source.volume = _baseVolume * value;
        _source.pitch = _basePitch * Time.timeScale;
    }

    private void FinishFade()
    {
        _fadeTween = null;
        if (_source != null) _source.Stop();
    }

    private void KillFade()
    {
        if (_fadeTween == null) return;

        _fadeTween.Kill();
        _fadeTween = null;
    }
}
