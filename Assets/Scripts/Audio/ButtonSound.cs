using UnityEngine;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
    [SerializeField] private string _soundName = "button";

    private void Awake()
    {
        Button button = GetComponent<Button>();
        if (button != null) button.onClick.AddListener(PlaySound);
    }

    private void PlaySound()
    {
        GameAudio.PlaySfx(_soundName);
    }
}
