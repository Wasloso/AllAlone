using UnityEngine;

using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] private PlayableDirector director;
    [SerializeField] private string nextSceneName = "Game";

    void Start()
    {
        director.stopped += OnCutsceneFinished;
        director.Play();
    }

    private void OnCutsceneFinished(PlayableDirector obj)
    {
        SceneManager.LoadScene(nextSceneName);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            director.time = director.duration;
            director.Evaluate();
            director.Stop();
        }
    }
}
