using System;
using UnityEngine;

namespace Vector3Helper
{
    public enum Vector3Type { Position, Rotation, Direction }

    /// <summary>
    /// A Psudeo-Vector 3 Struct to represent Points and Positions in space.
    /// </summary>
    public struct Position
    {
        private Vector3 vector;
        public float x { get => vector.x; set => vector.x = value; }
        public float y { get => vector.y; set => vector.y = value; }
        public float z { get => vector.z; set => vector.z = value; }

        public Position(float x, float y, float z)
        {
            vector.x = x;
            vector.y = y;
            vector.z = z;
        }
        public Position(Vector3 value) => vector = value;

        public static implicit operator Vector3(Position value) => value.vector;
        public static implicit operator Position(Vector3 value) => new(value);
        public static implicit operator Position(Direction value) => new(value);

        public static bool operator ==(Position left, Position right) => left.vector == right.vector;
        public static bool operator !=(Position left, Position right) => left.vector != right.vector;
        public static bool operator ==(Position left, Vector3 right) => left.vector == right;
        public static bool operator !=(Position left, Vector3 right) => left.vector != right;
        public static bool operator ==(Vector3 left, Position right) => left == right.vector;
        public static bool operator !=(Vector3 left, Position right) => left != right.vector;

        public static Position operator +(Position primary, Position value) => new(primary.vector + value.vector);
        public static Position operator -(Position primary, Position value) => new(primary.vector - value.vector);
        public static Position operator *(Position primary, float value) => new(primary.vector * value);
        public static Position operator /(Position primary, float value) => new(primary.vector / value);
        public static Position operator *(Position primary, Position value)
        {
            Vector3 result = primary.vector;
            result.x *= value.x; result.y *= value.y; result.z *= value.z;
            return new Position(result);
        }
        public static Position operator /(Position primary, Position value)
        {
            Vector3 result = primary.vector;
            result.x /= value.x; result.y /= value.y; result.z /= value.z;
            return new Position(result);
        }
        public static Position operator +(Position primary, Direction value) => new(primary.vector + value.vector);
        public static Position operator -(Position primary, Direction value) => new(primary.vector - value.vector);


        public override bool Equals(object obj)
        {
            return obj is Position position &&
                   vector.Equals(position.vector) &&
                   x == position.x &&
                   y == position.y &&
                   z == position.z;
        }
        public override int GetHashCode() => HashCode.Combine(vector, x, y, z);
    }

    /// <summary>
    /// A Psudeo-Vector 3 Struct to represent Euler Rotations around 3 axes.
    /// </summary>
    public struct Rotation
    {
        public Vector3 vector;
        public float x { get => vector.x; set => vector.x = value; }
        public float y { get => vector.y; set => vector.y = value; }
        public float z { get => vector.z; set => vector.z = value; }

        public Rotation(float x, float y, float z, Vector3Type type = Vector3Type.Rotation)
        {
            if (type == Vector3Type.Position) Debug.Log("Idk what you thought would happen feeding a Position into a Rotation. The Raw values have been used.");

            vector.x = x;
            vector.y = y;
            vector.z = z;

            if (type == Vector3Type.Direction) vector = vector.DirToRot();
        }
        public Rotation(Vector3 value, Vector3Type type = Vector3Type.Rotation)
        {
            if (type == Vector3Type.Position) Debug.Log("Idk what you thought would happen feeding a Position into a Rotation. The Raw values have been used.");

            vector = value;

            if (type == Vector3Type.Direction) vector = value.DirToRot();
        }

        public static implicit operator Vector3(Rotation value) => value.vector;
        public static implicit operator Rotation(Vector3 value) => new(value);

        public static explicit operator Rotation(Direction value) => new(((Vector3)value).DirToRot());

        public static bool operator ==(Rotation primary, Vector3 other) => primary.vector == other;
        public static bool operator !=(Rotation primary, Vector3 other) => primary.vector != other;
        public static bool operator ==(Rotation primary, Rotation other) => primary.vector == other.vector;
        public static bool operator !=(Rotation primary, Rotation other) => primary.vector != other.vector;
        public static bool operator ==(Rotation primary, Direction other) => primary.vector == other.vector.DirToRot();
        public static bool operator !=(Rotation primary, Direction other) => primary.vector != other.vector.DirToRot();

        public static Rotation operator +(Rotation primary, Rotation value) => new(primary.vector + value.vector);
        public static Rotation operator -(Rotation primary, Rotation value) => new(primary.vector - value.vector);

        public Direction ToDirection() => new(vector.RotToDir());

        public override bool Equals(object obj)
        {
            return obj is Rotation rotation &&
                   vector.Equals(rotation.vector) &&
                   x == rotation.x &&
                   y == rotation.y &&
                   z == rotation.z;
        }
        public override int GetHashCode() => HashCode.Combine(vector, x, y, z);
    }

    /// <summary>
    /// A Psudeo-Vector 3 Struct to represent Directional Rotations.
    /// </summary>
    public struct Direction
    {
        public Vector3 vector;
        public float x { get => vector.x; set => vector.x = value; }
        public float y { get => vector.y; set => vector.y = value; }
        public float z { get => vector.z; set => vector.z = value; }
        public Direction normalized => vector.normalized;
        public float magnitude => vector.magnitude;

