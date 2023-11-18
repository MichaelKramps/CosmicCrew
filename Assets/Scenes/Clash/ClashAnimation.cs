using System;
public class ClashAnimation
{
    ActingPlayer actingPlayer = ActingPlayer.None;
    ClashAnimationType animationType = ClashAnimationType.None;
    int primaryInteger = 0;
    int secondaryInteger = 0;

    public ClashAnimation(string animationCodeFromServer)
    {
        string[] splitCode = animationCodeFromServer.Split(",");

        if (splitCode.Length == 4)
        {
            this.actingPlayer = determineActingPlayer(splitCode[0]);
            this.animationType = determineAnimationType(splitCode[1]);
            this.primaryInteger = determineIntegerValue(splitCode[2]);
            this.secondaryInteger = determineIntegerValue(splitCode[3]);
        }
    }

    private ActingPlayer determineActingPlayer(string serverCode)
    {
        switch (serverCode.ToLower())
        {
            case "b":
                return ActingPlayer.Both;
            case "p":
                return ActingPlayer.Primary;
            case "s":
                return ActingPlayer.Secondary;
            default:
                return ActingPlayer.None;
        }
    }

    private ClashAnimationType determineAnimationType(string serverCode)
    {
        switch (serverCode.ToLower())
        {
            case "w":
                return ClashAnimationType.Wait;
            case "dws":
                return ClashAnimationType.DrawCard;
            case "gtc":
                return ClashAnimationType.GetTopCard;
            case "p":
                return ClashAnimationType.PlayCard;
            case "r":
                return ClashAnimationType.RollDice;
            case "sd":
                return ClashAnimationType.ShowDice;
            case "hd":
                return ClashAnimationType.HideDice;
            case "sf":
                return ClashAnimationType.SelectFighters;
            case "g1":
                return ClashAnimationType.FighterOneWins;
            case "g2":
                return ClashAnimationType.FighterTwoWins;
            case "gt":
                return ClashAnimationType.FightersTie;
            case "ppo":
                return ClashAnimationType.PreparePowerCounter;
            case "pow":
                return ClashAnimationType.PowerCounter;
            case "end":
                return ClashAnimationType.EndFight;
            default:
                return ClashAnimationType.None;
        }
    }

    private int determineIntegerValue(string serverCode)
    {
        try
        {
            return Int32.Parse(serverCode);
        }
        catch
        {
            return 0;
        }
    }

    public ClashAnimationType getAnimationType()
    {
        return animationType;
    }

    public ActingPlayer getActingPlayer()
    {
        return actingPlayer;
    }

    public int getPrimaryInteger()
    {
        return primaryInteger;
    }

    public int getSecondaryInteger()
    {
        return secondaryInteger;
    }
}