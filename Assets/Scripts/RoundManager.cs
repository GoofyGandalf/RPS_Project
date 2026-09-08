using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class RoundManager : MonoBehaviour
{
    // Shared timing values for the queued result playback.
    private const float TimeBetweenCountdowns = 1.71f;
    private const float TimeAfterChoiceDisplayClears = 0.855f;

    [SerializeField]
    private PlayerHandler Players;

    [SerializeField]
    private CountdownUI countdownUI;

    [SerializeField]
    [FormerlySerializedAs("buttons")]
    private Button[] regularButtons;

    [SerializeField]
    private Button[] highChoiceButtons;

    [SerializeField]
    private Button specialButton;

    [SerializeField]
    private ChoiceDisplayUI choiceDisplayUI;

    [SerializeField]
    private int randomNumber;

    [SerializeField]
    private int clickCount;

    // Stores the current four-click batch until it is processed as a group.
    [SerializeField]
    private List<string> resultsThisGroup = new List<string>();

    private readonly List<bool> highEnemyResultsThisGroup = new List<bool>();
    private readonly List<int> playerChoicesThisGroup = new List<int>();
    private readonly List<int> enemyChoicesThisGroup = new List<int>();
    private readonly List<string> specialTimingAttempts = new List<string>();

    private bool isPlayingResults;
    private bool specialMoveRequested;
    private bool specialMoveUsedThisCountdown;
    private bool highEnemyNumberUsedThisExecution;
    private bool highChoiceUsedThisExecution;
    private bool currentCountdownIsSpecial;
    private bool timingAttemptRecorded;
    private int currentCountdownIndex;

    private void Awake()
    {
        // Guard against missing serialized references in the scene setup.
        if (regularButtons == null)
            regularButtons = new Button[0];

        if (highChoiceButtons == null)
            highChoiceButtons = new Button[0];

        SetHighChoiceButtonsInteractable(true);
        if (choiceDisplayUI != null)
            choiceDisplayUI.Clear();

        if (specialButton != null)
            specialButton.interactable = false;
    }

    // Used by the normal buttons that only feed the enemy roll and the result queue.
    // Player-selected choices are validated and tracked before they are added to the current group.
    public void OnPlayerChoice(int selectedPlayerChoice)
    {
        if (isPlayingResults || selectedPlayerChoice < 1 || selectedPlayerChoice > 6)
            return;

        if (selectedPlayerChoice >= 4)
        {
            if (highChoiceUsedThisExecution)
                return;

            highChoiceUsedThisExecution = true;
            SetHighChoiceButtonsInteractable(false);
        }

        ProcessButtonClick(true, selectedPlayerChoice);
    }

    // Every four clicks are batched together and then played back in order.
    private void ProcessButtonClick(bool isPlayerChoice, int selectedPlayerChoice = 0)
    {
        if (isPlayingResults)
            return;

        clickCount++;

        if (isPlayerChoice && clickCount % 4 == 1)
            highEnemyNumberUsedThisExecution = false;

        randomNumber = GenerateEnemyNumber(isPlayerChoice);

        bool isHighNumberMatchup = selectedPlayerChoice >= 4 || randomNumber >= 4;
        highEnemyResultsThisGroup.Add(randomNumber >= 4);
        playerChoicesThisGroup.Add(selectedPlayerChoice);
        enemyChoicesThisGroup.Add(randomNumber);

        if (isPlayerChoice && selectedPlayerChoice >= 4 && randomNumber >= 4)
            resultsThisGroup.Add("High Draw");
        else if (isPlayerChoice && selectedPlayerChoice == randomNumber)
            resultsThisGroup.Add("No effect");
        else if (isPlayerChoice && ChoiceRules.DoesChoiceDefeat(selectedPlayerChoice, randomNumber))
        {
            resultsThisGroup.Add(isHighNumberMatchup ? "Double Win" : "Win");
        }
        else if (isPlayerChoice && ChoiceRules.DoesChoiceDefeat(randomNumber, selectedPlayerChoice))
        {
            resultsThisGroup.Add(isHighNumberMatchup ? "Double Loss" : "Loss");
        }
        else if (isPlayerChoice)
            resultsThisGroup.Add("No effect");
        else if (randomNumber == 1)
        {
            resultsThisGroup.Add("Loss");
        }
        else if (randomNumber == 2)
        {
            resultsThisGroup.Add("Win");
        }
        else
            resultsThisGroup.Add("No effect");

        if (clickCount % 4 != 0)
            return;

        SetRegularButtonsInteractable(false);
        SetHighChoiceButtonsInteractable(false);

        StartCoroutine(ReadResultsAndSetHealth(
            new List<string>(resultsThisGroup),
            new List<bool>(highEnemyResultsThisGroup),
            new List<int>(playerChoicesThisGroup),
            new List<int>(enemyChoicesThisGroup)));
        resultsThisGroup.Clear();
        highEnemyResultsThisGroup.Clear();
        playerChoicesThisGroup.Clear();
        enemyChoicesThisGroup.Clear();
    }

    // High-value enemy rolls are limited to one per four-click batch.
    private int GenerateEnemyNumber(bool useExtendedNumbers)
    {
        if (!useExtendedNumbers || highEnemyNumberUsedThisExecution)
            return Random.Range(1, 4);

        int generatedNumber = Random.Range(1, 7);

        if (generatedNumber >= 4)
            highEnemyNumberUsedThisExecution = true;

        return generatedNumber;
    }

    // The special button can only be used once per current countdown and only records a hit when the timing window is open.
    public void OnSpecialButtonClick()
    {
        if (!isPlayingResults || specialMoveUsedThisCountdown)
            return;

        specialMoveUsedThisCountdown = true;
        timingAttemptRecorded = true;
        SetSpecialButtonInteractable(false);

        if (countdownUI == null || !countdownUI.IsSpecialTimingOpen)
        {
            if (currentCountdownIsSpecial && countdownUI != null)
            {
                double offset = countdownUI.GetSpecialTimingOffset();
                string direction = offset < 0d ? "early" : "late";
                specialTimingAttempts.Add(
                    $"Countdown {currentCountdownIndex + 1}: {Mathf.Abs((float)offset):0.000} seconds {direction}");
            }
            else
            {
                specialTimingAttempts.Add($"Countdown {currentCountdownIndex + 1}: not special");
            }

            return;
        }

        specialMoveRequested = true;
        specialTimingAttempts.Add($"Countdown {currentCountdownIndex + 1}: within window");
    }

    // This coroutine runs the queued results one by one and applies the corresponding health changes.
    private IEnumerator ReadResultsAndSetHealth(
        List<string> results,
        List<bool> highEnemyResults,
        List<int> playerChoices,
        List<int> enemyChoices)
    {
        isPlayingResults = true;
        specialMoveRequested = false;
        specialMoveUsedThisCountdown = false;
        highEnemyNumberUsedThisExecution = false;
        highChoiceUsedThisExecution = false;
        specialTimingAttempts.Clear();

        SetSpecialButtonInteractable(false);

        if (countdownUI != null)
            countdownUI.Clear();

        yield return new WaitForSecondsRealtime(3.42f);

        int specialCountdownIndex = Random.Range(0, results.Count);
        List<string> actualResults = new List<string>();

        Debug.Log($"Player choices: {FormatChoices(playerChoices)} | " +
            $"Enemy choices: {FormatChoices(enemyChoices)} | " +
            $"Results: [{string.Join(", ", results)}] | " +
            $"Special countdown: {specialCountdownIndex + 1} of {results.Count}");

        for (int index = 0; index < results.Count; index++)
        {
            if (Players.Health <= 0f || Players.Enemy_Health <= 0f)
            {
                break;
            }

            if (choiceDisplayUI != null)
                choiceDisplayUI.Clear();

            yield return new WaitForSecondsRealtime(TimeAfterChoiceDisplayClears);

            bool isSpecialCountdown = index == specialCountdownIndex;
            currentCountdownIndex = index;
            currentCountdownIsSpecial = isSpecialCountdown;
            timingAttemptRecorded = false;
            specialMoveUsedThisCountdown = false;
            SetSpecialButtonInteractable(true);

            if (countdownUI != null)
                yield return StartCoroutine(countdownUI.PlayCountdown(isSpecialCountdown));

            SetSpecialButtonInteractable(false);

            if (choiceDisplayUI != null)
                choiceDisplayUI.DisplayChoices(
                    playerChoices[index],
                    enemyChoices[index],
                    index == specialCountdownIndex);

            if (isSpecialCountdown && !timingAttemptRecorded)
                specialTimingAttempts.Add($"Countdown {index + 1}: no input");

            string resultToExecute = results[index];
            bool specialMoveSucceeded = isSpecialCountdown && specialMoveRequested;

            if (isSpecialCountdown && !specialMoveSucceeded)
                resultToExecute = highEnemyResults[index] ? "Double Loss" : "Loss";

            if (specialMoveSucceeded)
            {
                Players.SetHealth(20f);
                Players.SetEnemyHealth(-20f);
                specialMoveRequested = false;
                resultToExecute = "Double Win";
            }

            switch (resultToExecute)
            {
                case "Double Loss":
                    Players.SetHealth(-20f);
                    Players.SetEnemyHealth(20f);
                    break;

                case "Loss":
                    Players.SetHealth(-20f);
                    break;

                case "Double Win":
                    Players.SetHealth(20f);
                    Players.SetEnemyHealth(-20f);
                    break;

                case "High Draw":
                    Players.SetHealth(20f);
                    Players.SetEnemyHealth(20f);
                    break;

                case "Win":
                    Players.SetEnemyHealth(-20f);
                    break;
            }

            actualResults.Add(resultToExecute);

            if (Players.Health <= 0f || Players.Enemy_Health <= 0f)
            {
                break;
            }

            if (index < results.Count - 1)
                yield return new WaitForSecondsRealtime(TimeBetweenCountdowns);
            else
                yield return new WaitForSecondsRealtime(TimeBetweenCountdowns);
        }

        isPlayingResults = false;

        if (countdownUI != null)
            countdownUI.Clear();

        if (choiceDisplayUI != null)
            choiceDisplayUI.Clear();

        SetSpecialButtonInteractable(false);

        SetRegularButtonsInteractable(true);
        SetHighChoiceButtonsInteractable(true);

        Debug.Log($"Actual results executed: [{string.Join(", ", actualResults)}]");
        Debug.Log($"Special timing attempts: [{string.Join(", ", specialTimingAttempts)}]");

    }

    // Centralized button state helper for locking or unlocking the regular inputs.
    private void SetRegularButtonsInteractable(bool interactable)
    {
        foreach (Button currentButton in regularButtons)
        {
            if (currentButton != null && currentButton != specialButton)
                currentButton.interactable = interactable;
        }
    }

    // Keeps the special button enabled only during the current countdown window.
    private void SetSpecialButtonInteractable(bool interactable)
    {
        if (specialButton == null)
            return;

        specialButton.interactable = interactable;
    }

    // The high-choice buttons are disabled after their first use in the current group.
    private void SetHighChoiceButtonsInteractable(bool interactable)
    {
        foreach (Button currentButton in highChoiceButtons)
        {
            if (currentButton != null && currentButton != specialButton)
                currentButton.interactable = interactable;
        }
    }

    // Converts the stored choice integers into readable labels for logging.
    private string FormatChoices(List<int> choices)
    {
        List<string> names = new List<string>();

        foreach (int choice in choices)
            names.Add(ChoiceDisplayUI.GetChoiceName(choice));

        return $"[{string.Join(", ", names)}]";
    }

}
