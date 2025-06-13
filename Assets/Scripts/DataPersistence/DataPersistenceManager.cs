using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;

    public int currentGame = 0;
    private static DataPersistenceManager _instance;
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

    private GameData gameData;

    private List<IDataPersistence> dataPersistenceList;
    private FileDataHandler fileDataHandler;

    private void Awake()
    {
        fileDataHandler = new FileDataHandler(Application.persistentDataPath);
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private List<IDataPersistence> FindAllDataPersistence()
    {
        IEnumerable<IDataPersistence> dataPersistences = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistences);
    }

    public void NewGame()
    {
        gameData = new GameData();
        Debug.Log("New Game!");
    }

    public void LoadGameScene()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene("TestScene");
    }


    private void OnSceneLoaded(Scene scene,LoadSceneMode mode)
    {
        LoadGame();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void LoadGame()
    {
        SetPersistenceList();
        gameData = fileDataHandler.Load(fileName+currentGame);
        if (gameData == null)
        {
            NewGame();  
       
        }

        foreach (IDataPersistence dataPersistence in dataPersistenceList)
        {
            dataPersistence.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        SetPersistenceList();
        foreach (IDataPersistence dataPersistence in dataPersistenceList)
        {
            dataPersistence.SaveData(ref gameData);
        }

        fileDataHandler.Save(gameData, fileName+currentGame);
        dataPersistenceList.Clear();
    }

    public void OnApplicationQuit()
    {
        SaveGame();
    }

    public void SetPersistenceList()
    {
        if (dataPersistenceList == null || dataPersistenceList.Count == 0)
        {
            dataPersistenceList = FindAllDataPersistence();
        }
    }

    public void ResetGame()
    {
        gameData = new GameData();
        SaveGame();
    }

}