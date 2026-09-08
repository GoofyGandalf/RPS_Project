public static class ChoiceRules
{
    // Centralized lookup for which choice beats another in the current rule set.
    public static bool DoesChoiceDefeat(int playerChoice, int opponentChoice)
    {
        return (playerChoice == 3 && opponentChoice == 2) ||
            (playerChoice == 2 && opponentChoice == 1) ||
            (playerChoice == 1 && opponentChoice == 3) ||
            (playerChoice == 4 && (opponentChoice == 2 || opponentChoice == 3)) ||
            (playerChoice == 5 && (opponentChoice == 1 || opponentChoice == 3)) ||
            (playerChoice == 6 && (opponentChoice == 1 || opponentChoice == 2)) ||
            (playerChoice == 1 && opponentChoice == 4) ||
            (playerChoice == 2 && opponentChoice == 5) ||
            (playerChoice == 3 && opponentChoice == 6);
    }
}
