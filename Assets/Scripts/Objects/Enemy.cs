public class Enemy {
    private IntVector2 _position;
    private Enumerators.EnemyType _type;
    private Enumerators.EnemyDirection _dir;

    public IntVector2 Position
    {
        get { return _position; }
    }

    public Enumerators.EnemyType Type{
        get { return _type; }
    }

    private void RandomDirButNotThis(Enumerators.EnemyDirection direction) {
        int randomDir = UnityEngine.Random.Range(0, 4);
        if (_dir == direction) {
            randomDir = ++randomDir % 4;
        }
        _dir = (Enumerators.EnemyDirection)randomDir;
    }

    private IntVector2 GetNextPosition() {
        switch (_dir) {
            case Enumerators.EnemyDirection.LD:
                return _position.GetLD();
            case Enumerators.EnemyDirection.RD:
                return _position.GetRD();
            case Enumerators.EnemyDirection.LU:
                return _position.GetLU();
            case Enumerators.EnemyDirection.RU:
                return _position.GetRU();
        }
        throw new System.NotSupportedException();
    }

    private void SwitchDirection(IntVector2 collisionPosition) {
        Enumerators.SquareType collisionType = GameManager.GetType(collisionPosition);
        switch (_dir) {
            case Enumerators.EnemyDirection.LD:
                if (GameManager.GetType(collisionPosition.GetU()) == collisionType &&
                    GameManager.GetType(collisionPosition.GetR()) == collisionType) {
                    _dir = Enumerators.EnemyDirection.RU;
                    return;
                }
                if (GameManager.GetType(collisionPosition.GetU()) == collisionType) {
                    _dir = Enumerators.EnemyDirection.RD;
                    return;
                }
                if (GameManager.GetType(collisionPosition.GetR()) == collisionType) {
                    _dir = Enumerators.EnemyDirection.LU;
                    return;
                }
                RandomDirButNotThis(Enumerators.EnemyDirection.LD);
                break;
            case Enumerators.EnemyDirection.RD:
                if (GameManager.GetType(collisionPosition.GetU()) == collisionType &&
                    GameManager.GetType(collisionPosition.GetL()) == collisionType) {
                    _dir = Enumerators.EnemyDirection.LU;
                    return;
                }
                if (GameManager.GetType(collisionPosition.GetU()) == collisionType) {
                    _dir = Enumerators.EnemyDirection.LD;
                    return;
                }
                if (GameManager.GetType(collisionPosition.GetL()) == collisionType) {
                    _dir = Enumerators.EnemyDirection.RU;
                    return;
                }
                RandomDirButNotThis(Enumerators.EnemyDirection.LU);
                break;
            case Enumerators.EnemyDirection.LU:
                if (GameManager.GetType(collisionPosition.GetD()) == collisionType &&
                    GameManager.GetType(collisionPosition.GetR()) == collisionType) {
                    _dir = Enumerators.EnemyDirection.RD;
                    return;
                }
                if (GameManager.GetType(collisionPosition.GetD()) == collisionType) {
                    _dir = Enumerators.EnemyDirection.RU;
                    return;
                }
                if (GameManager.GetType(collisionPosition.GetR()) == collisionType) {
                    _dir = Enumerators.EnemyDirection.LD;
                    return;
                }
                RandomDirButNotThis(Enumerators.EnemyDirection.RD);
                break;
            case Enumerators.EnemyDirection.RU:
                if (GameManager.GetType(collisionPosition.GetD()) == collisionType &&
                    GameManager.GetType(collisionPosition.GetL()) == collisionType) {
                    _dir = Enumerators.EnemyDirection.LD;
                    return;
                }
                if (GameManager.GetType(collisionPosition.GetD()) == collisionType) {
                    _dir = Enumerators.EnemyDirection.LU;
                    return;
                }
                if (GameManager.GetType(collisionPosition.GetL()) == collisionType) {
                    _dir = Enumerators.EnemyDirection.RD;
                    return;
                }
                RandomDirButNotThis(Enumerators.EnemyDirection.RU);
                break;
        }
    }

    private void SetRandomDirection() {
        _dir = (Enumerators.EnemyDirection)UnityEngine.Random.Range(0, 4);
    }

    private void SwitchDirectionOut() {
        switch (_dir) {
            case Enumerators.EnemyDirection.LD:
                int y_ld = GameManager.MaxY - 1;
                if (_position.x == 0 && _position.y == y_ld) {
                    _dir = Enumerators.EnemyDirection.RU;
                    return;
                }
                if (_position.x == 0) {
                    _dir = Enumerators.EnemyDirection.RD;
                    return;
                }
                if (_position.y == y_ld) {
                    _dir = Enumerators.EnemyDirection.LU;
                    return;
                }
                break;
            case Enumerators.EnemyDirection.RD:
                int x_rd = GameManager.MaxX - 1, y_rd = GameManager.MaxY - 1;
                if (_position.x == x_rd && _position.y == y_rd) {
                    _dir = Enumerators.EnemyDirection.LU;
                    return;
                }
                if (_position.x == x_rd) {
                    _dir = Enumerators.EnemyDirection.LD;
                    return;
                }
                if (_position.y == y_rd) {
                    _dir = Enumerators.EnemyDirection.RU;
                    return;
                }
                break;
            case Enumerators.EnemyDirection.LU:
                if (_position.x == 0 && _position.y == 0) {
                    _dir = Enumerators.EnemyDirection.RD;
                    return;
                }
                if (_position.x == 0) {
                    _dir = Enumerators.EnemyDirection.RU;
                    return;
                }
                if (_position.y == 0) {
                    _dir = Enumerators.EnemyDirection.LD;
                    return;
                }
                break;
            case Enumerators.EnemyDirection.RU:
                int x_ru = GameManager.MaxX - 1;
                if (_position.x == x_ru && _position.y == 0) {
                    _dir = Enumerators.EnemyDirection.LD;
                    return;
                }
                if (_position.x == x_ru) {
                    _dir = Enumerators.EnemyDirection.LU;
                    return;
                }
                if (_position.y == 0) {
                    _dir = Enumerators.EnemyDirection.RD;
                    return;
                }
                break;
        }
    }

    public void Move() {
        IntVector2 nextPos = GetNextPosition();
        if (GameManager.IsOutOfRange(nextPos)) {
            SwitchDirectionOut();
        }
        else {
            switch (_type) {
                case Enumerators.EnemyType.EWT:
                    if (GameManager.GetType(nextPos) == Enumerators.SquareType.GROUND) SwitchDirection(nextPos);
                    break;
                case Enumerators.EnemyType.EGR:
                    if (GameManager.GetType(nextPos) == Enumerators.SquareType.WATER || GameManager.GetType(nextPos) == Enumerators.SquareType.TRACK) SwitchDirection(nextPos);
                    break;
            }
        }

        _position = GetNextPosition();
        IntVector2 D = _position.GetD(), U = _position.GetU(), R = _position.GetR(), L = _position.GetL(), LD = _position.GetLD(), LU = _position.GetLU(), RD = _position.GetRD(), RU = _position.GetRD();
        if (_type == Enumerators.EnemyType.EGR) if (D == GameManager.playerPosition || U == GameManager.playerPosition || R == GameManager.playerPosition || L == GameManager.playerPosition || LD == GameManager.playerPosition || LU == GameManager.playerPosition || RD == GameManager.playerPosition || RU == GameManager.playerPosition) {
            GameManager.Die();
        }

        if ((GameManager.GetType(_position) == Enumerators.SquareType.TRACK && _type == Enumerators.EnemyType.EWT)|| _position == GameManager.playerPosition) {
            GameManager.Die();
        }
    }

    public Enemy(IntVector2 position, Enumerators.EnemyType type) {
        _position = position;
        _type = type;
        SetRandomDirection();
    }

    public Enemy(IntVector2 position, Enumerators.EnemyType type, Enumerators.EnemyDirection dir) {
        _position = position;
        _type = type;
        SetRandomDirection();
    }
}