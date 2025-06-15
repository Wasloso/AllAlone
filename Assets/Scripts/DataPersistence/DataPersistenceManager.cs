using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : MonoBehaviour
{
    private static DataPersistenceManager _instance;

    [Header("File Storage Config")] [SerializeField]
    private string fileName;

    public int currentGame;

    private List<IDataPersistence> dataPersistenceList;

    private GameData gameData;
    public FileDataHandler FileDataHandler { get; private set; }

    public static DataPersistenceManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindAnyObjectByType<DataPersistenceManager>();
            return _instance;
        }

        private set { }
    }

    private void Awake()
    {
        FileDataHandler = new FileDataHandler(Application.persistentDataPath);
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<IDataPersistence> FindAllDataPersistence()
    {
        var dataPersistences = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistences);
    }

    public void NewGame()
    {
        gameData = new GameData();
    }

    public void LoadGameScene()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        var currentIndex = SceneManager.GetActiveScene().buildIndex;
        var nextIndex = currentIndex + 1;
        if (FileDataHandler.SaveExists(fileName + currentGame))
            nextIndex++;

        if (nextIndex < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(nextIndex);
        else
            Debug.LogWarning("No next scene found in build settings!");
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LoadGame();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void LoadGame()
    {
        SetPersistenceList();
        gameData = FileDataHandler.Load(fileName + currentGame);
        if (gameData == null) NewGame();

        foreach (var dataPersistence in dataPersistenceList) dataPersistence.LoadData(gameData);
    }

    public void SaveGame()
    {
        SetPersistenceList();
        foreach (var dataPersistence in dataPersistenceList) dataPersistence.SaveData(ref gameData);

        FileDataHandler.Save(gameData, fileName + currentGame);
        dataPersistenceList.Clear();
    }

    public void SetPersistenceList()
    {
        if (dataPersistenceList == null || dataPersistenceList.Count == 0)
            dataPersistenceList = FindAllDataPersistence();
    }

    public void ResetGame(int slotNum)
    {
        FileDataHandler.Delete(fileName + slotNum);
        gameData = null;
    }
}