using System.Collections;
using UnityEngine;


public static class ExtensionMethods
{
    public static bool Toggle(this ref bool boolean)
    {
        boolean = !boolean;
        return boolean;
    }
}
