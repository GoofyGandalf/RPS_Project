using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class EnemyProgressionManager : MonoBehaviour
{
    private const string CurrentEnemyIndexKey = "RPS_Project.CurrentEnemyIndex";

    [SerializeField]
    private List<EnemyAI> enemiesInOrder = new List<EnemyAI>();

    [SerializeField]
    private int currentEnemyIndex;

    public int CurrentEnemyIndex => currentEnemyIndex;
    public EnemyAI CurrentEnemy => GetEnemyAt(currentEnemyIndex);
    public static bool HasSavedProgress => PlayerPrefs.GetInt(CurrentEnemyIndexKey, 0) > 0;

    private void Awake()
    {
        int savedEnemyIndex = PlayerPrefs.GetInt(CurrentEnemyIndexKey, currentEnemyIndex);
        currentEnemyIndex = enemiesInOrder.Count == 0 ? 0 : Mathf.Clamp(savedEnemyIndex, 0, enemiesInOrder.Count - 1);
        SaveProgress();
    }

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
        SaveProgress();
        return true;
    }

    public static void ResetSavedProgress()
    {
        PlayerPrefs.DeleteKey(CurrentEnemyIndexKey);
        PlayerPrefs.Save();
    }

    private void SaveProgress()
    {
        PlayerPrefs.SetInt(CurrentEnemyIndexKey, currentEnemyIndex);
        PlayerPrefs.Save();
    }
}