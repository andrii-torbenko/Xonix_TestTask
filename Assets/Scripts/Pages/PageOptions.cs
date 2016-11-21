using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PageOptions : Page {

    private Button _buttonSound,
                   _buttonBack;
    private string _onSoundText = "Off Sound",
                   _offSoundText = "On Sound";

    public override void Init() {
        _buttonSound = instance.transform.Find("Grid_Buttons/Button_Sound").GetComponent<Button>();
        _buttonBack = instance.transform.Find("Button_Back").GetComponent<Button>();

        _buttonSound.onClick.AddListener(OnSound);
        _buttonBack.onClick.AddListener(OnBack);
    }

    public void OnSound() {
        if (AudioManager.Mute()) {
            _buttonSound.GetComponent<Text>().text = _offSoundText;
        }
        else {
            _buttonSound.GetComponent<Text>().text = _onSoundText;
        }
    }

    public void OnBack() {
        UIManager.ActivePage = Enumerators.UIState.PAGE_MAIN_MENU;
    }

    public void OnButtonExit() {
        Application.Quit();
    }

    public PageOptions() {
        prefabName = "Page_Options";
    }
}