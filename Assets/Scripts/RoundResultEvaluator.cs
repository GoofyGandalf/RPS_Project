public static class RoundResultEvaluator
{
    // Determines the normal outcome for a player versus enemy choice pair.
    public static string GetBaseResult(int selectedPlayerChoice, int enemyChoice)
    {
        bool isHighNumberMatchup = selectedPlayerChoice >= 4 || enemyChoice >= 4;

        if (selectedPlayerChoice >= 4 && enemyChoice >= 4)
            return "High Draw";

        if (selectedPlayerChoice == enemyChoice)
            return "No effect";

        if (ChoiceRules.DoesChoiceDefeat(selectedPlayerChoice, enemyChoice))
            return isHighNumberMatchup ? "Double Win" : "Win";

        if (ChoiceRules.DoesChoiceDefeat(enemyChoice, selectedPlayerChoice))
            return isHighNumberMatchup ? "Double Loss" : "Loss";

        return "No effect";
    }

    // Determines the result when the special move is missed on a special countdown.
    public static string GetMissedSpecialResult(int enemyChoice)
    {
        return enemyChoice >= 4 ? "Double Loss" : "Loss";
    }

    // Determines the result when the player successfully parries during a special countdown.
    public static string GetSuccessfulParryResult(int playerChoice)
    {
        return playerChoice >= 4 ? "Double Parry Win" : "Parry Win";
    }

    // Applies the computed outcome to player and enemy health.
    public static void ApplyResult(PlayerHandler playerHandler, string result)
    {
        switch (result)
        {
            case "Double Loss":
                playerHandler.SetHealth(-20f);
                playerHandler.SetEnemyHealth(20f);
                break;

            case "Loss":
                playerHandler.SetHealth(-20f);
                break;

            case "Double Win":
                playerHandler.SetHealth(20f);
                playerHandler.SetEnemyHealth(-20f);
                break;

            case "High Draw":
                playerHandler.SetHealth(20f);
                playerHandler.SetEnemyHealth(20f);
                break;

            case "Win":
                playerHandler.SetEnemyHealth(-20f);
                break;

            case "Parry Win":
                playerHandler.SetHealth(20f);
                playerHandler.SetEnemyHealth(-20f);
                break;

            case "Double Parry Win":
                playerHandler.SetHealth(40f);
                playerHandler.SetEnemyHealth(-40f);
                break;

            case "No effect":
                break;
        }
    }
}
