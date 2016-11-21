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

    public void MoveD() {
        _y += 1;
    }

    public void MoveU() {
        _y -= 1;
    }
    
    public void MoveR() {
        _x += 1;
    }
    
    public void MoveL() {
        _x -= 1;
    }
    
    public void MoveRD() {
        _x += 1;
        _y += y;
    }
    
    public void MoveLD() {
        _x -= 1;
        _y += y;
    }

    public void MoveRU() {
        _x += 1;
        _y -= y;
    }
    
    public void MoveLU() {
        _x -= 1;
        _y -= y;
    }

    public IntVector2 GetD() {
        return new IntVector2(_x, _y + 1);
    }

    public IntVector2 GetU() {
        return new IntVector2(_x, _y - 1);
    }

    public IntVector2 GetR() {
        return new IntVector2(_x + 1, _y);
    }

    public IntVector2 GetL() {
        return new IntVector2(_x - 1, _y);
    }

    public IntVector2 GetRD() {
        return new IntVector2(_x + 1, _y + 1);
    }

    public IntVector2 GetLD() {
        return new IntVector2(_x - 1, _y + 1);
    }

    public IntVector2 GetRU() {
        return new IntVector2(_x + 1, _y - 1);
    }

    public IntVector2 GetLU() {
        return new IntVector2(_x - 1, _y - 1);
    }
}
