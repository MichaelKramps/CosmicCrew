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
            case "d":
                return ClashAnimationType.DrawCard;
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
}
