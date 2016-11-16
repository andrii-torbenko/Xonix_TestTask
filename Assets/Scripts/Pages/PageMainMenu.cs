using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PageMainMenu : Page {

    private Button _buttonNewGame,
                   _buttonOptions,
                   _buttonScoreTab,
                   _buttonExit; 

    public override void Init() {
        _buttonNewGame = instance.transform.Find("Grid_Buttons/Button_New_game").GetComponent<Button>();
        _buttonOptions = instance.transform.Find("Grid_Buttons/Button_Options").GetComponent<Button>();
        _buttonScoreTab = instance.transform.Find("Grid_Buttons/Button_Scoretab").GetComponent<Button>();
        _buttonExit = instance.transform.Find("Grid_Buttons/Button_Exit").GetComponent<Button>();

        _buttonNewGame.onClick.AddListener(OnNewGame);
        _buttonOptions.onClick.AddListener(OnOptions);
        _buttonScoreTab.onClick.AddListener(OnScoretab);
        _buttonExit.onClick.AddListener(OnButtonExit);
    }

    public void OnNewGame() {
        StateManager.AppState = Enumerators.AppState.GAME;
    }

    public void OnOptions() {
        UIManager.ActivePage = Enumerators.UIState.PAGE_OPTIONS;
    }

    public void OnScoretab() {
        UIManager.ActivePage = Enumerators.UIState.PAGE_SCORETAB;
    }

    public void OnButtonExit() {
        Application.Quit();
    }

    public PageMainMenu() {
        prefabName = "Page_MainMenu";
    }
}