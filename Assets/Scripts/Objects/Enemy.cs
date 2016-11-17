//using UnityEngine;

//public class Enemy : DynamicSquare {

//    private Enumerators.EnemyDirection _enemyDirection;
//    private Enumerators.EnemyType _enemyType;
//    private static IntVector2 LD = new IntVector2(-1, 1),
//                              LU = new IntVector2(-1, -1),
//                              RU = new IntVector2(1, -1),
//                              RD = new IntVector2(1, 1);

//    public Enumerators.EnemyType EnemyType
//    {
//        get { return _enemyType; }
//    }

//    public IntVector2 GetNextPosition() {
        
//        switch (_enemyDirection) {
//            case Enumerators.EnemyDirection.LD: return position + LD;
//            case Enumerators.EnemyDirection.LU: return position + LU;
//            case Enumerators.EnemyDirection.RU: return position + RU;
//            case Enumerators.EnemyDirection.RD: return position + RD;
//        }

//        throw new System.InvalidOperationException();
//    }

//    public override void Move() {

//        IntVector2 nextPos = GetNextPosition();
        
//        if (!FieldManager.CanEnemyMoveThere(nextPos, _enemyType)) {
//            switch (_enemyDirection) {
//                case Enumerators.EnemyDirection.LD: _enemyDirection = Enumerators.EnemyDirection.RD; break;
//                case Enumerators.EnemyDirection.LU: _enemyDirection = Enumerators.EnemyDirection.RU; break;
//                case Enumerators.EnemyDirection.RU: _enemyDirection = Enumerators.EnemyDirection.LU; break;
//                case Enumerators.EnemyDirection.RD: _enemyDirection = Enumerators.EnemyDirection.LD; break;
//            }

//            nextPos = GetNextPosition();

//            if (!FieldManager.CanEnemyMoveThere(nextPos, _enemyType)) {
//                switch (_enemyDirection) {
//                    case Enumerators.EnemyDirection.LD: _enemyDirection = Enumerators.EnemyDirection.RU; break;
//                    case Enumerators.EnemyDirection.LU: _enemyDirection = Enumerators.EnemyDirection.RD; break;
//                    case Enumerators.EnemyDirection.RU: _enemyDirection = Enumerators.EnemyDirection.LD; break;
//                    case Enumerators.EnemyDirection.RD: _enemyDirection = Enumerators.EnemyDirection.LU; break;
//                }
//            }

//            nextPos = GetNextPosition();

//            if (!FieldManager.CanEnemyMoveThere(nextPos, _enemyType)) {
//                switch (_enemyDirection) {
//                    case Enumerators.EnemyDirection.LD: _enemyDirection = Enumerators.EnemyDirection.RD; break;
//                    case Enumerators.EnemyDirection.LU: _enemyDirection = Enumerators.EnemyDirection.RU; break;
//                    case Enumerators.EnemyDirection.RU: _enemyDirection = Enumerators.EnemyDirection.LU; break;
//                    case Enumerators.EnemyDirection.RD: _enemyDirection = Enumerators.EnemyDirection.LD; break;
//                }
//            }
//        }
//        IntVector2 temp = position;
//        position = nextPos;
//        FieldManager.UpdateBlock(temp);
//        FieldManager.UpdateBlock(position);
//    }

//    public Enemy(Enumerators.EnemyDirection enemyDirection, Enumerators.EnemyType enemyType, IntVector2 position) : base(position) {
//        _enemyDirection = enemyDirection;
//        _enemyType = enemyType;
//    }

//    public Enemy(Enumerators.EnemyType enemyType, IntVector2 position) : base(position) {
//        SetRandomDirection();
//        _enemyType = enemyType;
//    }

//    private void SetRandomDirection() {
//        _enemyDirection = (Enumerators.EnemyDirection)UnityEngine.Random.Range(0, (int)Enumerators.EnemyDirection.RD + 1);
//    }
//}
