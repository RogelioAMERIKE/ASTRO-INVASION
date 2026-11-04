using UnityEngine;

public class ScrollingObject : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _lifetime = 30f;
    [SerializeField] private bool _despawnOffscreen = true;
    [SerializeField] private float _offscreenMargin = 2f;

    private static Camera _cam;
    private Collider2D _collider;
    private float _lifeLeft;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        _lifeLeft = _lifetime;
    }

    public void Initialize(float speed, float lifetime)
    {
        _speed = speed;
        _lifetime = lifetime;
        _lifeLeft = lifetime;
    }

    private void Update()
    {
        transform.Translate(Vector2.left * _speed * Time.deltaTime, Space.World);

        _lifeLeft -= Time.deltaTime;
        if (_lifeLeft <= 0f)
        {
            gameObject.SetActive(false);
            return;
        }

        if (_despawnOffscreen && IsOffscreenLeft())
        {
            gameObject.SetActive(false);
        }
    }

    private bool IsOffscreenLeft()
    {
        if (_cam == null) _cam = Camera.main;
        if (_cam == null) return false;

        float leftEdge = _cam.ViewportToWorldPoint(new Vector3(0f, 0.5f, 0f)).x;

        float objectRight = transform.position.x;
        if (_collider != null) objectRight = _collider.bounds.max.x;

        return objectRight < leftEdge - _offscreenMargin;
    }
}
