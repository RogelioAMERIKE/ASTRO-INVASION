using UnityEngine;
using UnityEngine.SceneManagement;
 
public class SplashLoader : MonoBehaviour
{
    [SerializeField] private float _duration = 5f;
    [SerializeField] private string _nextScene = "Intro";

    private void Start()
    {
        Invoke(nameof(LoadNextScene), _duration);
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(_nextScene);
    }
}
