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

public class EnemyAI : MonoBehaviour
{
    [SerializeField]
    private EnemyData enemyData;

    public EnemyData Data => enemyData;

    public List<int> GenerateChoices()
    {
        //the previous list stays in memory right now, maybe use that to recap last turn?
        List<int> choices = new List<int>(4);
        bool highChoiceUsed = false;

        choices.Add(GetRandomChoice(enemyData.MoveOneChoices, ref highChoiceUsed));
        choices.Add(GetRandomChoice(enemyData.MoveTwoChoices, ref highChoiceUsed));
        choices.Add(GetRandomChoice(enemyData.MoveThreeChoices, ref highChoiceUsed));
        choices.Add(GetRandomChoice(enemyData.MoveFourChoices, ref highChoiceUsed));
        return choices;
    }

    public int GenerateSpecialCountdownIndex()
    {
        if (enemyData == null)
            return -1;

        float[] likelihoods =
        {
            enemyData.MoveOneSpecialLikelihood,
            enemyData.MoveTwoSpecialLikelihood,
            enemyData.MoveThreeSpecialLikelihood,
            enemyData.MoveFourSpecialLikelihood
        };

        float roll = Random.Range(0f, 100f);

        for (int index = 0; index < likelihoods.Length; index++)
        {
            roll -= Mathf.Max(0f, likelihoods[index]);

            if (roll < 0f)
                return index;
        }

        return likelihoods.Length;
    }

    private int GetRandomChoice(IReadOnlyList<int> availableChoices, ref bool highChoiceUsed)
    {
        List<int> validChoices = new List<int>();

        for (int index = 0; availableChoices != null && index < availableChoices.Count; index++)
        {
            int choice = availableChoices[index];

            if (!highChoiceUsed || choice < 4)
                validChoices.Add(choice);
        }

        int selectedChoice = validChoices.Count > 0
            ? validChoices[Random.Range(0, validChoices.Count)]
            : Random.Range(1, 4);

        if (selectedChoice >= 4)
            highChoiceUsed = true;

        return selectedChoice;
    }
}