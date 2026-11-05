using UnityEngine;
using System.Collections;

public class SceneMusic : MonoBehaviour
{
    [SerializeField] private string _musicName;

    private void Start()
    {
        Invoke(nameof(PlayMusic), 0.1f);
    }

    private void PlayMusic()
    {
        GameAudio.ApplySavedVolumes();
        GameAudio.PlayMusic(_musicName);
    }
}
