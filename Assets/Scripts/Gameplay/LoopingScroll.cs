using UnityEngine;
using UnityEngine.UI;

public class LoopingScroll : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private int _tiles = 6;

    private RawImage _rawImage;
    private SpriteRenderer _sprite;
    private float _width;
    private Vector3 _startPosition;

    private void Start()
    {
        _rawImage = GetComponent<RawImage>();
        _sprite = GetComponent<SpriteRenderer>();

        if (_sprite != null) SetupSprite();
    }

    private void Update()
    {
        if (_rawImage != null) ScrollRawImage();
        else if (_sprite != null) ScrollSprite();
    }

    private void SetupSprite()
    {
        float tileWidth = _sprite.sprite.bounds.size.x;
        float tileHeight = _sprite.sprite.bounds.size.y;

        if (_sprite.drawMode == SpriteDrawMode.Simple)
        {
            _sprite.drawMode = SpriteDrawMode.Tiled;
            _sprite.size = new Vector2(tileWidth * _tiles, tileHeight);
        }

        _width = tileWidth * transform.localScale.x;
        _startPosition = transform.position;
    }

    private void ScrollSprite()
    {
        transform.Translate(Vector2.left * _speed * Time.deltaTime, Space.World);

        if (transform.position.x <= _startPosition.x - _width)
        {
            transform.position = _startPosition;
        }
    }

    private void ScrollRawImage()
    {
        Rect rect = _rawImage.uvRect;
        rect.x = Mathf.Repeat(rect.x + _speed * Time.deltaTime, 1f);
        _rawImage.uvRect = rect;
    }
}
