using TMPro;
using UnityEngine;

public class ChoiceDisplayUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text playerChoiceText;

    [SerializeField]
    private TMP_Text enemyChoiceText;

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
