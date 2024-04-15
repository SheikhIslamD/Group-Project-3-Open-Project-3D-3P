using System;
using UnityEngine;

namespace TrigHelper
{
    internal enum AngleType { Degrees, Radians }

    /// <summary>
    /// An Angle as described from 0 to 360.
    /// </summary>
    public struct Degree
    {
        public float value;
        public Degree(float value) => this.value = value;
        public Degree(Radian value) => this.value = value.value * Mathf.Rad2Deg;

        public static implicit operator float(Degree deg) => deg.value;
        public static implicit operator Degree(float input) => new(input);

        public static explicit operator Degree(Radian input) => new(input.value * Mathf.Rad2Deg);
        public static explicit operator Degree(Revolution input) => new(input.value * FullCircle);

        public static bool operator ==(Degree primary, Degree other) => primary.value == other.value;
        public static bool operator !=(Degree primary, Degree other) => primary.value != other.value;
        public static bool operator ==(Degree primary, Radian other) => primary.value == (Degree)other.value;
        public static bool operator !=(Degree primary, Radian other) => primary.value != (Degree)other.value;
        public static bool operator ==(Degree primary, Revolution other) => primary.value == (Degree)other.value;
        public static bool operator !=(Degree primary, Revolution other) => primary.value != (Degree)other.value;

        public static Degree operator +(Degree primary, Degree value) => new(primary.value + value);
        public static Degree operator -(Degree primary, Degree value) => new(primary.value - value);
        public static Degree operator *(Degree primary, Degree value) => new(primary.value * value);
        public static Degree operator /(Degree primary, Degree value) => new(primary.value / value);

        public static Degree operator ++(Degree primary) => new(primary.value + 1f);
        public static Degree operator --(Degree primary) => new(primary.value - 1f);

        public override bool Equals(object obj)
        {
            return obj is Degree degree &&
                   value == degree.value;
        }
        public override int GetHashCode() => HashCode.Combine(value);

        public float Sin() => Mathf.Sin(value);
        public float Cos() => Mathf.Cos(value);
        public float Tan() => Mathf.Tan(value);

        public Radian ToRadians() => new(value * Mathf.Deg2Rad);
        public Revolution ToRevolutions() => new(value / FullCircle);

        public const float FullCircle = 360;
        public const float HalfCircle = 180;
        public const float QuarterCircle = 90;


        public Degree ClampToCircle() => value % FullCircle;
        public Degree ClampToCircleMirrored() => value % (FullCircle * (value >= 0 ? 1 : -1));
        public Degree ClampToHalfCircleMirrored() => ((value + HalfCircle) % FullCircle) - HalfCircle;

    }
    /// <summary>
    /// An Angle as described from 0 to 2*pi.
    /// </summary>
    public struct Radian
    {
        public float value;
        public Radian(float value) => this.value = value;
        public Radian(Degree value) => this.value = value.value * Mathf.Deg2Rad;

        public static implicit operator float(Radian deg) => deg.value;
        public static implicit operator Radian(float input) => new(input);

        public static explicit operator Radian(Degree input) => new(input.value * Mathf.Deg2Rad);
        public static explicit operator Radian(Revolution input) => new(input.value * FullCircle);

        public static bool operator ==(Radian primary, Radian other) => primary.value == other.value;
        public static bool operator !=(Radian primary, Radian other) => primary.value != other.value;
        public static bool operator ==(Radian primary, Degree other) => primary.value == (Radian)other.value;
        public static bool operator !=(Radian primary, Degree other) => primary.value != (Radian)other.value;
        public static bool operator ==(Radian primary, Revolution other) => primary.value == (Radian)other.value;
        public static bool operator !=(Radian primary, Revolution other) => primary.value != (Radian)other.value;

        public static Radian operator +(Radian primary, Radian value) => new(primary.value + value);
        public static Radian operator -(Radian primary, Radian value) => new(primary.value - value);
        public static Radian operator *(Radian primary, Radian value) => new(primary.value * value);
        public static Radian operator /(Radian primary, Radian value) => new(primary.value / value);

        public override bool Equals(object obj)
        {
            return obj is Radian degree &&
                   value == degree.value;
        }
        public override int GetHashCode() => HashCode.Combine(value);

        public Degree ToDegrees() => new(value * Mathf.Rad2Deg);
        public Revolution ToRevolutions() => new(value / FullCircle);

