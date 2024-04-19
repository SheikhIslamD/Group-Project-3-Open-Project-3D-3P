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

public static class EasierMathExtensions
{
    public static float P(this float F) => Mathf.Pow(F, 2);
    public static float P(this float F, int power) => Mathf.Pow(F, power);
    public static float SQRT(this float F) => Mathf.Sqrt(F);
    public static float Sin(this float F) => Mathf.Sin(F);
    public static float Cos(this float F) => Mathf.Cos(F);
    public static float Tan(this float F) => Mathf.Tan(F);
    public static float ASin(this float F) => Mathf.Asin(F);
    public static float ACos(this float F) => Mathf.Acos(F);
    public static float ATan(this float F) => Mathf.Atan(F);

    public static float Clamp(this float value, float min, float max) => (value < min) ? min : (value > max) ? max : value;
    public static float Min(this float value, float min) => (value < min) ? min : value;
    public static float Max(this float value, float max) => (value > max) ? max : value;

    public static int Int(this float value) => (int)value;
    public static float Float(this int value) => (float)value;
    public static int Floor(this float value) => Mathf.FloorToInt(value);
    public static int Ceil(this float value) => Mathf.CeilToInt(value);

    public static int Sign(this float value) => (int)Mathf.Sign(value);
    public static float Abs(this float value) => Mathf.Abs(value);
    public static float Repeat(this float value, float length) => Mathf.Repeat(value, length);

}
