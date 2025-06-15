using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public int slotNum;
    public Button loadGame;
    public Button resetGame;
    [SerializeField] private TMP_Text slotText;
    private DataPersistenceManager dataPersistence;

    private void Awake()
    {
        dataPersistence = DataPersistenceManager.Instance;

        loadGame.onClick.AddListener(OnLoadGame);
        resetGame.onClick.AddListener(OnResetGame);
    }

    private void Start()
    {
        SetSlotText();
    }

    private void SetSlotText()
    {
        var exists = DataPersistenceManager.Instance.FileDataHandler.SaveExists("slot" + slotNum);
        if (slotText != null)
        {
            if (exists)
                slotText.text = "Game " + slotNum;
            else
                slotText.text = "None";
        }
    }

    private void OnLoadGame()
    {
        dataPersistence.currentGame = slotNum;
        dataPersistence.LoadGameScene();
    }


    private void OnResetGame()
    {
        dataPersistence.ResetGame(slotNum);
        SetSlotText();
    }
}