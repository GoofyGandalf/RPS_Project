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
    private readonly EnemyAI enemyAI;

    private int clickCount;

    public RoundInputBuffer(EnemyAI enemyAI)
    {
        this.enemyAI = enemyAI;
    }

    // Adds one player choice to the current group and returns a completed batch when four entries are collected.
    public RoundBatch AddChoice(int selectedPlayerChoice)
    {
        clickCount++;

        playerChoicesThisGroup.Add(selectedPlayerChoice);

        if (clickCount % 4 != 0)
            return null;

        RoundBatch batch = new RoundBatch(playerChoicesThisGroup, enemyAI.GenerateChoices());
        Clear();
        return batch;
    }

    public void Clear()
    {
        playerChoicesThisGroup.Clear();
    }
}
