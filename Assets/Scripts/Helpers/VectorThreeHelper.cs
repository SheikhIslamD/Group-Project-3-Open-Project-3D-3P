using System.Collections;
using UnityEngine;
using System;

namespace Vector3Helper
{
    public enum Vector3Type { Position, Rotation, Direction }

    /// <summary>
    /// A Psudeo-Vector 3 Struct to represent Points and Positions in space.
    /// </summary>
    public struct Position
    {
        Vector3 value;
        public float x => value.x;
        public float y => value.y;
        public float z => value.z;

        public Position(float x, float y, float z)
        {
            value.x = x;
            value.y = y;
            value.z = z;
        }
        public Position(Vector3 value) => this.value = value;

        public static implicit operator Vector3(Position value) => value.value;
        public static implicit operator Position(Vector3 value) => new Position(value);

        public static bool operator ==(Position left, Position right) => left.value == right.value;
        public static bool operator !=(Position left, Position right) => left.value != right.value;
        public static bool operator ==(Position left, Vector3 right) => left.value == right;
        public static bool operator !=(Position left, Vector3 right) => left.value != right;
        public static bool operator ==(Vector3 left, Position right) => left == right.value;
        public static bool operator !=(Vector3 left, Position right) => left != right.value;

        public static Position operator +(Position primary, Position value) => new Position(primary.value + value.value);
        public static Position operator -(Position primary, Position value) => new Position(primary.value - value.value);
        public static Position operator *(Position primary, float value) => new Position(primary.value * value);
        public static Position operator /(Position primary, float value) => new Position(primary.value / value);
        public static Position operator *(Position primary, Position value)
        {
            Vector3 result = primary.value;
            result.x *= value.x; result.y *= value.y; result.z *= value.z;
            return new Position(result);
        }
        public static Position operator /(Position primary, Position value)
        {
            Vector3 result = primary.value;
            result.x /= value.x; result.y /= value.y; result.z /= value.z;
            return new Position(result);
        }
        public static Position operator +(Position primary, Direction value) => new Position(primary.value + value.value);
        public static Position operator -(Position primary, Direction value) => new Position(primary.value - value.value);


        public override bool Equals(object obj)
        {
            return obj is Position position &&
                   value.Equals(position.value) &&
                   x == position.x &&
                   y == position.y &&
                   z == position.z;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(value, x, y, z);
        }
    }

    /// <summary>
    /// A Psudeo-Vector 3 Struct to represent Euler Rotations around 3 axes.
    /// </summary>
    public struct Rotation
    {
        public Vector3 value;
        public float x => value.x;
        public float y => value.y;
        public float z => value.z;

        public Rotation(float x, float y, float z, Vector3Type type = Vector3Type.Rotation)
        {
            if (type == Vector3Type.Position) Debug.Log("Idk what you thought would happen feeding a Position into a Rotation. The Raw values have been used.");

            value.x = x;
            value.y = y;
            value.z = z;

            if (type == Vector3Type.Direction) value = value.DirToRot();
        }
        public Rotation(Vector3 value, Vector3Type type = Vector3Type.Rotation)
        {
            if (type == Vector3Type.Position) Debug.Log("Idk what you thought would happen feeding a Position into a Rotation. The Raw values have been used.");

            this.value = value;

            if (type == Vector3Type.Direction) this.value = value.DirToRot();
        }

        public static implicit operator Vector3(Rotation value) => value.value;
        public static implicit operator Rotation(Vector3 value) => new Rotation(value);

        public static explicit operator Rotation(Direction value) => new Rotation(((Vector3)value).DirToRot());
        
        public static bool operator ==(Rotation primary, Vector3 other) => primary.value == other;
        public static bool operator !=(Rotation primary, Vector3 other) => primary.value != other;
        public static bool operator ==(Rotation primary, Rotation other) => primary.value == other.value;
        public static bool operator !=(Rotation primary, Rotation other) => primary.value != other.value;
        public static bool operator ==(Rotation primary, Direction other) => primary.value == other.value.DirToRot();
        public static bool operator !=(Rotation primary, Direction other) => primary.value != other.value.DirToRot();

