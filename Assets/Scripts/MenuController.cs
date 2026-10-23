using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void StartBattle()
    {
        EnemyProgressionManager.ResetSavedProgress();
        SceneManager.LoadScene("BattleScreen");
    }

    public void ContinueBattle()
    {
        SceneManager.LoadScene("BattleScreen");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
