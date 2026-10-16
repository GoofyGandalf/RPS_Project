using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void StartBattle()
    {
        SceneManager.LoadScene("BattleScreen");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
