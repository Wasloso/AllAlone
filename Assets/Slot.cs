using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Slot: MonoBehaviour
{
    private DataPersistenceManager dataPersistence;
    public int slotNum;
    public Button loadGame;
    public Button resetGame;

    private void Awake()
    {
        dataPersistence = DataPersistenceManager.Instance;

        loadGame.onClick.AddListener(OnLoadGame);
        resetGame.onClick.AddListener(OnResetGame);

    }

    private void OnLoadGame()
    {
        dataPersistence.currentGame = slotNum;
        dataPersistence.LoadGameScene();

    }


    private void OnResetGame()
    {
        dataPersistence.currentGame = slotNum;
        dataPersistence.ResetGame();
    }
}