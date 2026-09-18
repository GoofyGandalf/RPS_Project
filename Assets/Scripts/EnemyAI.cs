using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "RPS/Enemy Data")]
public class EnemyData : ScriptableObject
{
    [SerializeField]
    private string enemyName = "Enemy";

    [SerializeField]
    private List<int> moveOneChoices = new List<int> { 1, 2, 3, 4, 5, 6 };

    [SerializeField]
    private List<int> moveTwoChoices = new List<int> { 1, 2, 3, 4, 5, 6 };

    [SerializeField]
    private List<int> moveThreeChoices = new List<int> { 1, 2, 3, 4, 5, 6 };

    [SerializeField]
    private List<int> moveFourChoices = new List<int> { 1, 2, 3, 4, 5, 6 };

    public string EnemyName => enemyName;
    public IReadOnlyList<int> MoveOneChoices => moveOneChoices;
    public IReadOnlyList<int> MoveTwoChoices => moveTwoChoices;
    public IReadOnlyList<int> MoveThreeChoices => moveThreeChoices;
    public IReadOnlyList<int> MoveFourChoices => moveFourChoices;
}

public class EnemyAI
{
    private readonly EnemyData enemyData;

    public EnemyAI(EnemyData enemyData)
    {
        this.enemyData = enemyData;
    }

    public List<int> GenerateChoices()
    {
        List<int> choices = new List<int>(4);
        choices.Add(GetRandomChoice(enemyData.MoveOneChoices));
        choices.Add(GetRandomChoice(enemyData.MoveTwoChoices));
        choices.Add(GetRandomChoice(enemyData.MoveThreeChoices));
        choices.Add(GetRandomChoice(enemyData.MoveFourChoices));
        return choices;
    }

    private int GetRandomChoice(IReadOnlyList<int> availableChoices)
    {
        return availableChoices[Random.Range(0, availableChoices.Count)];
    }
}