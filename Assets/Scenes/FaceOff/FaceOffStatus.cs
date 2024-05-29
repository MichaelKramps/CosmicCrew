using System;
public enum FaceOffStatus
{
    Start,
    WaitingForPlayerToPlayCard,
    WaitingForPlayerToSelectFanaticForGearAttachment,
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
