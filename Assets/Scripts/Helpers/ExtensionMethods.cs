using System.Collections;
using UnityEngine;


public static class ExtensionMethods
{
    public static bool Toggle(this ref bool boolean)
    {
        boolean = !boolean;
        return boolean;
    }

    /*

    public static T Get<T>(this ref T c, MonoBehaviour mono) where T : struct
    {
        Component C = mono.GetComponent<Component>();
        c = C;
        return c;
    }
     */
}
