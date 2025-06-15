using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

// Or UnityEngine.Video if using VideoPlayer

public class CutsceneManager : MonoBehaviour
{
    public PlayableDirector cutscene;

    private void Start()
    {
        if (cutscene != null)
        {
            cutscene.stopped += OnCutsceneFinished;
            cutscene.Play();
        }
        else
        {
            Debug.LogWarning("No cutscene assigned.");
            ProceedToGame();
        }
    }

    private void OnCutsceneFinished(PlayableDirector pd)
    {
        ProceedToGame();
    }

    private void ProceedToGame()
    {
        SceneManager.sceneLoaded += OnGameSceneLoaded;
        var currentIndex = SceneManager.GetActiveScene().buildIndex;
        var nextIndex = currentIndex + 1;

        if (nextIndex < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(nextIndex);
        else
            Debug.LogWarning("No next scene found in build settings!");
    }

    private void OnGameSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        DataPersistenceManager.Instance.LoadGame();
        SceneManager.sceneLoaded -= OnGameSceneLoaded;
    }
}