        public const float FullCircle = 2 * Mathf.PI;
        public const float HalfCircle = Mathf.PI;
        public const float QuarterCircle = Mathf.PI / 2;


        public Radian ClampToCircle() => value % FullCircle;
        public Radian ClampToCircleMirrored() => value % (FullCircle * (value >= 0 ? 1 : -1));
        public Radian ClampToHalfCircleMirrored() => ((value + HalfCircle) % FullCircle) - HalfCircle;

        public float Sin() => Mathf.Sin(value);
        public float Cos() => Mathf.Cos(value);
        public float Tan() => Mathf.Tan(value);

        public static float Sin(Radian input) => Mathf.Sin(input.value);
        public static float Cos(Radian input) => Mathf.Cos(input.value);
        public static float Tan(Radian input) => Mathf.Tan(input.value);
        public static Radian ASin(float input) => new(Mathf.Asin(input));
        public static Radian ACos(float input) => new(Mathf.Acos(input));
        public static Radian ATan(float input) => new(Mathf.Atan(input));

    }
    /// <summary>
    /// An Angle as described from 0 to 1.
    /// </summary>
    public struct Revolution
    {
        public float value;
        public Revolution(float value) => this.value = value;

        public static implicit operator float(Revolution deg) => deg.value;
        public static implicit operator Revolution(float input) => new(input);

        public static explicit operator Revolution(Degree input) => new(input.value / Degree.FullCircle);
        public static explicit operator Revolution(Radian input) => new(input.value / Radian.FullCircle);

        public static bool operator ==(Revolution primary, Revolution other) => primary.value == other.value;
        public static bool operator !=(Revolution primary, Revolution other) => primary.value != other.value;
        public static bool operator ==(Revolution primary, Degree other) => primary.value == (Revolution)other.value;
        public static bool operator !=(Revolution primary, Degree other) => primary.value != (Revolution)other.value;
        public static bool operator ==(Revolution primary, Radian other) => primary.value == (Revolution)other.value;
        public static bool operator !=(Revolution primary, Radian other) => primary.value != (Revolution)other.value;

        public static Revolution operator +(Revolution primary, Revolution value) => new(primary.value + value);
        public static Revolution operator -(Revolution primary, Revolution value) => new(primary.value - value);
        public static Revolution operator *(Revolution primary, Revolution value) => new(primary.value * value);
        public static Revolution operator /(Revolution primary, Revolution value) => new(primary.value / value);

        public static Revolution operator ++(Revolution primary) => new(primary.value + 1f);
        public static Revolution operator --(Revolution primary) => new(primary.value - 1f);


        public override bool Equals(object obj)
        {
            return obj is Revolution degree &&
                   value == degree.value;
        }
        public override int GetHashCode() => HashCode.Combine(value);

        public Radian ToRadians() => new(value * Radian.FullCircle);
        public Degree ToDegrees() => new(value * Degree.FullCircle);

        public const float FullCircle = 1;
        public const float HalfCircle = 0.5f;
        public const float QuarterCircle = 0.25f;


        public Revolution ClampToCircle() => value % FullCircle;
        public Revolution ClampToCircleMirrored() => value % (FullCircle * (value >= 0 ? 1 : -1));
        public Revolution ClampToHalfCircleMirrored() => ((value + HalfCircle) % FullCircle) - HalfCircle;

    }

    public static class TrigHelperExtensionMethods
    {
        public static Degree Degree(this float F) => new(F);
        public static Radian Radian(this float F) => new(F);
        public static Revolution Revolution(this float F) => new(F);
    }

    /// <summary>
    /// An imaginary right Triangle for easy Trigonometric results.
    /// </summary>
    public struct Trigono
    {
        public Radian originAngle;

        public float lengthLine;
        public float sideLine;

        public Vector2 sidePoint => new(lengthLine, sideLine);
        public Radian sideAngle => Radian.ACos(sideLine / hypotenuseLine);
        public float hypotenuseLine => Vector2.Distance(Vector2.zero, new Vector2(lengthLine, sideLine));
        public const float rightAngle = Radian.QuarterCircle;


        public Trigono(float lengthLine, float sideLine)
        {
            this.lengthLine = lengthLine;
            this.sideLine = sideLine;

            originAngle = Radian.ATan(sideLine / lengthLine);
        }

        public Trigono(float lengthLine, Radian originAngle)
        {
            this.lengthLine = lengthLine;
            this.originAngle = originAngle;

            sideLine = lengthLine * originAngle.Tan();
        }

    }


}