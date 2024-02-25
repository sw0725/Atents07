using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestData : IComparable<TestData>
{
    int x;
    float y;
    string z;

    public TestData(int x, float y, string z) 
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public int CompareTo(TestData other)
    {
        //if (other == null)                    // == 오버로딩과의 충돌
        //    return 1;

        return other.z.CompareTo(this.z);       //내림차순
    }

    public static bool operator ==(TestData left, TestData right)
    {
        return (left.x == right.x);
    }

    public static bool operator ==(TestData left, int right)
    {
        return (left.x == right);
    }

    public static bool operator !=(TestData left, TestData right)
    {
        return (left.x != right.x);
    }

    public static bool operator !=(TestData left, int right)
    {
        return (left.x != right);
    }

    public override bool Equals(object obj)
    {
        return obj is TestData other && this.x == other.x;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(x,y,z);
    }
}
