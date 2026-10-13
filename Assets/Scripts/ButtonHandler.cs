using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    private static readonly int[] HighChoiceUnlockEnemyIndexes = { 2, 4, 3 };

    [SerializeField]
    private RoundManager roundManager;

    [SerializeField]
    private Button[] regularButtons;

    [SerializeField]
    private Button[] highChoiceButtons;

    [SerializeField]
    private Button specialButton;

    private bool highChoiceButtonsAllowed;
    private int currentEnemyIndex;

    public void OnPlayerChoice(int playerChoice)
    {
        if (roundManager == null)
        {
            Debug.LogError("Assign a RoundManager to ButtonHandler in the Inspector.");
            return;
        }

        roundManager.OnPlayerChoice(playerChoice);
    }

    public void SetRegularButtonsInteractable(bool interactable)
    {
        if (regularButtons == null)
            return;

        foreach (Button currentButton in regularButtons)
        {
            if (currentButton != null && currentButton != specialButton)
                currentButton.interactable = interactable;
        }
    }

    public void SetSpecialButtonInteractable(bool interactable)
    {
        if (specialButton == null)
            return;

        specialButton.interactable = interactable;
    }

    public void SetHighChoiceButtonsInteractable(bool interactable, int enemyIndex)
    {
        if (highChoiceButtons == null)
            return;

        highChoiceButtonsAllowed = interactable;
        currentEnemyIndex = enemyIndex;

        for (int index = 0; index < highChoiceButtons.Length; index++)
        {
            Button currentButton = highChoiceButtons[index];
            if (currentButton == null || currentButton == specialButton)
                continue;

            int unlockIndex = index < HighChoiceUnlockEnemyIndexes.Length
                ? HighChoiceUnlockEnemyIndexes[index]
                : int.MaxValue;

            currentButton.interactable = highChoiceButtonsAllowed && currentEnemyIndex >= unlockIndex;
        }
    }
}
