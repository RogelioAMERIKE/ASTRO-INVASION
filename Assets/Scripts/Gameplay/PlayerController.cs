using DG.Tweening;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private enum GhostMode { None, Rising, Timed }

    [Header("Jump")]
    [SerializeField] private float _jumpForce = 14f;

    [Header("Ghost Mode")]
    [SerializeField] private float _ghostDuration = 2f;
    [SerializeField] private float _maxGhostOverrun = 3f;
    [Range(0.1f, 1f)]
    [SerializeField] private float _ghostAlpha = 0.5f;
    [SerializeField] private SpriteRenderer _sprite;

    [Header("Ground Check")]
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private LayerMask _platformMask;
    [SerializeField] private float _checkDistance = 0.1f;

    [Header("Shooting")]
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private float _fireCooldown = 0.25f;

    [Header("Animation")]
    [SerializeField] private Animator _animator;

    [Header("Effects")]
    [SerializeField] private ParticleSystem _shootEffect;
    [SerializeField] private ParticleSystem _explosionEffect;
    [SerializeField] private string _deathSound = "player_death";

    [Header("Sounds")]
    [SerializeField] private string _jumpSound = "jump";
    [SerializeField] private string _dropSound = "drop";
    [SerializeField] private string _shootSound = "shoot";

    [Header("Entrance")]
    [SerializeField] private Vector2 _entranceOffset = new Vector2(-8f, 0f);
    [SerializeField] private float _entranceTime = 1f;

    private Rigidbody2D _rb;
    private Collider2D _col;
    private ObjectPool _bulletPool;

    private GhostMode _ghostMode = GhostMode.None;
    private float _ghostTimer;
    private float _nextFireTime;
    private bool _entering;
    private float _defaultGravity;

    private bool IsGhost
    {
        get { return _ghostMode != GhostMode.None; }
    }

    private bool CanAct
    {
        get
        {
            if (_entering) return false;
            if (GameManager.Instance == null) return false;
            return GameManager.Instance.State == GameState.Playing;
        }
    }

    private Vector2 Velocity
    {
        get { return _rb.linearVelocity; }
        set { _rb.linearVelocity = value; }
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponent<Collider2D>();
        _rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        _bulletPool = new ObjectPool(_bulletPrefab, 10);
    }

    private void Start()
    {
        StartEntrance();
    }

    private void OnDestroy()
    {
        transform.DOKill();
    }

    private void OnDisable()
    {
        SetGhostMode(GhostMode.None);
    }

    private void Update()
    {
        if (!IsGhost) return;

        _ghostTimer -= Time.deltaTime;

        bool timeUp = _ghostTimer <= 0f;
        bool startedFalling = _ghostMode == GhostMode.Rising && Velocity.y <= 0f;

        if (!timeUp && !startedFalling) return;

        if (_ghostTimer <= -_maxGhostOverrun || !IsInsidePlatform())
        {
            SetGhostMode(GhostMode.None);
        }
    }

    public void Jump()
    {
        if (!CanAct) return;
        if (Velocity.y > 0.1f) return;
        if (!IsOnGround() && !IsOnPlatform()) return;

        Velocity = new Vector2(0f, _jumpForce);
        StartGhost(GhostMode.Rising);
        PlayAnimation("Jump");
        GameAudio.PlaySfx(_jumpSound);
    }

    public void Drop()
    {
        if (!CanAct) return;
        if (IsGhost) return;
        if (IsOnGround()) return;

        StartGhost(GhostMode.Timed);
        PlayAnimation("Down");
        GameAudio.PlaySfx(_dropSound);
    }

    public void Shoot()
    {
        if (!CanAct) return;
        if (Time.time < _nextFireTime) return;

        _nextFireTime = Time.time + _fireCooldown;

        Vector3 origin = transform.position;
        if (_firePoint != null) origin = _firePoint.position;

        _bulletPool.Get(origin);
        PlayAnimation("Shoot");
        if (_shootEffect != null) _shootEffect.Play();
        GameAudio.PlaySfx(_shootSound);
    }

    private void StartEntrance()
    {
        if (_entranceTime <= 0f) return;

        Vector3 endPosition = transform.position;
        transform.position = endPosition + (Vector3)_entranceOffset;

        _entering = true;
        _defaultGravity = _rb.gravityScale;
        _rb.gravityScale = 0f;
        Velocity = Vector2.zero;

        transform.DOMove(endPosition, _entranceTime).SetEase(Ease.OutCubic).OnComplete(EndEntrance);
    }

    private void EndEntrance()
    {
        _rb.gravityScale = _defaultGravity;
        _entering = false;
    }

    private void PlayAnimation(string trigger)
    {
        if (_animator != null) _animator.SetTrigger(trigger);
    }

    private void Explode()
    {
        if (_sprite != null) _sprite.enabled = false;
        if (_explosionEffect != null) _explosionEffect.Play();
        GameAudio.PlaySfx(_deathSound);
    }

    private void StartGhost(GhostMode mode)
    {
        _ghostTimer = _ghostDuration;
        SetGhostMode(mode);
    }

    private void SetGhostMode(GhostMode mode)
    {
        _ghostMode = mode;
        bool ghost = IsGhost;

        int playerLayer = gameObject.layer;
        for (int layer = 0; layer < 32; layer++)
        {
            if ((_platformMask.value & (1 << layer)) != 0)
            {
                Physics2D.IgnoreLayerCollision(playerLayer, layer, ghost);
            }
        }

        if (_sprite != null)
        {
            Color c = _sprite.color;
            if (ghost) c.a = _ghostAlpha;
            else c.a = 1f;
            _sprite.color = c;
        }
    }

    private bool IsInsidePlatform()
    {
        Bounds b = _col.bounds;
        return Physics2D.OverlapBox(b.center, b.size * 0.9f, 0f, _platformMask) != null;
    }

    private bool IsOnGround()
    {
        return CheckBelow(_groundMask);
    }

    private bool IsOnPlatform()
    {
        if (IsGhost) return false;
        return CheckBelow(_platformMask);
    }

    private bool CheckBelow(LayerMask mask)
    {
        Bounds b = _col.bounds;
        Vector2 center = new Vector2(b.center.x, b.min.y - _checkDistance * 0.5f);
        Vector2 size = new Vector2(b.size.x * 0.9f, _checkDistance);
        return Physics2D.OverlapBox(center, size, 0f, mask) != null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!CanAct) return;

        if (other.GetComponentInParent<Hazard>() != null)
        {
            Explode();
            GameManager.Instance.PlayerDied();
            return;
        }

        Coin coin = other.GetComponentInParent<Coin>();
        if (coin != null) coin.Collect();
    }
}
