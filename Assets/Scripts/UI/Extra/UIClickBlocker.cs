using UnityEngine;
using UnityEngine.UI;

public class UIClickBlocker : MonoBehaviour
{
    private static UIClickBlocker _instance;

    private Image _image;
    private float _lockUntil;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStatics()
    {
        _instance = null;
    }

    public static void Lock(float time)
    {
        if (_instance == null) Create();

        float until = Time.unscaledTime + time;
        if (until > _instance._lockUntil) _instance._lockUntil = until;

        _instance._image.raycastTarget = true;
    }

    private static void Create()
    {
        GameObject root = new GameObject("UIClickBlocker");
        DontDestroyOnLoad(root);

        Canvas canvas = root.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 32000;
        root.AddComponent<GraphicRaycaster>();

        GameObject imageObject = new GameObject("Blocker");
        imageObject.transform.SetParent(root.transform, false);

        Image image = imageObject.AddComponent<Image>();
        image.color = new Color(0f, 0f, 0f, 0f);
        image.raycastTarget = false;
        image.canvasRenderer.cullTransparentMesh = false;

        RectTransform rect = image.rectTransform;
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        _instance = root.AddComponent<UIClickBlocker>();
        _instance._image = image;
    }

    private void Update()
    {
        if (!_image.raycastTarget) return;

        if (Time.unscaledTime >= _lockUntil) _image.raycastTarget = false;
    }
}
