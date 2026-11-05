using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonSoundManaer : MonoBehaviour
{
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        Install();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Install();
    }

    private void Install()
    {
        Button[] buttons = FindObjectsByType<Button>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i].GetComponent<ButtonSound>() == null)
            {
                buttons[i].gameObject.AddComponent<ButtonSound>();
            }
        }
    }
}
