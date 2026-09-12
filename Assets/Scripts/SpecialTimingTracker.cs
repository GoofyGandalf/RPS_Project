public class SpecialTimingTracker
{
    private bool specialMoveRequested;
    private bool specialMoveUsedThisCountdown;

    public bool SpecialMoveRequested => specialMoveRequested;

    public void StartExecution()
    {
        specialMoveRequested = false;
        specialMoveUsedThisCountdown = false;
    }

    // Records whether the special button was pressed during an open timing window.
    public bool HandleSpecialButtonClick(bool isPlayingResults, CountdownUI countdownUI)
    {
        if (!isPlayingResults || specialMoveUsedThisCountdown)
            return false;

        specialMoveUsedThisCountdown = true;

        if (countdownUI == null || !countdownUI.IsSpecialTimingOpen)
            return false;

        specialMoveRequested = true;
        return true;
    }

    // Clears the pending special-move request after it has been handled.
    public void ConsumeSpecialMoveRequest()
    {
        specialMoveRequested = false;
    }
}
