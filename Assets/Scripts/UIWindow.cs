using UnityEngine;
using DG.Tweening;
using NaughtyAttributes;

public class UIWindow : MonoBehaviour
{
    [SerializeField] private RectTransform _canvasRectTransfrom;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private bool _hideOnStart;

    [Header("Animation Settings")]
    [SerializeField] private float showDuration = 0.5f;
    [SerializeField] private float hideDuration = 0.5f;

    [SerializeField] private Ease showDuration = Ease.OutBack;
    [SerializeField] private Ease hideDuration = Ease.InBack;

    public CanvasGroup CanvasGroup => _canvasGroup;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    public virtual void Initialize()
    {
        if (_hideOnStart)
        {
            Hide();
        }
    }
    public virtual void Show(bool instant = false)
    {
        if(instant)
        {
            _canvasRectTransfrom.gameObject.SetActive(true);
        }
        else 
        {
            RectTransform rectTransform = _canvasGroup.GetComponent<RectTransform>();
            rectTransform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        }
    }

    public virtual void Hide(bool instant = false)
    {
        if(instant)
        {
            _canvasRectTransfrom.gameObject.SetActive(false);
        }
        else 
        {
            RectTransform rectTransform = _canvasGroup.GetComponent<RectTransform>();
            rectTransform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
            {
                _canvasRectTransfrom.gameObject.SetActive(false);
            });
        }
    }

    #region Test

    [Button]

    private void ShowTest()
    {
        Show();
    }

    [Button]

    private void HideTest()
    {
        Hide();
    }

    #endregion
}
