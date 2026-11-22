using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }

    private Vector3 _startPosition;
    private float _intensity;
    private float _duration;
    private float _timeLeft;

    private void Awake()
    {
        Instance = this;
        _startPosition = transform.localPosition;
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
