using System.Collections.Generic;
using UnityEngine;

public class EnemyProgressionManager : MonoBehaviour
{
    [SerializeField]
    private List<EnemyAI> enemiesInOrder = new List<EnemyAI>();

    [SerializeField]
    private int currentEnemyIndex;

    public EnemyAI CurrentEnemy => GetEnemyAt(currentEnemyIndex);

    public EnemyAI GetEnemyAt(int enemyIndex)
    {
        if (enemyIndex < 0 || enemyIndex >= enemiesInOrder.Count)
            return null;

        return enemiesInOrder[enemyIndex];
    }

    public bool ChallengeNextEnemy()
    {
        if (currentEnemyIndex + 1 >= enemiesInOrder.Count)
            return false;

        currentEnemyIndex++;
        return true;
    }
}