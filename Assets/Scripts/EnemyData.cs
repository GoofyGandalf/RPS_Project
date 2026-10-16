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

    [SerializeField, Range(0f, 100f)]
    private float moveOneSpecialLikelihood;

    [SerializeField, Range(0f, 100f)]
    private float moveTwoSpecialLikelihood;

    [SerializeField, Range(0f, 100f)]
    private float moveThreeSpecialLikelihood;

    [SerializeField, Range(0f, 100f)]
    private float moveFourSpecialLikelihood;

    public string EnemyName => enemyName;
    public IReadOnlyList<int> MoveOneChoices => moveOneChoices;
    public IReadOnlyList<int> MoveTwoChoices => moveTwoChoices;
    public IReadOnlyList<int> MoveThreeChoices => moveThreeChoices;
    public IReadOnlyList<int> MoveFourChoices => moveFourChoices;
    public float MoveOneSpecialLikelihood => moveOneSpecialLikelihood;
    public float MoveTwoSpecialLikelihood => moveTwoSpecialLikelihood;
    public float MoveThreeSpecialLikelihood => moveThreeSpecialLikelihood;
    public float MoveFourSpecialLikelihood => moveFourSpecialLikelihood;
}