using UnityEngine;

public class AreaSpawner : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private GameObject _prefab;
    [SerializeField] private int _prewarm = 3;

    [Header("Spawn Area")]
    [SerializeField] private Vector2 _areaSize = new Vector2(2f, 6f);
    [SerializeField] private Color _gizmoColor = Color.yellow;

    [Header("Timing (segundos)")]
    [SerializeField] private float _startDelay = 0f;
    [SerializeField] private float _minInterval = 5f;
    [SerializeField] private float _maxInterval = 5f;

    [Header("Objetos spawneados")]
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _lifetime = 30f;

    [Header("Límite ")]
    [SerializeField] private int _maxAlive = 0;

    private ObjectPool _pool;
    private float _timer;

    private void Awake()
    {
        if (_prefab == null)
        {
            Debug.LogError(name + ": falta asignar el prefab", this);
            enabled = false;
            return;
        }

        _pool = new ObjectPool(_prefab, _prewarm);
        _timer = _startDelay;
    }

    private void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer > 0f) return;

        _timer = Random.Range(_minInterval, _maxInterval);
        Spawn();
    }

    private void Spawn()
    {
        if (_maxAlive > 0 && _pool.CountActive() >= _maxAlive) return;

        GameObject obj = _pool.Get(GetRandomPosition());

        ScrollingObject mover = obj.GetComponent<ScrollingObject>();
        if (mover != null) mover.Initialize(_speed, _lifetime);
    }

    private Vector3 GetRandomPosition()
    {
        Vector3 center = transform.position;
        float x = Random.Range(-_areaSize.x * 0.5f, _areaSize.x * 0.5f);
        float y = Random.Range(-_areaSize.y * 0.5f, _areaSize.y * 0.5f);
        return new Vector3(center.x + x, center.y + y, center.z);
    }

    private void OnValidate()
    {
        _areaSize = Vector2.Max(_areaSize, Vector2.zero);
        _minInterval = Mathf.Max(0.1f, _minInterval);
        _maxInterval = Mathf.Max(_minInterval, _maxInterval);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(_gizmoColor.r, _gizmoColor.g, _gizmoColor.b, 0.2f);
        Gizmos.DrawCube(transform.position, _areaSize);
        Gizmos.color = _gizmoColor;
        Gizmos.DrawWireCube(transform.position, _areaSize);
    }
}
