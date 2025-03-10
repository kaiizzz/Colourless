using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class SceneManagerSingleton : MonoBehaviour
{
    public static SceneManagerSingleton Instance { get; private set; }

    public Image fadeCanvas;  // Assign a UI Canvas Image for fading
    public float fadeDuration = 1.0f;

    private void Awake()
    {
        // Enforce Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    /// <summary>
    /// Load a scene normally with fade effect.
    /// </summary>
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneWithFade(sceneName));
    }

    /// <summary>
    /// Load a scene asynchronously with a fade effect.
    /// </summary>
    private IEnumerator LoadSceneWithFade(string sceneName)
    {
        if (fadeCanvas != null)
        {
            yield return StartCoroutine(FadeIn());
        }

        yield return SceneManager.LoadSceneAsync(sceneName);

        if (fadeCanvas != null)
        {
            yield return StartCoroutine(FadeOut());
        }
    }

    /// <summary>
    /// Reloads the current scene.
    /// </summary>
    public void ReloadScene()
    {
        LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Loads the next scene in the build order.
    /// </summary>
    public void LoadNextScene()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            LoadScene(SceneManager.GetSceneAt(nextSceneIndex).name);
        }
    }

    /// <summary>
    /// Fades the screen to black.
    /// </summary>
    private IEnumerator FadeIn()
    {
        float time = 0;
        Color tempColor = fadeCanvas.color;
        while (time < fadeDuration)
        {
            tempColor.a = Mathf.Lerp(0, 1, time / fadeDuration);
            fadeCanvas.color = tempColor;
            time += Time.deltaTime;
            yield return null;
        }
        tempColor.a = 1;
        fadeCanvas.color = tempColor;
    }

    /// <summary>
    /// Fades the screen from black.
    /// </summary>
    private IEnumerator FadeOut()
    {
        float time = 0;
        Color tempColor = fadeCanvas.color;
        while (time < fadeDuration)
        {
            tempColor.a = Mathf.Lerp(1, 0, time / fadeDuration);
            fadeCanvas.color = tempColor;
            time += Time.deltaTime;
            yield return null;
        }
        tempColor.a = 0;
        fadeCanvas.color = tempColor;
    }
}
