using System;
using System.Collections.Generic;

public static class FaceOffGlobalInformation
{
    public static List<int> animatingGameObjectIds = new List<int>();

    public static bool noAnimations()
    {
        return animatingGameObjectIds.Count == 0;
    }

    public static void objectAnimating(int objectId)
    {
        if (!FaceOffGlobalInformation.animatingGameObjectIds.Contains(objectId))
        {
            FaceOffGlobalInformation.animatingGameObjectIds.Add(objectId);
        }
    }

    public static void objectDoneAnimating(int objectId)
    {
        FaceOffGlobalInformation.animatingGameObjectIds.Remove(objectId);
    }
}
