using UnityEngine;

public class ButtonHandler : MonoBehaviour
{
    [SerializeField]
    private RoundManager roundManager;

    //called when the player clicks a button
    public void OnPlayerChoice(int playerChoice)
    {
        if (roundManager == null)
        {
            Debug.LogError("Assign a RoundManager to ButtonHandler in the Inspector.");
            return;
        }

        roundManager.OnPlayerChoice(playerChoice);
    }

}
