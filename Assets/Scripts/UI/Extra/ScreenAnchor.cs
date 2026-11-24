using UnityEngine;

public class ScreenAnchor : MonoBehaviour
{
    [Header("Posicion en X de la pantalla (0 = borde izquierdo, 1 = borde derecho)")]
    [SerializeField] private float _viewportX = 1f;

    [Header("Desplazamiento en unidades (negativo = hacia la izquierda, positivo = hacia la derecha)")]
    [SerializeField] private float _offsetX = -3f;

    private Camera _cam;

    private void Update()
    {
        if (_cam == null) _cam = Camera.main;
        if (_cam == null) return;

        float edge = _cam.ViewportToWorldPoint(new Vector3(_viewportX, 0.5f, 0f)).x;

        Vector3 position = transform.position;
        position.x = edge + _offsetX;
        transform.position = position;
    }
}
