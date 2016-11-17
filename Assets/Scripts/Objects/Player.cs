//using UnityEngine;

//public class Player : DynamicSquare {

//    private bool _isMoving = false;

//    private bool _isLeavingTrack = false;

//    public bool IsMoving
//    {
//        get { return _isMoving; }
//    }

//    private Enumerators.PlayerDirection _playerDirection;

//    public Enumerators.PlayerDirection PlayerDirection
//    {
//        get { return _playerDirection; }
//        set
//        {
//            _isMoving = true;
//            _playerDirection = value;
//        }
//    }

//    public void ResetPlayer() {
//        position = new IntVector2(FieldManager.cols / 2, 0);
//        _isLeavingTrack = false;
//        _isMoving = false;
//    }

//    public override void Move() {

//        if (!_isMoving) return;

//        IntVector2 nextPos = position;
//        switch (_playerDirection) {
//            case Enumerators.PlayerDirection.L: nextPos += new IntVector2(-1, 0); break;
//            case Enumerators.PlayerDirection.R: nextPos += new IntVector2(1, 0); break;
//            case Enumerators.PlayerDirection.U: nextPos += new IntVector2(0, -1); break;
//            case Enumerators.PlayerDirection.D: nextPos += new IntVector2(0, 1); break;
//        }

//        if (nextPos.x >= 0 && nextPos.y >= 0 && nextPos.x < FieldManager.cols && nextPos.y < FieldManager.rows) {
//            IntVector2 temp = position;
//            position = nextPos;
            
//            Enumerators.StaticSquareType nextSquareType = FieldManager.GetSquareType(position);

//            switch (nextSquareType) {
//                case Enumerators.StaticSquareType.WTR:
//                    FieldManager.AddTrack(position);
//                    _isLeavingTrack = true;
//                    break;
//                case Enumerators.StaticSquareType.GRD:
//                    if (_isLeavingTrack) {
//                        _isMoving = false;
//                        _isLeavingTrack = false;
//                        FieldManager.SeizeFieldsWithNoEnemies();
//                        Debug.Log("Need to seize ground.");
//                    }
//                    break;
//                case Enumerators.StaticSquareType.TRK:
//                    if (_isLeavingTrack) {
//                        _isMoving = false;
//                        FieldManager.Restart();
//                        AudioManager.PlaySoundType(Enumerators.SoundType.LOSE);
//                    }
//                    break;
//            }
//        }
//        else {
//            _isMoving = false;
//        }
//    }

//    public Player(Enumerators.PlayerDirection playerDirection, IntVector2 position) : base(position) {
//        _playerDirection = playerDirection;
//    }
//}


