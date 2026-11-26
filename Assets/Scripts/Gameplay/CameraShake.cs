using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }

    [Header("Temblor al iniciar la escena")]
    [SerializeField] private bool _shakeOnStart = true;
    [SerializeField] private float _startIntensity = 0.2f;
    [SerializeField] private float _startDuration = 0.5f;
    [SerializeField] private string _startSound = "scene_start";
    [SerializeField] private float _soundFadeDelay = 0f;
    [SerializeField] private float _soundFadeTime = 3f;

    private Vector3 _startPosition;
    private float _intensity;
    private float _duration;
    private float _timeLeft;

    private void Awake()
    {
        Instance = this;
        _startPosition = transform.localPosition;
    }

    private void Start()
    {
        if (_shakeOnStart) Shake(_startIntensity, _startDuration);
        GameAudio.PlaySfx(_startSound);
        GameAudio.FadeOutSfx(_startSound, _soundFadeDelay, _soundFadeTime);
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    public void Shake(float intensity, float duration)
    {
        _intensity = intensity;
        _duration = duration;
        _timeLeft = duration;
    }

    private void LateUpdate()
    {
        if (_timeLeft <= 0f) return;

        _timeLeft -= Time.unscaledDeltaTime;

        if (_timeLeft <= 0f)
        {
            transform.localPosition = _startPosition;
            return;
        }

        float strength = _intensity * (_timeLeft / _duration);
        Vector2 offset = Random.insideUnitCircle * strength;
        transform.localPosition = _startPosition + new Vector3(offset.x, offset.y, 0f);
    }
}
