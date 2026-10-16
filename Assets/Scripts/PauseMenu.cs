using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(RoundManager))]
public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseOverlay;

    private RoundManager roundManager;
    private bool isOpen;

    private void Awake()
    {
        roundManager = GetComponent<RoundManager>();

        if (roundManager == null)
        {
            Debug.LogError("PauseMenu must be attached to the same GameObject as RoundManager.", this);
            enabled = false;
            return;
        }

        if (pauseOverlay == null)
        {
            Debug.LogError("Assign the pause overlay in the PauseMenu Inspector.", this);
            enabled = false;
            return;
        }

        pauseOverlay.SetActive(false);
    }

    private void Update()
    {
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
}