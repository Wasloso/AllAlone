using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TimeOfDaySystem timeOfDaySystem;
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private HealthSystem playerHealthSystem;
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private GameObject gameFinishedScreen;
    public bool IsFinished;

    private bool isGameOver;
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        timeOfDaySystem.OnTimeOfDayChanged += HandleTimeChange;
        playerHealthSystem.OnDied += HandlePlayerDeath;
    }

    private void HandleTimeChange(TimeOfDaySystem.TimeOfDay timeOfDay)
    {
        if (isGameOver) return;

        if (timeOfDay == TimeOfDaySystem.TimeOfDay.Night)
            enemySpawner.StartSpawning();
        else
            enemySpawner.StopSpawning();
    }

    private void HandlePlayerDeath()
    {
        if (isGameOver) return;

        isGameOver = true;
        Debug.Log("Player death");

        enemySpawner.StopSpawning();
        deathScreen.SetActive(true);
        // DataPersistenceManager.Instance.ResetGame();
    }

    public void QuitWithoutSaving()
    {
        SceneManager.LoadScene(0);
    }

    public void FinishGame()
    {
        IsFinished = true;
        gameFinishedScreen.SetActive(true);
    }
}