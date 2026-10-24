using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    private Button continueButton;

    private void Awake()
    {
        continueButton.interactable = EnemyProgressionManager.HasSavedProgress;
    }

    public void StartBattle()
    {
        EnemyProgressionManager.ResetSavedProgress();
        SceneManager.LoadScene("BattleScreen");
    }

    public void ContinueBattle()
    {
        if (!EnemyProgressionManager.HasSavedProgress)
            return;

        SceneManager.LoadScene("BattleScreen");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
