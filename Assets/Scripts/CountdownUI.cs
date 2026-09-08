using System.Collections;
using TMPro;
using UnityEngine;

public class CountdownUI : MonoBehaviour
{
    private const double CountdownInterval = 0.4275d;
    private const double SpecialTimingGracePeriod = 0.2d;

    [SerializeField]
    private TMP_Text countdownText;

    public int CurrentCountdown { get; private set; } = -1;
    public int SpecialTargetCountdown { get; private set; } = -1;
    public bool IsPlaying { get; private set; }
    // Allows the special move to be checked only during the highlighted countdown window.
    public bool IsSpecialTimingOpen =>
        SpecialTargetCountdown >= 0 &&
        CurrentCountdown == SpecialTargetCountdown &&
        Time.realtimeSinceStartupAsDouble <= GetSpecialTimingEndTime();
    private double specialTargetTime;
    private double specialTargetDisplayTime;

    private void Awake()
    {
        Clear();
    }

    // Runs the 3-2-1 countdown, optionally highlighting the special timing target.
    public IEnumerator PlayCountdown(bool isSpecialCountdown)
    {
        IsPlaying = true;
        double nextChangeTime = Time.realtimeSinceStartupAsDouble;

        if (isSpecialCountdown)
        {
            SpecialTargetCountdown = 2 - Random.Range(0, 3);
            specialTargetTime = nextChangeTime + (3 - SpecialTargetCountdown) * CountdownInterval;
        }
        else
        {
            SpecialTargetCountdown = -1;
            specialTargetTime = 0d;
        }

        for (int countdown = 3; countdown >= 0; countdown--)
        {
            CurrentCountdown = countdown;

            if (countdownText != null)
            {
                countdownText.text = countdown == 0 ? "Shoot!" : countdown.ToString();
                countdownText.color = isSpecialCountdown &&
                    countdown == SpecialTargetCountdown + 1 ? Color.yellow : Color.white;
            }

            if (countdown == SpecialTargetCountdown)
            {
                yield return new WaitForEndOfFrame();
                specialTargetDisplayTime = Time.realtimeSinceStartupAsDouble;
            }

            nextChangeTime += CountdownInterval;

            while (Time.realtimeSinceStartupAsDouble < nextChangeTime)
                yield return null;
        }

        IsPlaying = false;
        Clear();
    }

    public void Clear()
    {
        if (IsPlaying)
            return;

        CurrentCountdown = -1;
        SpecialTargetCountdown = -1;
        specialTargetTime = 0d;
        specialTargetDisplayTime = 0d;

        if (countdownText != null)
        {
            countdownText.text = string.Empty;
            countdownText.color = Color.white;
        }
    }

    // Returns how early or late the special button was pressed relative to the target moment.
    public double GetSpecialTimingOffset()
    {
        double timingReference = specialTargetDisplayTime > 0d
            ? specialTargetDisplayTime
            : specialTargetTime;
        double timingOffset = Time.realtimeSinceStartupAsDouble - timingReference;

        if (timingOffset > 0d)
            return timingOffset - SpecialTimingGracePeriod;

        return timingOffset;
    }

    private double GetSpecialTimingEndTime()
    {
        if (CurrentCountdown == SpecialTargetCountdown)
            return specialTargetDisplayTime + SpecialTimingGracePeriod;

        return specialTargetTime;
    }
}
