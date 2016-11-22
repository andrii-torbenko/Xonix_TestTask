using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public static class GameManager {

    private static int _lives = 2,
                        _score = 0,
                        _level = 1,
                        _defEnemiesGroundCount = 1,
                        _defEenemiesWaterCount = 1,
                        _enemiesGroundCount = 1,
                        _enemiesWaterCount = 1,
                        _completed = 0,
                        _notCountedSurroundBlocks = 0,
                        _borderComplToWin = 75,
                        _rangeBetweenOnSpawn = 2,
                        _scoreMultiplier = 2;
    private static float _enemiesGroundAddRate = 0.4f,
                         _enemiesWaterAddRate = 0.5f,
                         _timeAlive = 0,
                         _maxTimeAlive = 60,
                         _nextPlayTime = 0,
                         _updateDelay = 0.07f;

    private const int _blocksX = 64,
                      _blocksY = 36;
    
    private static bool _isPaused = false,
                        _wasPaused = false;

    private static string _textPaused = "Paused",
                          _textStartGame = "Tap to play";


    public static int MaxX { get { return _blocksX; } }
    public static int MaxY { get { return _blocksY; } }
    public static bool IsPaused
    {
        get { return _isPaused; }
        set {
            if (!value) _nextPlayTime = Time.time;
            _buttonScreen.gameObject.SetActive(value);
            _textMainEvent.gameObject.SetActive(value);
            _textMainEvent.text = _textPaused;
            _isPaused = value;
        }
    }
    public static float TimeAlive
    {
        get { return _timeAlive; }
        private set
        {
            _timeAlive = value;
            _textTimeAlive.text = "Time: " + ((int)_timeAlive).ToString();
        }
    }

    public static int Lives
    {
        get { return _lives; }
        private set
        {
            _lives = value;
            _textLives.text = "Lives: " + _lives.ToString();
        }
    }
    public static int Score
    {
        get { return _score; }
        private set
        {
            _score = value;
            _textScore.text = "Score: " + _score.ToString();
        }
    }
    public static int Level
    {
        get { return _level; }
        private set
        {
            _level = value;
            _textLevel.text = "Level: " + _level.ToString();
        }
    }
    public static int Completed
    {
        get { return _completed; }
        private set
        {
            _completed = value;
            _textCompleted.text = "Compl: " + _completed.ToString();
        }
    }
    public static IntVector2 playerPosition
    {
        get { return _player.Position; }
    }


    private static Player _player = new Player(new IntVector2(_blocksX / 2, 0));

    private static Image[,] _sceneImgs   = new Image[_blocksX, _blocksY];
    private static Enumerators.SquareType[,] _typePixels = new Enumerators.SquareType[_blocksX, _blocksY];

    private static List<IntVector2> _trackList = new List<IntVector2>();

    private static Button _buttonPause,
                          _buttonBack,
                          _buttonScreen;

    private static Text _textLevel,
                        _textScore,
                        _textLives,
                        _textCompleted,
                        _textMainEvent,
                        _textTimeAlive;

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
        _field = UIManager.Canvas.transform.Find("Main_Game/Field").gameObject;
        _blockPrefab = Resources.Load<GameObject>("Prefabs/Image");
        _playerImg = Resources.Load<Sprite>("Images/Image_MainChar");
        _enemyGroundImg = Resources.Load<Sprite>("Images/Image_Enemy_Ground");
        _enemyWaterImg = Resources.Load<Sprite>("Images/Image_Enemy_Water");

        var mainPage = _field.transform.parent;

        _buttonPause = mainPage.Find("Button_Pause").GetComponent<Button>();
        _buttonBack = mainPage.Find("Button_Back").GetComponent<Button>();
        _buttonScreen = mainPage.Find("Button_Screen").GetComponent<Button>();
        _textMainEvent = mainPage.Find("Text_MainGameEvent").GetComponent<Text>();
        var gridText = _field.transform.parent.Find("Grid_Text");
        
        _textLevel = gridText.transform.Find("Text_Level").GetComponent<Text>();
        _textLives = gridText.transform.Find("Text_Lives").GetComponent<Text>();
        _textScore = gridText.transform.Find("Text_Score").GetComponent<Text>();
        _textCompleted = gridText.transform.Find("Text_Completed").GetComponent<Text>();
        _textTimeAlive = gridText.transform.Find("Text_TimeAlive").GetComponent<Text>();

        _buttonPause.onClick.AddListener(PauseGame);
        _buttonBack.onClick.AddListener(OpenMenu);
        _buttonScreen.onClick.AddListener(StartGame);

        for (int x = 0; x < _blocksX; x++) {
            for (int y = 0; y < _blocksY; y++) {
                GameObject new_instance = Object.Instantiate(_blockPrefab);
                _sceneImgs[x, y] = new_instance.GetComponent<Image>();
                new_instance.transform.SetParent(_field.transform, false);
            }
        }
    }


    #region buttonActions
    public static void StartGame() {
        if (_buttonScreen.interactable) {
            //ResetScore();
            _buttonScreen.interactable = false;
            IsPaused = false;
            ResetField();
            _buttonScreen.gameObject.SetActive(false);
            _textMainEvent.gameObject.SetActive(false);
        }
    }

    public static void PauseGame() {
        if (_isPaused) {
            IsPaused = false;
        }
        else {
            IsPaused = true;
        }
    }

    public static void OpenMenu() {
        QuitGame();
        IsPaused = false;
        StateManager.AppState = Enumerators.AppState.MENU;
        UIManager.ActivePage = Enumerators.UIState.PAGE_MAIN_MENU;
    }
    #endregion

    #region base
    public static void HideField() {
        _field.transform.parent.gameObject.SetActive(false);
    }

    public static void ShowField() {
        _field.transform.parent.gameObject.SetActive(true);
        ResetScore();
    }
    #endregion

    #region instanceCreating

    public static void AddWaterEnemy(int posx) {
        _enemies.Add(new Enemy(new IntVector2(posx, _blocksY / 2), Enumerators.EnemyType.EWT));
    }
    
    public static void AddGroundEnemy(int posx) {
        _enemies.Add(new Enemy(new IntVector2(posx, _blocksY - 1), Enumerators.EnemyType.EGR));
    }

    public static void LeaveTrack(IntVector2 position) {
        _typePixels[position.x, position.y] = Enumerators.SquareType.TRACK;
        _trackList.Add(position);
    }

    #endregion

    #region getSquare

    public static Enumerators.SquareType GetType(IntVector2 position) {
        return _typePixels[position.x, position.y];
    }

    public static Enumerators.SquareType GetType(int x, int y) {
        return _typePixels[x, y];
    }

    public static Image GetImage(int x, int y) {
        return _sceneImgs[x, y];
    }

    public static Image GetImage(IntVector2 position) {
        return _sceneImgs[position.x, position.y];
    }

    #endregion

    #region playerControlling
    public static Enumerators.PlayerDirection GetPlayerDirection() {
        return _player.Direction;
    }

    public static void ResetPlayer() {
        _player.Position = new IntVector2(_blocksX / 2, 0);
        _player.IsMoving = false;
        _player.IsLeavingTrack = false;
    }

    public static bool IsPlayerMooving
    {
        get { return _player.IsMoving; }
        set { _player.IsMoving = value; }
    }

    public static bool IsPlayerLeavingTrack {
        get { return _player.IsLeavingTrack; }
    }

    public static void SetPlayerDirection(Enumerators.PlayerDirection dir) {
        _player.Direction = dir;
    }

    #endregion

    public static void ShowScreenButton() {
        ResetField();
        IsPaused = true;
        _buttonScreen.interactable = true;
        _buttonScreen.gameObject.SetActive(true);
        _textMainEvent.gameObject.SetActive(true);
        _textMainEvent.text = _textStartGame;
    }

    public static bool IsOutOfRange(IntVector2 position) {
        return position.x >= _blocksX || position.x < 0 || position.y >= _blocksY || position.y < 0;
    }

    public static void ResetField() {
        _notCountedSurroundBlocks = 0;
        for (int x = 0; x < _blocksX; x++) {
            for (int y = 0; y < _blocksY; y++) {
                if (x < 2 || x >= _blocksX - 2 || y < 2 || y >= _blocksY - 2) {
                    _notCountedSurroundBlocks++;
                    _sceneImgs[x, y].color = _groundColor;
                    _typePixels[x, y] = Enumerators.SquareType.GROUND;
                }
                else {
                    _sceneImgs[x, y].color = _waterColor;
                    _typePixels[x, y] = Enumerators.SquareType.WATER;
                }
            }
        }
        RestartLevel();
    }

    private static void DeleteGroundEnemies() {
        List<Enemy> enemiesToRemove = new List<Enemy>();
        for (int i = 0; i < _enemies.Count; i++) {
            if (_enemies[i].Type == Enumerators.EnemyType.EGR) enemiesToRemove.Add(_enemies[i]);
        } 
        for (int i = 0; i < enemiesToRemove.Count; i++) {
            _enemies.Remove(enemiesToRemove[i]);
        }
    }

    private static void DeleteWaterEnemies() {
        List<Enemy> enemiesToRemove = new List<Enemy>();
        for (int i = 0; i < _enemies.Count; i++) {
            if (_enemies[i].Type == Enumerators.EnemyType.EWT) enemiesToRemove.Add(_enemies[i]);
        }
        for (int i = 0; i < enemiesToRemove.Count; i++) {
            _enemies.Remove(enemiesToRemove[i]);
        }
    }

    private static bool AreWaterEnemiesOnField() {
        for (int i = 0; i < _enemies.Count; i++) {
            if (_enemies[i].Type == Enumerators.EnemyType.EWT) return true;
        }
        return false;
    }

    private static int GetWaterEnemiesCount() {
        int count = 0;
        for (int i = 0; i < _enemies.Count; i++) {
            if (_enemies[count].Type == Enumerators.EnemyType.EWT) count++;
        }
        return count;
    }

    private static void RestartLevel() {
        RemoveTracks();
        ResetPlayer();
        DeleteGroundEnemies();
        int groundHalfRange = (_blocksX / 2) - (_rangeBetweenOnSpawn * _enemiesGroundCount) / 2;
        for (int i = 0; i < _enemiesGroundCount; i++) {
            AddGroundEnemy(groundHalfRange + i * _rangeBetweenOnSpawn);
        }
        if (!AreWaterEnemiesOnField()) {
            int waterHalfRange = (_blocksX / 2) - (_rangeBetweenOnSpawn * _enemiesWaterCount) / 2;
            for (int i = 0; i < _enemiesWaterCount; i++) {
                AddWaterEnemy(waterHalfRange + i * _rangeBetweenOnSpawn);
            }
        }
        if (GetWaterEnemiesCount() < _enemiesWaterCount) {
            DeleteWaterEnemies();
            int waterHalfRange = (_blocksX / 2) - (_rangeBetweenOnSpawn * _enemiesWaterCount) / 2;
            for (int i = 0; i < _enemiesWaterCount; i++) {
                AddWaterEnemy(waterHalfRange + i * _rangeBetweenOnSpawn);
            }
        }
    }

    private static void RemoveTracks() {
        for (int i = 0; i < _trackList.Count; i++) {
            _typePixels[_trackList[i].x, _trackList[i].y] = Enumerators.SquareType.WATER;
        }
        _trackList.RemoveRange(0, _trackList.Count);
        
    }

    private static void ResetScore() {
        Lives = 2;
        Score = 0;
        Level = 1;
        _enemiesGroundCount = _defEnemiesGroundCount;
        _enemiesWaterCount = _defEenemiesWaterCount;
        Completed = 0;
        TimeAlive = 0;
    }

    private static void GoToNextLevel() {
        _isPaused = true;
        _buttonPause.interactable = false;
        AudioManager.PlaySoundType(Enumerators.SoundType.WIN);
        TimeManager.AddTimer(StartNextLevel, null, false, Time.time, 0, Time.time + 2);
    }

    private static void StartNextLevel(object[] args) {
        _isPaused = false;
        _buttonPause.interactable = true;
        Completed = 0;
        TimeAlive = 0;
        Lives++;
        Level++;
        _enemiesGroundCount = 1 + (int)(Level * _enemiesGroundAddRate);
        _enemiesWaterCount = 1 + (int)(Level * _enemiesWaterAddRate);
        ResetField();
        RestartLevel();
    }

    public static void QuitGame() {
        IsPaused = true;
        ResetPlayer();
        DeleteEnemies();
        IsPaused = true;
        StateManager.AppState = Enumerators.AppState.MENU;
        UIManager.OpenScoretabWithInput();
    }
    
    public static void DeleteEnemies() {
        _enemies.RemoveRange(0, _enemies.Count);
    }

    public static void UpdateVisualField() {
        for (int x = 0; x < _blocksX; x++) {
            for (int y = 0; y < _blocksY; y++) {
                GetImage(x, y).sprite = null;
                switch (GetType(x, y)) {
                    case Enumerators.SquareType.GROUND:
                        GetImage(x, y).color = _groundColor;
                        break;
                    case Enumerators.SquareType.WATER:
                        GetImage(x, y).color = _waterColor;
                        break;
                    case Enumerators.SquareType.TRACK:
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
            if (_timeAlive > _maxTimeAlive) {
                Die();
                TimeAlive = 0;
            }
            _player.Move();
            for (int i = 0; i < _enemies.Count; i++) {
                _enemies[i].Move();
            }
            UpdateVisualField();
            TimeAlive += _updateDelay;
            _nextPlayTime += _updateDelay;
        }
    }

    public static void Die() {
        AudioManager.PlaySoundType(Enumerators.SoundType.LOSE);
        _isPaused = true;
        _buttonPause.interactable = false;
        TimeManager.AddTimer(RestartAfterDeath, null, false, Time.time, 0, Time.time + 1.3f);
    }

    private static void RestartAfterDeath(object[] args) {
        _isPaused = false;
        _buttonPause.interactable = true;
        RestartLevel();
        Lives--;
        if (_lives < 0) {
            QuitGame();
        }
    }

    public static void FillSquare(IntVector2 position) {
        _typePixels[position.x, position.y] = Enumerators.SquareType.TRACK;
        IntVector2 U = position.GetU();
            if (GetType(U) == Enumerators.SquareType.WATER) FillSquare(U);
        IntVector2 D = position.GetD();
            if (GetType(D) == Enumerators.SquareType.WATER) FillSquare(D);
        IntVector2 R = position.GetR();
            if (GetType(R) == Enumerators.SquareType.WATER) FillSquare(R);
        IntVector2 L = position.GetL();
            if (GetType(L) == Enumerators.SquareType.WATER) FillSquare(L);
    }

    public static void SeizeField() {
        foreach (Enemy enemy in _enemies) {
            if (enemy.Type == Enumerators.EnemyType.EWT) FillSquare(enemy.Position);
        }

        int count_gr = 0;
        int count_seized = 0;
        for (int i = 0; i < _trackList.Count; i++, count_gr++, count_seized++) {
            _typePixels[_trackList[i].x, _trackList[i].y] = Enumerators.SquareType.GROUND;
        }

        _trackList.RemoveRange(0, _trackList.Count);

        for (int x = 0; x < _blocksX; x++) for (int y = 0; y < _blocksY; y++) {
                switch (GetType(x, y)) {
                    case Enumerators.SquareType.WATER:
                        _typePixels[x, y] = Enumerators.SquareType.GROUND;
                        count_gr++;
                        count_seized++;
                        break;
                    case Enumerators.SquareType.TRACK:
                        _typePixels[x, y] = Enumerators.SquareType.WATER;
                        break;
                    case Enumerators.SquareType.GROUND:
                        count_gr++;
                        break;
                }
            }
        Completed = (100 * (count_gr - _notCountedSurroundBlocks)) / (_blocksX * _blocksY - _notCountedSurroundBlocks);
        Score += count_seized * _scoreMultiplier;
        if (Completed >= _borderComplToWin) {
            _isPaused = true;
            _buttonPause.interactable = false;
            GoToNextLevel();
        }
    }
}