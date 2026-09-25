using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChoiceDisplayUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text playerChoiceText;

    [SerializeField]
    private TMP_Text enemyChoiceText;

    [SerializeField]
    private TMP_Text playerLingeringChoiceText_1;
    [SerializeField]
    private TMP_Text playerLingeringChoiceText_2;
    [SerializeField]
    private TMP_Text playerLingeringChoiceText_3;
    [SerializeField]
    private TMP_Text playerLingeringChoiceText_4;
    [SerializeField]
    private TMP_Text enemyLingeringChoiceText_1;
    [SerializeField]
    private TMP_Text enemyLingeringChoiceText_2;
    [SerializeField]
    private TMP_Text enemyLingeringChoiceText_3;
    [SerializeField]
    private TMP_Text enemyLingeringChoiceText_4;

    private void Awake()
    {
        Clear();
    }

    public void DisplayChoices(int playerChoice, int enemyChoice, bool highlightEnemyChoice)
    {
        playerChoiceText.text = GetChoiceName(playerChoice);
        enemyChoiceText.text = GetChoiceName(enemyChoice);
        enemyChoiceText.color = highlightEnemyChoice ? Color.yellow : Color.white;
    }

    public void DisplayLingeringChoices(List<int> playerChoices, List<int> enemyChoices, int specialCountdownIndex)
    {
        TMP_Text[] playerLingeringChoices = { playerLingeringChoiceText_1, playerLingeringChoiceText_2, playerLingeringChoiceText_3, playerLingeringChoiceText_4 };
        TMP_Text[] enemyLingeringChoices = { enemyLingeringChoiceText_1, enemyLingeringChoiceText_2, enemyLingeringChoiceText_3, enemyLingeringChoiceText_4 };

        for (int index = 0; index < playerLingeringChoices.Length; index++)
        {
            playerLingeringChoices[index].text = GetChoiceName(playerChoices[index]);
            enemyLingeringChoices[index].text = GetChoiceName(enemyChoices[index]);
            enemyLingeringChoices[index].color = index == specialCountdownIndex ? Color.yellow : Color.white;
        }
    }

    public void Clear()
    {
        playerChoiceText.text = string.Empty;
        enemyChoiceText.text = string.Empty;
        enemyChoiceText.color = Color.white;
    }

    public static string GetChoiceName(int choice)
    {
        switch (choice)
        {
            case 1: return "Rock";
            case 2: return "Paper";
            case 3: return "Scissors";
            case 4: return "Gun";
            case 5: return "Hydraulic Press";
            case 6: return "Balloon";
            default: return "Unknown";
        }
    }
}
