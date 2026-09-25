using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private ButtonHandler buttonHandler;

    [SerializeField]
    private ChoiceDisplayUI choiceDisplayUI;

    [SerializeField]
    private EnemyProgressionManager enemyProgressionManager;

    private RoundInputBuffer roundInputBuffer;
    private readonly SpecialTimingTracker specialTimingTracker = new SpecialTimingTracker();

    private bool isPlayingResults;

    private void Awake()
    {
        roundInputBuffer = new RoundInputBuffer(enemyProgressionManager.CurrentEnemy);

        buttonHandler.SetHighChoiceButtonsInteractable(true);
        buttonHandler.SetSpecialButtonInteractable(false);
        choiceDisplayUI.Clear();
    }

    // Validates a player choice and queues it into the current batch.
    public void OnPlayerChoice(int selectedPlayerChoice)
    {
        if (isPlayingResults || selectedPlayerChoice < 1 || selectedPlayerChoice > 6)
            return;

        if (selectedPlayerChoice >= 4)
            buttonHandler.SetHighChoiceButtonsInteractable(false);

        ProcessButtonClick(selectedPlayerChoice);
    }

    private void ProcessButtonClick(int selectedPlayerChoice)
    {
        if (isPlayingResults)
            return;

        roundInputBuffer.SetEnemyAI(enemyProgressionManager.CurrentEnemy);
        RoundBatch batch = roundInputBuffer.AddChoice(selectedPlayerChoice);

        if (batch == null)
            return;

        buttonHandler.SetRegularButtonsInteractable(false);
        buttonHandler.SetHighChoiceButtonsInteractable(false);

        StartCoroutine(ReadResultsAndSetHealth(batch));
    }

    public void OnSpecialButtonClick()
    {
        specialTimingTracker.HandleSpecialButtonClick(isPlayingResults, countdownUI);
        buttonHandler.SetSpecialButtonInteractable(false);
    }

    // Plays out the queued batch one countdown at a time and applies the resulting health changes.
    private IEnumerator ReadResultsAndSetHealth(RoundBatch batch)
    {
        isPlayingResults = true;
        specialTimingTracker.StartExecution();

        buttonHandler.SetSpecialButtonInteractable(false);
        countdownUI.Clear();

        yield return new WaitForSecondsRealtime(3.42f);

        int specialCountdownIndex = enemyProgressionManager.CurrentEnemy
            .GenerateSpecialCountdownIndex();

        for (int index = 0; index < batch.PlayerChoices.Count; index++)
        {
            if (Players.Health <= 0f || Players.Enemy_Health <= 0f)
            {
                break;
            }

            choiceDisplayUI.Clear();

            yield return new WaitForSecondsRealtime(TimeAfterChoiceDisplayClears);

            bool isSpecialCountdown = index == specialCountdownIndex;
            buttonHandler.SetSpecialButtonInteractable(true);
            yield return StartCoroutine(countdownUI.PlayCountdown(isSpecialCountdown));

            buttonHandler.SetSpecialButtonInteractable(false);

            choiceDisplayUI.DisplayChoices(
                batch.PlayerChoices[index],
                batch.EnemyChoices[index],
                index == specialCountdownIndex);

            string resultToExecute = RoundResultEvaluator.GetBaseResult(
                batch.PlayerChoices[index],
                batch.EnemyChoices[index]);

            bool specialMoveSucceeded = isSpecialCountdown && specialTimingTracker.SpecialMoveRequested;

            if (isSpecialCountdown && !specialMoveSucceeded)
                resultToExecute = RoundResultEvaluator.GetMissedSpecialResult(batch.EnemyChoices[index]);

            if (specialMoveSucceeded)
            {
                specialTimingTracker.ConsumeSpecialMoveRequest();
                resultToExecute = RoundResultEvaluator.GetSuccessfulParryResult(batch.PlayerChoices[index]);
            }

            RoundResultEvaluator.ApplyResult(Players, resultToExecute);

            if (Players.Health <= 0f || Players.Enemy_Health <= 0f)
            {
                break;
            }

            if (index < batch.PlayerChoices.Count - 1)
                yield return new WaitForSecondsRealtime(TimeBetweenCountdowns);
            else
                yield return new WaitForSecondsRealtime(TimeBetweenCountdowns);
        }

        HandleEnemyDefeat();

        isPlayingResults = false;

        countdownUI.Clear();
        choiceDisplayUI.Clear();

        buttonHandler.SetSpecialButtonInteractable(false);
        buttonHandler.SetRegularButtonsInteractable(true);
        buttonHandler.SetHighChoiceButtonsInteractable(true);

    }

    private void HandleEnemyDefeat()
    {
        if (Players.Enemy_Health > 0f)
            return;

        enemyProgressionManager.ChallengeNextEnemy();
        Players.SetEnemyHealth(100f);
        Players.SetHealth(100f);
    }

}
