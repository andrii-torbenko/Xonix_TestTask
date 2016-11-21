using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public static class GameManager {

    private static int _lives = 2;
    private static bool _isPaused = false;

    public static bool IsPaused
    {
        get { return _isPaused; }
        set {
            _isPaused = value;
        }
    }

    private static float _nextPlayTime = 0,
                         _updateDelay = 0.08f;

    private const int _blocksX = 64, _blocksY = 36;

    public static int MaxX { get { return _blocksX; } }
    public static int MaxY { get { return _blocksY; } }


    public enum SquareType { GROUND, WATER, TRACK };
    public enum PlayerDirection { L, R, U, D };

    private static Player _player = new Player(new IntVector2(_blocksX / 2, 0));
    private static Image[,] _sceneImgs   = new Image[_blocksX, _blocksY];
    private static SquareType[,] _typePixels = new SquareType[_blocksX, _blocksY];
    private static List<IntVector2> _trackList = new List<IntVector2>();

    private static GameObject _field,
                       _blockPrefab;

    private static Sprite _playerImg,
                   _enemyGroundImg,
                   _enemyWaterImg;

    private static Color _purpleDarkColor = new Color(0.11f, 0, 0.19f),
                         _purpleLightColor = new Color(0.4f, 0.01f, 0.98f),
                         _groundColor = _purpleLightColor,
                         _trackColor = Color.white,
                         _waterColor = Color.black,
                         _enemyColorWater = _purpleLightColor,
                         _enemyColorGround = _purpleLightColor,
                         _playerColor = _purpleLightColor;

    private static List<Enemy> _enemies = new List<Enemy>();

    public static void Init() {
        _field = UIManager.Canvas.transform.Find("Page_Game/Field").gameObject;
        _blockPrefab = Resources.Load<GameObject>("Prefabs/Image");
        _playerImg = Resources.Load<Sprite>("Images/Image_MainChar");
        _enemyGroundImg = Resources.Load<Sprite>("Images/Image_Enemy_Ground");
        _enemyWaterImg = Resources.Load<Sprite>("Images/Image_Enemy_Water");

        for (int x = 0; x < _blocksX; x++) {
            for (int y = 0; y < _blocksY; y++) {
                GameObject new_instance = GameObject.Instantiate(_blockPrefab);
                _sceneImgs[x, y] = new_instance.GetComponent<Image>();
                new_instance.transform.SetParent(_field.transform, false);
            }
        }
    }

    public static void HideField() {
        _field.SetActive(false);
    }

    public static void ShowField() {
        _field.SetActive(true);
        //TimeManager.AddTimer(ShowMsg, null, false, Time.time + 1, 1, Time.time + 5);
    }

    public static bool IsOutOfRange(IntVector2 position) {
        return position.x >= _blocksX || position.x < 0 || position.y >= _blocksY || position.y < 0;
    }

    public static void AddWaterEnemy() {
        _enemies.Add(new Enemy(new IntVector2(_blocksX / 2, _blocksY / 2), Enumerators.EnemyType.EWT));
    }
    
    public static void AddGroundEnemy() {
        _enemies.Add(new Enemy(new IntVector2(_blocksX / 2, _blocksY - 1), Enumerators.EnemyType.EGR));
    }

    public static SquareType GetType(IntVector2 position) {
        return _typePixels[position.x, position.y];
    }

    public static SquareType GetType(int x, int y) {
        return _typePixels[x, y];
    }

    public static void LeaveTrack(IntVector2 position) {
        _typePixels[position.x, position.y] = SquareType.TRACK;
        _trackList.Add(position);
    }

    public static Image GetImage(int x, int y) {
        return _sceneImgs[x, y];
    }

    public static Image GetImage(IntVector2 position) {
        return _sceneImgs[position.x, position.y];
    }

    public static Enumerators.PlayerDirection GetPlayerDirection() {
        return _player.Direction;
    }

    public static void ResetPlayer() {
        _player.Position = new IntVector2(_blocksX / 2, 0);
        _player.IsMoving = false;
    }

    public static void StopPlayer() {
        _player.IsMoving = false;
    }

    public static void StartPlayer() {
        _player.IsMoving = true;
    }

    public static bool IsPlayerMooving() {
        return _player.IsMoving;
    }

    public static bool IsPlayerOnWater() {
        return _player.IsLeavingTrack;
    }

    public static void SetPlayerDirection(Enumerators.PlayerDirection dir) {
        _player.Direction = dir;
    }

    public static void ResetField() {
        for (int x = 0; x < _blocksX; x++) {
            for (int y = 0; y < _blocksY; y++) {
                if (x < 2 || x >= _blocksX - 2 || y < 2 || y >= _blocksY - 2) {
                    _sceneImgs[x, y].color = _groundColor;
                    _typePixels[x, y] = SquareType.GROUND;
                }
                else {
                    _sceneImgs[x, y].color = _waterColor;
                    _typePixels[x, y] = SquareType.WATER;
                }
            }
        }
        AddWaterEnemy();
        AddGroundEnemy();
    }

    public static void DeleteEnemies() {
        _enemies.RemoveRange(0, _enemies.Count);
    }

    public static void UpdateVisualField() {
        for (int x = 0; x < _blocksX; x++) {
            for (int y = 0; y < _blocksY; y++) {
                GetImage(x, y).sprite = null;
                switch (GetType(x, y)) {
                    case SquareType.GROUND:
                        GetImage(x, y).color = _groundColor;
                        break;
                    case SquareType.WATER:
                        GetImage(x, y).color = _waterColor;
                        break;
                    case SquareType.TRACK:
                        GetImage(x, y).color = _trackColor;
                        break;
                }
            }
        }
        for (int i = 0; i < _enemies.Count; i++) {
            switch (_enemies[i].Type) {
                case Enumerators.EnemyType.EGR:
                    GetImage(_enemies[i].Position).sprite = _enemyGroundImg;
                    GetImage(_enemies[i].Position).color = Color.white;
                    break;
                case Enumerators.EnemyType.EWT:
                    GetImage(_enemies[i].Position).sprite = _enemyWaterImg;
                    GetImage(_enemies[i].Position).color = Color.white;
                    break;
            }
        }
        GetImage(_player.Position).sprite = _playerImg;
        GetImage(_player.Position).color = Color.white;
    }

    public static void Update() {
        while (_nextPlayTime < Time.time) {
            for (int i = 0; i < _enemies.Count; i++) {
                _enemies[i].Move();
            }
            _player.Move();
            UpdateVisualField();
            _nextPlayTime += _updateDelay;
        }
    }

    public static void Die() {
        //GameManager.HideField();
        //StateManager.AppState = Enumerators.AppState.MENU;
        //if (DataManager.isRecord) UIManager.OpenScoretabWithInput();
    }

    public static void FillSquare(IntVector2 position) {
        _typePixels[position.x, position.y] = SquareType.TRACK;
        IntVector2 U = position.GetU();
            if (GetType(U) == SquareType.WATER) FillSquare(U);
        IntVector2 D = position.GetD();
            if (GetType(D) == SquareType.WATER) FillSquare(D);
        IntVector2 R = position.GetR();
            if (GetType(R) == SquareType.WATER) FillSquare(R);
        IntVector2 L = position.GetL();
            if (GetType(L) == SquareType.WATER) FillSquare(L);
    }

    public static void SeizeField() {
        foreach (Enemy enemy in _enemies) {
            if (enemy.Type == Enumerators.EnemyType.EWT) FillSquare(enemy.Position);
        }

        for (int i = 0; i < _trackList.Count; i++) {
            _typePixels[_trackList[i].x, _trackList[i].y] = SquareType.GROUND;
        }

        for (int x = 0; x < _blocksX; x++) for (int y = 0; y < _blocksY; y++) {
                switch (GetType(x, y)) {
                    case SquareType.WATER:
                        _typePixels[x, y] = SquareType.GROUND;
                        break;
                    case SquareType.TRACK:
                        _typePixels[x, y] = SquareType.WATER;
                        break;
                }
            }
    }
}