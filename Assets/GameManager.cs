using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TimeOfDaySystem timeOfDaySystem;
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private HealthSystem playerHealthSystem;

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
        {
            Debug.Log("It's night — starting enemy spawn.");
            enemySpawner.StartSpawning();
        }
        else
        {
            Debug.Log("It's not night — stopping enemy spawn.");
            enemySpawner.StopSpawning();
        }
    }

    private void HandlePlayerDeath()
    {
        if (isGameOver) return;

        isGameOver = true;
        Debug.Log("Game Over!");

        enemySpawner.StopSpawning();
    }
}