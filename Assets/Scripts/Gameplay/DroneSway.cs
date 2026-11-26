using UnityEngine;

public class DroneSway : MonoBehaviour
{
    [SerializeField] private float _sideAmount = 0.15f;
    [SerializeField] private float _heightAmount = 0.08f;
    [SerializeField] private float _tiltAngle = 4f;
    [SerializeField] private float _speed = 2f;

    private Vector3 _startPosition;
    private Quaternion _startRotation;
    private float _offset;

    private void Awake()
    {
        _startPosition = transform.localPosition;
        _startRotation = transform.localRotation;
    }

    private void OnEnable()
    {
        transform.localPosition = _startPosition;
        transform.localRotation = _startRotation;
        _offset = Random.Range(0f, 10f);
    }

    private void Update()
    {
        float t = Time.time * _speed + _offset;

        float side = Mathf.Sin(t) * _sideAmount;
        float height = Mathf.Sin(t * 1.7f) * _heightAmount;
        float tilt = -Mathf.Sin(t) * _tiltAngle;

        transform.localPosition = _startPosition + new Vector3(side, height, 0f);
        transform.localRotation = _startRotation * Quaternion.Euler(0f, 0f, tilt);
    }
}
