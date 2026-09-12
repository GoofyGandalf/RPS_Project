using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    [SerializeField]
    private RoundManager roundManager;

    [SerializeField]
    private Button[] regularButtons;

    [SerializeField]
    private Button[] highChoiceButtons;

    [SerializeField]
    private Button specialButton;

    // Forwards a UI button click to the round manager.
    public void OnPlayerChoice(int playerChoice)
    {
        if (roundManager == null)
        {
            Debug.LogError("Assign a RoundManager to ButtonHandler in the Inspector.");
            return;
        }

        roundManager.OnPlayerChoice(playerChoice);
    }

    // Updates the regular choice buttons enabled state.
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

    // Updates the special/parry button enabled state.
    public void SetSpecialButtonInteractable(bool interactable)
    {
        if (specialButton == null)
            return;

        specialButton.interactable = interactable;
    }

    // Updates the high-choice buttons enabled state.
    public void SetHighChoiceButtonsInteractable(bool interactable)
    {
        if (highChoiceButtons == null)
            return;

        foreach (Button currentButton in highChoiceButtons)
        {
            if (currentButton != null && currentButton != specialButton)
                currentButton.interactable = interactable;
        }
    }
}
