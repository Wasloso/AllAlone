using Player;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;

    [SerializeField] private PlayerInputHandler inputHandler;
    private bool isPaused;

    private void Awake()
    {
        if (!inputHandler) inputHandler = FindFirstObjectByType<PlayerInputHandler>();
        if (!inputHandler)
        {
            Debug.LogError("No input handler found");
            enabled = false;
        }
    }

    private void OnEnable()
    {
        if (inputHandler != null) inputHandler.OnPausePressed += TogglePause;
    }

    private void OnDisable()
    {
        if (inputHandler != null) inputHandler.OnPausePressed -= TogglePause;
    }

    private void TogglePause()
    {
        if (isPaused)
            Resume();
        else
            Pause();
    }

    public void Resume()
    {
        inputHandler.InputActions.Player.Enable();
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Pause()
    {
        inputHandler.InputActions.Player.Disable();
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }


    public void Quit()
    {
        TogglePause();
        DataPersistenceManager.Instance.SaveGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1); ;
    }
}