using System;
public enum FaceOffStatus
{
    Start,
    WaitingForPlayerToPlayCard,
    WaitingForEnemyToPlayCard,
    CheckForCardsToPlay,
    DetermineNextStep,
    ChooseDuelCards,
    Duel,
    AfterDuel,
    Waiting,
    Animating,
    End
}
