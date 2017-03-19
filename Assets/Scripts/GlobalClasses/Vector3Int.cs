using UnityEngine;
using System;


[System.Serializable]
public class Vector3Int : IEquatable<Vector3Int>
{
    public byte x, y, z;

    public Vector3Int(byte _x, byte _y, byte _z)
    {
        x = _x;
        y = _y;
        z = _z;
    }
    public Vector3Int(int _x, int _y, int _z)
    {
        x = (byte)_x;
        y = (byte)_y;
        z = (byte)_z;
    }
    public Vector3Int(float _x, float _y, float _z)
    {
        x = (byte)_x;
        y = (byte)_y;
        z = (byte)_z;
    }
    public Vector3Int(Vector3 _vec3)
    {
        x = (byte)_vec3.x;
        y = (byte)_vec3.y;
        z = (byte)_vec3.z;
    }



    public static Vector3Int operator+ (Vector3Int _vec1, Vector3Int _vec2)
    {
        return new Vector3Int(_vec1.x + _vec2.x, _vec1.y + _vec2.y, _vec1.z + _vec2.z);
    }

    public static bool operator== (Vector3Int _vec1, Vector3Int _vec2)
    {
        return (_vec1.x == _vec2.x && _vec1.y == _vec2.y && _vec1.z == _vec2.z);
    }
    public static bool operator !=(Vector3Int _vec1, Vector3Int _vec2)
    {
        return (_vec1.x != _vec2.x || _vec1.y != _vec2.y || _vec1.z != _vec2.z);
    }






    public override int GetHashCode()
    {
        return this.x.GetHashCode() ^ this.y.GetHashCode() ^ this.z.GetHashCode();
    }
    
    
    public bool Equals(Vector3Int other)
    {
        return (this.x.Equals(other.x) && this.y.Equals(other.y) && this.z.Equals(other.z));
    }
    
}

