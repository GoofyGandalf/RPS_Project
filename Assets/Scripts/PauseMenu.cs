using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(RoundManager))]
public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseOverlay;

    [SerializeField]
    private TMP_Text pauseOverlayText;

    [SerializeField]
    private TMP_Text resumeButtonText;

    private const string PausedText = "Paused";
    private const string ResumeText = "Resume";
    private const string PlayerLostText = "You Lost";
    private const string TryAgainText = "Try Again";

    private RoundManager roundManager;
    private bool isOpen;
    private bool hasLost;

    private void Awake()
    {
        roundManager = GetComponent<RoundManager>();
        roundManager.PlayerDefeated += HandlePlayerDefeat;
        pauseOverlayText.text = PausedText;
        resumeButtonText.text = ResumeText;
        pauseOverlay.SetActive(false);
    }

    private void OnDestroy()
    {
        roundManager.PlayerDefeated -= HandlePlayerDefeat;
    }

    private void Update()
    {
        if (hasLost)
            return;

        if (Keyboard.current == null || !Keyboard.current.escapeKey.wasPressedThisFrame)
            return;

        if (isOpen || !roundManager.IsRoundExecutionActive)
            SetOpen(!isOpen);
    }

    private void SetOpen(bool open)
    {
        if (isOpen == open)
            return;

        isOpen = open;
        pauseOverlay.SetActive(open);
    }

    public void ResumeGame()
    {
        if (hasLost)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            return;
        }

        SetOpen(false);
    }

    public void ReturnToMenu()
    {
        SetOpen(false);
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void HandlePlayerDefeat()
    {
        hasLost = true;
        pauseOverlayText.text = PlayerLostText;
        resumeButtonText.text = TryAgainText;
        SetOpen(true);
    }
}