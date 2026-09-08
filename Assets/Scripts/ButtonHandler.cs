using UnityEngine;

public class ButtonHandler : MonoBehaviour
{
    [SerializeField]
    private RoundManager roundManager;

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
