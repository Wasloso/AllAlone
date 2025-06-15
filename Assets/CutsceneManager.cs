using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

// Or UnityEngine.Video if using VideoPlayer


public class CutsceneManager : MonoBehaviour
{
    [TextArea] public string CutsceneText;
    [SerializeField] private TMP_Text signText;

    private readonly float typingSpeed = 0.05f; // seconds between each character
    private int currentPart;

    private string[] textParts;

    private Coroutine typingCoroutine;

    private void Start()
    {
        textParts = CutsceneText.Split('\n');
        currentPart = 0;
        if (textParts.Length > 0) StartTypingText(textParts[currentPart]);
    }

    private void Update()
    {
    }

    private void StartTypingText(string text)
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeTextRoutine(text));
    }

    private IEnumerator TypeTextRoutine(string fullText)
    {
        signText.text = "";
        if (fullText.Length == 0)
        {
            typingCoroutine = null;
            AdvanceText();
            yield break;
        }

        foreach (var c in fullText)
        {
            signText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        typingCoroutine = null;

        yield return new WaitForSeconds(2f);

        AdvanceText();
    }

    private void AdvanceText()
    {
        currentPart++;
        if (currentPart >= textParts.Length)
            ProceedToGame();
        else
            StartTypingText(textParts[currentPart]);
    }


    private void OnCutsceneFinished(PlayableDirector pd)
    {
        ProceedToGame();
    }

    public void ProceedToGame()
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