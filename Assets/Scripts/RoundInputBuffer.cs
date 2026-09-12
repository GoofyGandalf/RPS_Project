using System.Collections.Generic;
using UnityEngine;

public class RoundBatch
{
    public RoundBatch(List<int> playerChoices, List<int> enemyChoices)
    {
        PlayerChoices = new List<int>(playerChoices);
        EnemyChoices = new List<int>(enemyChoices);
    }

    public List<int> PlayerChoices { get; }
    public List<int> EnemyChoices { get; }
}

public class RoundInputBuffer
{
    private readonly List<int> playerChoicesThisGroup = new List<int>();
    private readonly List<int> enemyChoicesThisGroup = new List<int>();

    private int clickCount;
    private bool highEnemyNumberUsedThisExecution;

    // Adds one player choice to the current group and returns a completed batch when four entries are collected.
    public RoundBatch AddChoice(int selectedPlayerChoice)
    {
        clickCount++;

        if (clickCount % 4 == 1)
            highEnemyNumberUsedThisExecution = false;

        int randomNumber = GenerateEnemyNumber();

        playerChoicesThisGroup.Add(selectedPlayerChoice);
        enemyChoicesThisGroup.Add(randomNumber);

        if (clickCount % 4 != 0)
            return null;

        RoundBatch batch = new RoundBatch(playerChoicesThisGroup, enemyChoicesThisGroup);
        Clear();
        return batch;
    }

    // Clears the buffered choices for the next four-click group.
    public void Clear()
    {
        playerChoicesThisGroup.Clear();
        enemyChoicesThisGroup.Clear();
    }

    // Generates the next enemy choice, enforcing the high-number cap for the current group.
    private int GenerateEnemyNumber()
    {
        if (highEnemyNumberUsedThisExecution)
            return Random.Range(1, 4);

        int generatedNumber = Random.Range(1, 7);

        if (generatedNumber >= 4)
            highEnemyNumberUsedThisExecution = true;

        return generatedNumber;
    }
}
