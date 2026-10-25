using System.Collections;
using TMPro;
using UnityEngine;

public class VictoryScreen : MonoBehaviour
{
    [SerializeField]
    private GameObject victoryPanel;

    [SerializeField]
    private TMP_Text victoryText;

    [SerializeField]
    private GameObject nextChallengerPanel;

    [SerializeField]
    private TMP_Text nextChallengerText;

    [SerializeField]
    [Min(0f)]
    private float victoryDisplayDuration = 1.25f;

    [SerializeField]
    [Min(0f)]
    private float nextChallengerDisplayDuration = 1.5f;

    private void Awake()
    {
        victoryPanel.SetActive(false);
        nextChallengerPanel.SetActive(false);
    }

    public IEnumerator ShowVictory()
    {
        victoryText.text = "Victory!";
        victoryPanel.SetActive(true);
        yield return new WaitForSecondsRealtime(victoryDisplayDuration);
        victoryPanel.SetActive(false);
    }

    public IEnumerator ShowNextChallenger(string challengerName)
    {
        nextChallengerText.text = $"Next Challenger\n{challengerName}";
        nextChallengerPanel.SetActive(true);
        yield return new WaitForSecondsRealtime(nextChallengerDisplayDuration);
        nextChallengerPanel.SetActive(false);
    }

    public void ShowFinalVictory()
    {
        victoryText.text = "You defeated all challengers!";
        victoryPanel.SetActive(true);
    }
}
