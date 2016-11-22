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
                    UIManager.HideAll();
                    AudioManager.Stop();
                    StartGame();
                    break;
                case Enumerators.AppState.MENU:
                    GameManager.HideField();
                    AudioManager.PlayMusic();
                    UIManager.ShowPage(Enumerators.UIState.PAGE_MAIN_MENU);
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
            GameManager.Init();
            _isAppStarted = true;
            GameManager.HideField();
            AppState = Enumerators.AppState.MENU;
        }
        else {
            throw new System.NotImplementedException("Can't initialize state manager more than once");
        }
    }

    private static void StartGame() {
        GameManager.ShowField();
        GameManager.ShowScreenButton();
    }

    public static void OnCloseApp() {
        DataManager.Save();
    }
}
