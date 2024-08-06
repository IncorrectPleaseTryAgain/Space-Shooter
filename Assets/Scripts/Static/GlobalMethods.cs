using System;
using UnityEngine;

internal static class GlobalMethods
{
    internal static bool DestroyObject(UnityEngine.Object obj)
    {
        try
        {
            UnityEngine.Object.Destroy(obj);
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }

        return false;
    }
}