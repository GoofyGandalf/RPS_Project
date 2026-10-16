using System.Collections.Generic;
using UnityEngine;

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