        public Direction(float x, float y, float z, Vector3Type type = Vector3Type.Direction, bool normalize = false)
        {
            vector.x = x;
            vector.y = y;
            vector.z = z;

            if (type == Vector3Type.Rotation) vector = Quaternion.LookRotation(new Vector3(x, y, z)).eulerAngles;
            if (normalize) vector = vector.normalized;
        }
        public Direction(Vector3 value, Vector3Type type = Vector3Type.Direction, bool normalize = false)
        {

            vector = value;

            if (type == Vector3Type.Rotation) vector = Quaternion.LookRotation(value).eulerAngles;
            if (normalize) value = value.normalized;
        }


        public static implicit operator Vector3(Direction value) => value.vector;
        public static implicit operator Direction(Vector3 value) => new(value);
        public static implicit operator Direction(Position value) => new(value);

        public static explicit operator Direction(Rotation value) => new(((Vector3)value).RotToDir());

        public static bool operator ==(Direction primary, Vector3 other) => primary.vector == other;
        public static bool operator !=(Direction primary, Vector3 other) => primary.vector != other;
        public static bool operator ==(Direction primary, Direction other) => primary.vector == other.vector;
        public static bool operator !=(Direction primary, Direction other) => primary.vector != other.vector;
        public static bool operator ==(Direction primary, Rotation other) => primary.vector == other.vector.RotToDir();
        public static bool operator !=(Direction primary, Rotation other) => primary.vector != other.vector.RotToDir();

        public static Direction operator +(Direction primary, Direction value) => new(primary.vector + value.vector);
        public static Direction operator -(Direction primary, Direction value) => new(primary.vector - value.vector);
        public static Direction operator *(Direction primary, float value) => new(primary.vector * value);
        public static Direction operator /(Direction primary, float value) => new(primary.vector / value);
        public static Direction operator *(Direction primary, Direction value)
        {
            Vector3 result = primary.vector;
            result.x *= value.x; result.y *= value.y; result.z *= value.z;
            return new Direction(result);
        }
        public static Direction operator /(Direction primary, Direction value)
        {
            Vector3 result = primary.vector;
            result.x /= value.x; result.y /= value.y; result.z /= value.z;
            return new Direction(result);
        }

        public static Direction operator -(Direction value) => new(-value.vector);

        public Rotation ToRotation() => new(vector.RotToDir());

        public override bool Equals(object obj)
        {
            return obj is Direction direction &&
                   vector.Equals(direction.vector) &&
                   x == direction.x &&
                   y == direction.y &&
                   z == direction.z &&
                   normalized.Equals(direction.normalized);
        }
        public override int GetHashCode() => HashCode.Combine(vector, x, y, z, normalized);

        public Direction Rotate(float amount, Vector3 axis)
        {
            vector = Quaternion.AngleAxis(amount, axis) * vector;
            return this;
        }
        public Direction RotateTo(Direction towards, Direction reference)
        {
            vector = Quaternion.FromToRotation(reference, towards) * vector;
            return this;
        }
        public Direction Normalize()
        {
            vector.Normalize();
            return this;
        }


        public static Direction up = Vector3.up;
        public static Direction down = Vector3.down;
        public static Direction right = Vector3.right;
        public static Direction left = Vector3.left;
        public static Direction forward = Vector3.forward;
        public static Direction front = Vector3.forward;
        public static Direction back = Vector3.back;
        public static Direction one = Vector3.one;
        public static Direction zero = Vector3.zero;

        public static Direction XY = Vector3.one - Vector3.forward;
        public static Direction YZ = Vector3.one - Vector3.right;
        public static Direction XZ = Vector3.one - Vector3.up;

    }

    /// <summary>
    /// A Psudeo-Quaternion Struct to represent the Self-Orientation of a Transform. (Unfinished)
    /// </summary>
    public struct Orientation
    {
        public Quaternion quaternion;
        public Rotation eular => quaternion.eulerAngles;
        public float x => eular.x;
        public float y => eular.y;
        public float z => eular.z;

        public Direction front;
        public Direction up;
        public Direction right;
        public Direction back => -front;
        public Direction down => -up;
        public Direction left => -right;






    }


    public static class Vector3ExtensionMethods
    {
        public static Vector3 DirToRot(this Vector3 value) => Quaternion.LookRotation(value.normalized).eulerAngles;
        public static Vector3 RotToDir(this Vector3 value) => Quaternion.Euler(value) * Vector3.forward;

        public static Position Position(this Vector3 value) => new(value);
        public static Rotation Rotation(this Vector3 value) => new(value);
        public static Direction Direction(this Vector3 value) => new(value);

        public static Vector2 XYZToXZ(this Vector3 value) => new(value.x, value.z);
        public static Vector3 XZToXYZ(this Vector2 value) => new(value.x, 0, value.y);

        public static Vector3 Randomize(this Vector3 value, float min, float max)
        {
            value.x = UnityEngine.Random.Range(min, max);
            value.y = UnityEngine.Random.Range(min, max);
            value.z = UnityEngine.Random.Range(min, max);
            return value;
        }

    }



}