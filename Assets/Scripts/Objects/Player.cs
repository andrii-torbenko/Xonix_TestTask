using UnityEngine;

public class Player {
    private IntVector2 _position;
    private bool _isMoving = false,
                 _isLeavingTrack = false;
    private Enumerators.PlayerDirection _dir = Enumerators.PlayerDirection.R;

    public Enumerators.PlayerDirection Direction
    {
        get { return _dir; }
        set { _dir = value; }
    }

    public bool IsLeavingTrack { get { return _isLeavingTrack; } }

    public bool IsMoving {
        get { return _isMoving; }
        set { _isMoving = value; }
    }

    public IntVector2 Position
    {
        get { return _position; }
        set { _position = value; }
    }

    public void StartMoving() {
        _isMoving = true;
    }

    private IntVector2 GetNextPosition() {
        IntVector2 nextPos = new IntVector2(_position.x, _position.y);
        switch (_dir) {
            case Enumerators.PlayerDirection.U:
                nextPos.MoveU();
                break;
            case Enumerators.PlayerDirection.D:
                nextPos.MoveD();
                break;
            case Enumerators.PlayerDirection.L:
                nextPos.MoveL();
                break;
            case Enumerators.PlayerDirection.R:
                nextPos.MoveR();
                break;
        }
        return nextPos;
    }

    public void Move() {
        IntVector2 nextPos = GetNextPosition();
        if (GameManager.IsOutOfRange(nextPos)) {
            _isMoving = false;
            return;
        }
        else {
            //if (!_isLeavingTrack && FieldManager.GetType(nextPos) == FieldManager.SquareType.WATER) {
            //    _isLeavingTrack = true;
            //}
            //if (_isLeavingTrack && FieldManager.GetType(nextPos) == FieldManager.SquareType.GROUND) {
            //    _isLeavingTrack = false;
            //    _isMoving = false;
            //}
            if (_isMoving) _position = nextPos;

            if (_isLeavingTrack) {
                if (GameManager.GetType(nextPos) == GameManager.SquareType.GROUND) {
                    _isLeavingTrack = false;
                    _isMoving = false;
                    GameManager.SeizeField();
                }
            }
            else {
                if (GameManager.GetType(nextPos) == GameManager.SquareType.WATER) {
                    _isLeavingTrack = true;
                }
            }


            if (_isLeavingTrack) {
                GameManager.LeaveTrack(_position);
            }
        }
    }

    public Player(IntVector2 position) {
        _position = position;
    }

}