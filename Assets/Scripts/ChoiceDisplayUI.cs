using TMPro;
using UnityEngine;

public class ChoiceDisplayUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text playerChoiceText;

    [SerializeField]
    private TMP_Text enemyChoiceText;

    // Clears any previous choice display when the UI is initialized.
    private void Awake()
    {
        Clear();
    }

    // Shows the current player and enemy choices, with optional highlight styling.
    public void DisplayChoices(int playerChoice, int enemyChoice, bool highlightEnemyChoice)
    {
        if (playerChoiceText != null)
            playerChoiceText.text = GetChoiceName(playerChoice);

        if (enemyChoiceText != null)
        {
            enemyChoiceText.text = GetChoiceName(enemyChoice);
            enemyChoiceText.color = highlightEnemyChoice ? Color.yellow : Color.white;
        }
    }

    // Clears the displayed choices and resets the enemy highlight color.
    public void Clear()
    {
        if (playerChoiceText != null)
            playerChoiceText.text = string.Empty;

        if (enemyChoiceText != null)
        {
            enemyChoiceText.text = string.Empty;
            enemyChoiceText.color = Color.white;
        }
    }

    // Converts a numeric choice value into the matching display label.
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
