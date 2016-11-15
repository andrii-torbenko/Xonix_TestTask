using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public static class FieldManager {

    public const int rows = 45;
    public const int cols = 80;

    private static GameObject _canvas,
                              _field,
                              _blockPrefab;
    private static GameObject[,] _sceneBlocks = new GameObject[rows, cols];


    public static Player player = new Player(Enumerators.PlayerDirection.D, new IntVector2(cols / 2, 0));

    private static StaticSquare[,] _staticBackground = new StaticSquare[rows, cols];
    private static List<StaticSquare> _tracks = new List<StaticSquare>();
    private static List<Enemy> _enemies = new List<Enemy>();
    private static List<StaticSquare> _tempSelectedSquares = new List<StaticSquare>();
    

    private static Color _groundColor = Color.cyan,
                         _trackColor = Color.magenta,
                         _waterColor = Color.black,
                         _enemyColorWater = Color.red,
                         _enemyColorGround = Color.yellow,
                         _playerColor = Color.blue;

    private static Sprite _playerImg,
                          _enemyGroundImg,
                          _enemyWaterImg;

    private static float _nextMoveTime = 0;
    private static float _moveDeltaTime = .08f;

    private static float _swipeMinDelta = 1.5f;

    private static bool _isMoving = true;

    private static Enumerators.PlayerDirection _direction;

    public static void Init() {
        if (!StateManager.isAppStarted) {
            _canvas = GameObject.Find("Canvas");
            _field = _canvas.transform.Find("Field").gameObject;
            _blockPrefab = Resources.Load<GameObject>("Prefabs/Image");
            _playerImg = Resources.Load<Sprite>("Images/Image_MainChar");
            _enemyGroundImg = Resources.Load<Sprite>("Images/Image_Enemy_Ground");
            _enemyWaterImg = Resources.Load<Sprite>("Images/Image_Enemy_Water");
            _direction = Enumerators.PlayerDirection.R;
        }
        else {
            throw new System.NotImplementedException("Can't initialize field manager more than once");
        }
    }

    public static Enumerators.StaticSquareType GetSquareType(IntVector2 position) {
        return _staticBackground[position.y, position.x].StaticType;
    }

    public static void AddEnemy() {
        _enemies.Add(new Enemy(Enumerators.EnemyType.EWT, new IntVector2(cols / 2, rows / 2)));
    }

    public static void GenerateField() {
        if (_sceneBlocks[0, 0] == null) {
            for (var i_c = 0; i_c < cols; i_c++) {
                for (var i_r = 0; i_r < rows; i_r++) {
                    GameObject currentBlock = GameObject.Instantiate(_blockPrefab);
                    currentBlock.transform.SetParent(_field.transform, false);
                    _sceneBlocks[i_r, i_c] = currentBlock;

                    if ((i_c < 2 || i_c > cols - 3) || (i_r < 2 || i_r > rows - 3)) {
                        _staticBackground[i_r, i_c] = new StaticSquare(Enumerators.StaticSquareType.GRD, new IntVector2(i_c, i_r));
                    }
                    else {
                        _staticBackground[i_r, i_c] = new StaticSquare(Enumerators.StaticSquareType.WTR, new IntVector2(i_c, i_r));
                    }
                }
            }
        }
        else {
            for (var i_c = 0; i_c < cols; i_c++) {
                for (var i_r = 0; i_r < rows; i_r++) {
                    if ((i_c < 2 || i_c > cols - 3) || (i_r < 2 || i_r > rows - 3)) {
                        _staticBackground[i_r, i_c] = new StaticSquare(Enumerators.StaticSquareType.GRD, new IntVector2(i_c, i_r));
                    }
                    else {
                        _staticBackground[i_r, i_c] = new StaticSquare(Enumerators.StaticSquareType.GRD, new IntVector2(i_c, i_r));
                    }
                }
            }
        }
    }

    public static void DeleteEnemy() {
        if (_enemies.Count > 0) _enemies.RemoveAt(0);
    }

    public static void ClearAll() {
        for (int i = 0; i < _enemies.Count; i++) {
            _enemies.RemoveAt(i);
        }
        for (int i = 0; i < _tracks.Count; i++) {
            _tracks.RemoveAt(i);
        }
    }

    public static bool CanEnemyMoveThere(IntVector2 position, Enumerators.EnemyType enemyType) {
        if (NotOutOfRange(position)) {
            if (GetSquareType(position) == Enumerators.StaticSquareType.WTR && enemyType == Enumerators.EnemyType.EWT) {
                return true;
            }
            else if (GetSquareType(position) == Enumerators.StaticSquareType.GRD && enemyType == Enumerators.EnemyType.EGR) {
                return true;
            }
        }
        return false;
    }

    public static void Restart() {
        GenerateField();
        ClearAll();
        AddEnemy();
        player.ResetPlayer();
        UpdateField();
    }

    public static StaticSquare GetBackGroundSquare(IntVector2 position) {
        return _staticBackground[position.y, position.x];
    }

    public static StaticSquare GetBackGroundSquareXY(int x, int y) {
        return _staticBackground[y, x];
    }

    private static bool NotOutOfRange(IntVector2 position) {
        if (position.x >= 0 && position.x < cols && position.y >= 0 && position.y < rows) return true;
        return false;
    }

    private static bool IsWater(IntVector2 position) {
        if (_staticBackground[position.y, position.x].StaticType == Enumerators.StaticSquareType.WTR) return true;
        return false;
    }
    
    private static Enemy EnemyHere(IntVector2 position) {
        for (int i = 0; i < _enemies.Count; i++) {
            if (_enemies[i].Position == position) return _enemies[i];
        }
        return null;
    }

    private static bool IsTrackHere(IntVector2 position) {
        for (int i = 0; i < _tracks.Count; i++) {
            if (_tracks[i].Position == position) return true;
         }
        return false;
    }

    private static Image GetBackgroundImage(IntVector2 position) {
        return _sceneBlocks[position.y, position.x].GetComponent<Image>();
    }

    public static void UpdateBlock(IntVector2 position) {
        Image square = GetBackgroundImage(position);
        Enemy enemy = EnemyHere(position);
        if (player.Position == position) {
            square.color = Color.white;
            square.sprite = _playerImg;
        }
        else if (enemy != null) {
            square.color = Color.white;
            switch (enemy.EnemyType) {
                case Enumerators.EnemyType.EGR:
                    square.sprite = _enemyGroundImg;
                    break;
                case Enumerators.EnemyType.EWT:
                    square.sprite = _enemyWaterImg;
                    break;
            }
        }
        else {
            square.sprite = null;
            switch (GetSquareType(position)) {
                case Enumerators.StaticSquareType.GRD:
                    square.color = _groundColor;
                    break;
                case Enumerators.StaticSquareType.WTR:
                    square.color = _waterColor;
                    break;
                case Enumerators.StaticSquareType.TRK:
                    square.color = _trackColor;
                    break;
            }
        }
    }

    public static void UpdateField() {
        for (var i_c = 0; i_c < cols; i_c++) {
            for (var i_r = 0; i_r < rows; i_r++) {
                Image sceneBlockImg = _sceneBlocks[i_r, i_c].GetComponent<Image>();
                sceneBlockImg.sprite = null;
                switch (_staticBackground[i_r, i_c].StaticType) {
                    case Enumerators.StaticSquareType.GRD:
                        sceneBlockImg.color = _groundColor;
                        break;
                    case Enumerators.StaticSquareType.WTR:
                        sceneBlockImg.color = _waterColor;
                        break;
                }
            }
        }


        foreach (StaticSquare track in _tracks) {
           GetBackgroundImage(track.Position).color = _trackColor;
        }


        foreach (Enemy enemy in _enemies) {
            switch (enemy.EnemyType) {
                case Enumerators.EnemyType.EGR:
                    _sceneBlocks[enemy.Position.y, enemy.Position.x].GetComponent<Image>().color = Color.white;
                    _sceneBlocks[enemy.Position.y, enemy.Position.x].GetComponent<Image>().sprite = _enemyGroundImg;
                    break;
                case Enumerators.EnemyType.EWT:
                    _sceneBlocks[enemy.Position.y, enemy.Position.x].GetComponent<Image>().color = Color.white;
                    _sceneBlocks[enemy.Position.y, enemy.Position.x].GetComponent<Image>().sprite = _enemyWaterImg;
                    break;
            }
        }


        GetBackgroundImage(player.Position).color = Color.white;
        GetBackgroundImage(player.Position).sprite = _playerImg;

    }

    public static void AddTrack(IntVector2 position) {
        _tracks.Add(_staticBackground[position.y, position.x]);
        _staticBackground[position.y, position.x].StaticType = Enumerators.StaticSquareType.TRK;
    }

    public static bool IsRangeOk(int x, int y) {
        if (x >= 0 && x < cols && y >= 0 && y < rows) return true;
        return false;
    }

    public static void UpdatePositions() {
        if (Time.time > _nextMoveTime) {
            for (int i = 0; i < ((Time.time - _nextMoveTime) / _moveDeltaTime); i++) {
                foreach (Enemy enemy in _enemies) {
                    enemy.Move();
                }
                player.Move();
            }
            _nextMoveTime = Time.time + _moveDeltaTime;
        }
    }

    private static void GetSurroundSquares(IntVector2 position, List<StaticSquare> originalList, List<StaticSquare> checkingList, List<StaticSquare> surroundSquares) {
        int nextPos_x = position.x + 1,
            nextPos_y = position.y;
        StaticSquare square = GetBackGroundSquareXY(nextPos_x, nextPos_y);
        if (!originalList.Contains(square) && !surroundSquares.Contains(square) && square.StaticType == Enumerators.StaticSquareType.WTR) {
            surroundSquares.Add(square);
        }

        nextPos_x = position.x - 1;
        nextPos_y = position.y;
        square = GetBackGroundSquareXY(nextPos_x, nextPos_y);
        if (!originalList.Contains(square) && !surroundSquares.Contains(square) && square.StaticType == Enumerators.StaticSquareType.WTR) {
            surroundSquares.Add(square);
        }

        nextPos_x = position.x;
        nextPos_y = position.y - 1;
        square = GetBackGroundSquareXY(nextPos_x, nextPos_y);
        if (!originalList.Contains(square) && !surroundSquares.Contains(square) && square.StaticType == Enumerators.StaticSquareType.WTR) {
            surroundSquares.Add(square);
        }

        nextPos_x = position.x;
        nextPos_y = position.y + 1;
        square = GetBackGroundSquareXY(nextPos_x, nextPos_y);
        if (!originalList.Contains(square) && !surroundSquares.Contains(square) && square.StaticType == Enumerators.StaticSquareType.WTR) {
            surroundSquares.Add(square);
        }
        //nextPos_x = position.x - 1;
        //nextPos_y = position.y - 1;
        //square = GetBackGroundSquareXY(nextPos_x, nextPos_y);
        //if (square != null && !originalList.Contains(square) && !checkingList.Contains(square) && !surroundSquares.Contains(square) && square.StaticType == Enumerators.StaticSquareType.WTR) {
        //    surroundSquares.Add(square);
        //}

        //nextPos_x = position.x + 1;
        //nextPos_y = position.y + 1;
        //square = GetBackGroundSquareXY(nextPos_x, nextPos_y);
        //if (square != null && !originalList.Contains(square) && !checkingList.Contains(square) && !surroundSquares.Contains(square) && square.StaticType == Enumerators.StaticSquareType.WTR) {
        //    surroundSquares.Add(square);
        //}

        //nextPos_x = position.x - 1;
        //nextPos_y = position.y + 1;
        //square = GetBackGroundSquareXY(nextPos_x, nextPos_y);
        //if (square != null && !originalList.Contains(square) && !checkingList.Contains(square) && !surroundSquares.Contains(square) && square.StaticType == Enumerators.StaticSquareType.WTR) {
        //    surroundSquares.Add(square);
        //}

        //nextPos_x = position.x + 1;
        //nextPos_y = position.y - 1;
        //square = GetBackGroundSquareXY(nextPos_x, nextPos_y);
        //if (square != null && !originalList.Contains(square) && !checkingList.Contains(square) && !surroundSquares.Contains(square) && square.StaticType == Enumerators.StaticSquareType.WTR) {
        //    surroundSquares.Add(square);
        //}
    }

    private static List<StaticSquare> GetEnemyField(Enemy enemy) {
        List<StaticSquare> returnedList = new List<StaticSquare>();
        List<StaticSquare> checkingList = new List<StaticSquare>();
        List<StaticSquare> surroundSquares = new List<StaticSquare>();

        checkingList.Add(GetBackGroundSquare(enemy.Position));
        returnedList.Add(GetBackGroundSquare(enemy.Position));
        int i = 0;
        while (true) {
            for (int i_c = 0; i_c < checkingList.Count; i_c++) {
                GetSurroundSquares(checkingList[i_c].Position, returnedList, checkingList, surroundSquares);
            }
            if (checkingList.Count < 1) {
                break;
            }
            returnedList.AddRange(checkingList);
            checkingList.Clear();
            checkingList.AddRange(surroundSquares);
            surroundSquares.Clear();
            i++;
            if (i > 120) break;
        }
        return returnedList;
    }

    public static void SeizeFieldsWithNoEnemies() {

        foreach (StaticSquare track in _tracks) {
            track.StaticType = Enumerators.StaticSquareType.GRD;
        }
        _tracks.Clear();

        List<StaticSquare> enemyFields = new List<StaticSquare>();
        foreach (Enemy enemy in _enemies) {
            foreach (StaticSquare square in GetEnemyField(enemy)) {
                enemyFields.Add(square);
                //GetBackgroundImage(square.Position).color = Color.red;
            }
        }

        for (int x = 0; x < cols; x++) {
            for (int y = 0; y < rows; y++) {
                if (!enemyFields.Contains(_staticBackground[y, x])) {
                    _staticBackground[y, x].StaticType = Enumerators.StaticSquareType.GRD;
                }
            }
        }
        //foreach (StaticSquare square in enemyFields) {
        //    square.StaticType = Enumerators.StaticSquareType.WTR;
        //}
        //UpdateField();
    }
    }