using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;

    public static DataPersistenceManager Instance { get; private set; }
    private GameData gameData;

    private List<IDataPersistence> dataPersistenceList;
    private FileDataHandler fileDataHandler;

    private void Start()
    {
        fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        if (Instance == null)
            Instance = this;
        dataPersistenceList = FindAllDataPersistence();
        LoadGame();

    }

    private List<IDataPersistence> FindAllDataPersistence()
    {
        IEnumerable<IDataPersistence> dataPersistences = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistences);
    }

    public void Awake()
    {
        if (Instance == null)
            Instance = this;

    }

    public void NewGame()
    {
        gameData = new GameData();
        Debug.Log("New Game!");
    }

    public void LoadGame()
    {
        gameData = fileDataHandler.Load();
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
        foreach (IDataPersistence dataPersistence in this.dataPersistenceList)
        {
            dataPersistence.SaveData(ref gameData);
        }

        fileDataHandler.Save(gameData);
    }

    public void OnApplicationQuit()
    {
        SaveGame();
    }

}