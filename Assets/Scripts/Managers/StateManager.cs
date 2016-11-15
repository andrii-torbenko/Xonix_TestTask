public static class StateManager {
    private static Enumerators.AppState _appState;
    private static bool _isAppStarted = false;
    public static bool isAppStarted { get { return _isAppStarted; } }
    public static Enumerators.AppState AppState {
        get {
            return _appState;
        }
        set {
            switch(value) {
                case Enumerators.AppState.GAME:
                    StartGame();
                    break;
                case Enumerators.AppState.MENU:
                    OpenMenu();
                    break;
                case Enumerators.AppState.SCORETAB:
                    OpenScoretab();
                    break;
            }
            _appState = value;
        }
    }

    public static void Init() {
        if (!_isAppStarted) {
            DataManager.Init();
            AudioManager.Init();
            UIManager.Init();
            FieldManager.Init();
            _isAppStarted = true;
        }
        else {
            throw new System.NotImplementedException("Can't initialize state manager more than once");
        }
    }

    private static void StartGame() {
        AudioManager.Stop();
    }

    private static void OpenMenu() {
        AudioManager.Stop();
    }

    private static void OpenScoretab() {
        AudioManager.Stop();
        AudioManager.PlaySoundType(Enumerators.SoundType.SCORETAB);
    }
}
