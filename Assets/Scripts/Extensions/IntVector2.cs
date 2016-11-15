using UnityEngine;
using System.Collections;

public struct IntVector2 {
    private int _x,
                _y;

    public IntVector2(int x, int y) {
        _x = x;
        _y = y;
    }

    public int x
    {
        get { return _x; }
    }

    public int y
    {
        get { return _y; }
    }

    public static IntVector2 operator +(IntVector2 left, IntVector2 right) {
        left._x += right._x;
        left._y += right._y;
        return left;
    }

    public static IntVector2 operator -(IntVector2 left, IntVector2 right) {
        left._x -= right._x;
        left._y -= right._y;
        return left;
    }

    public static bool operator ==(IntVector2 left, IntVector2 right) {
        return (left.x == right.x && left.y == right.y);
    }

    public static bool operator !=(IntVector2 left, IntVector2 right) {
        return (left.x != right.x && left.y != right.y);
    }
}