        public static Rotation operator +(Rotation primary, Rotation value) => new Rotation(primary.value + value.value);
        public static Rotation operator -(Rotation primary, Rotation value) => new Rotation(primary.value - value.value);

        public Direction ToDirection() => new Direction(value.RotToDir());

        public override bool Equals(object obj)
        {
            return obj is Rotation rotation &&
                   value.Equals(rotation.value) &&
                   x == rotation.x &&
                   y == rotation.y &&
                   z == rotation.z;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(value, x, y, z);
        }
    }

    /// <summary>
    /// A Psudeo-Vector 3 Struct to represent Directional Rotations.
    /// </summary>
    public struct Direction
    {
        public Vector3 value;
        public float x => value.x;
        public float y => value.y;
        public float z => value.z;
        public Vector3 normalized => value.normalized;

        public Direction(float x, float y, float z, Vector3Type type = Vector3Type.Direction, bool normalize = false)
        {
            value.x = x;
            value.y = y;
            value.z = z;

            if (type == Vector3Type.Rotation) value = Quaternion.LookRotation(new Vector3(x, y, z)).eulerAngles;
            if (normalize) value = value.normalized;
        }
        public Direction(Vector3 value, Vector3Type type = Vector3Type.Direction, bool normalize = false)
        {

            this.value = value;

            if (type == Vector3Type.Rotation) this.value = Quaternion.LookRotation(value).eulerAngles;
            if (normalize) value = value.normalized;
        }


        public static implicit operator Vector3(Direction value) => value.value;
        public static implicit operator Direction(Vector3 value) => new Direction(value);

        public static explicit operator Direction(Rotation value) => new Direction(((Vector3)value).RotToDir());

        public static bool operator ==(Direction primary, Vector3 other) => primary.value == other;
        public static bool operator !=(Direction primary, Vector3 other) => primary.value != other;
        public static bool operator ==(Direction primary, Direction other) => primary.value == other.value;
        public static bool operator !=(Direction primary, Direction other) => primary.value != other.value;
        public static bool operator ==(Direction primary, Rotation other) => primary.value == other.value.RotToDir();
        public static bool operator !=(Direction primary, Rotation other) => primary.value != other.value.RotToDir();

        public static Direction operator +(Direction primary, Direction value) => new Direction(primary.value + value.value);
        public static Direction operator -(Direction primary, Direction value) => new Direction(primary.value - value.value);
        public static Direction operator *(Direction primary, float value) => new Direction(primary.value * value);
        public static Direction operator /(Direction primary, float value) => new Direction(primary.value / value);
        public static Direction operator *(Direction primary, Direction value)
        {
            Vector3 result = primary.value;
            result.x *= value.x; result.y *= value.y; result.z *= value.z;
            return new Direction(result);
        }
        public static Direction operator /(Direction primary, Direction value)
        {
            Vector3 result = primary.value;
            result.x /= value.x; result.y /= value.y; result.z /= value.z;
            return new Direction(result);
        }

        public Rotation ToRotation() => new Rotation(value.RotToDir());

        public override bool Equals(object obj)
        {
            return obj is Direction direction &&
                   value.Equals(direction.value) &&
                   x == direction.x &&
                   y == direction.y &&
                   z == direction.z &&
                   normalized.Equals(direction.normalized);
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(value, x, y, z, normalized);
        }

        public Direction Rotate(float amount, Vector3 axis) => Quaternion.AngleAxis(amount, axis) * value;

        public static Direction up =      Vector3.up;
        public static Direction down =    Vector3.down;
        public static Direction right =   Vector3.right;
        public static Direction left =    Vector3.left;
        public static Direction forward = Vector3.forward;
        public static Direction back =    Vector3.back;


    }




    public static class Vector3ExtensionMethods
    {
        public static Vector3 DirToRot(this Vector3 value) => Quaternion.LookRotation(value.normalized).eulerAngles;
        public static Vector3 RotToDir(this Vector3 value) => Quaternion.Euler(value) * Vector3.forward;

        public static Position Position(this Vector3 value) => new Position(value);
        public static Rotation Rotation(this Vector3 value) => new Rotation(value);
        public static Direction Direction(this Vector3 value) => new Direction(value);

    }



}