using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PageScoreTab : Page {

    private Button _buttonSound,
                   _buttonBack,
                   _buttonSave;

    private GameObject _playerPanelPrefab,
                       _playerPanelHandler,
                       _popupInputField;

    private Text _inputField;
    public override void Init() {

        _buttonBack = instance.transform.Find("Button_Back").GetComponent<Button>();
        _playerPanelHandler = instance.transform.Find("ScrollRect_Scores/Grid_Players").gameObject;
        _playerPanelPrefab = _playerPanelHandler.transform.Find("Panel_Player").gameObject;

        _popupInputField = instance.transform.Find("Popup_InputFieldBackground").gameObject;
        _inputField = _popupInputField.transform.Find("InputField/Input_Name").GetComponent<Text>();
        _buttonSave = _popupInputField.transform.Find("InputField/Button_Save").GetComponent<Button>();

        _buttonBack.onClick.AddListener(OnBack);
        _buttonSave.onClick.AddListener(OnSave);
        _inputField.transform.parent.GetComponent<InputField>().onEndEdit.AddListener(delegate { OnSave(); });

        UpdateTab();

        HidePopup(null);
    }

    public void UpdateTab() {
        List<GameObject> destroyingList = new List<GameObject>();

        for (int i = 1; i < _playerPanelHandler.transform.childCount; i++) {
            destroyingList.Add(_playerPanelHandler.transform.GetChild(i).gameObject);
        }

        foreach (GameObject go in destroyingList) {
            Object.Destroy(go);
        }

        _playerPanelPrefab = _playerPanelHandler.transform.GetChild(0).gameObject;
        _playerPanelPrefab.SetActive(true);
        for (int i = 0; i < DataManager.DBCount; i++) {
            GameObject new_instance = Object.Instantiate(_playerPanelPrefab);
            new_instance.transform.SetParent(_playerPanelHandler.transform, false);
            new_instance.transform.Find("Text_Position").GetComponent<Text>().text = (i + 1).ToString() + ".";
            new_instance.transform.Find("Text_Name").GetComponent<Text>().text = DataManager.GetNameByIndex(i);
            new_instance.transform.Find("Text_Score").GetComponent<Text>().text = DataManager.GetScoreByIndex(i).ToString();
            new_instance.transform.Find("Text_Level").GetComponent<Text>().text = DataManager.GetLevelByIndex(i).ToString();

        }

        if (DataManager.DBCount > 0) Object.Destroy(_playerPanelPrefab);
        else {
            _playerPanelPrefab.transform.Find("Text_Position").GetComponent<Text>().text = (1).ToString() + ".";
            _playerPanelPrefab.transform.Find("Text_Name").GetComponent<Text>().text = "Can be you";
            _playerPanelPrefab.transform.Find("Text_Score").GetComponent<Text>().text = "VERY HIGH";
            _playerPanelPrefab.transform.Find("Text_Level").GetComponent<Text>().text = "INF";
        }
    }

    public void ShowPopup() {
        _popupInputField.SetActive(true);
        _buttonSave.interactable = true;
        _inputField.transform.parent.GetComponent<Animator>().Play("InputFieldDrop");
    }

    public void HidePopup(object[] args) {
        _popupInputField.SetActive(false);
    }

    public void OnSave() {
        if (_inputField.text.Length > 0) {
            if (_buttonSave.interactable) {
                _buttonSave.interactable = false;
                DataManager.AddStat((uint)GameManager.Score, _inputField.text, (uint)GameManager.Level);
                _inputField.transform.parent.GetComponent<Animator>().Play("InputFieldClose");
                TimeManager.AddTimer(HidePopup, null, false, Time.time + 0, 0, Time.time + 1.3f);
                AudioManager.PlayMusic();
            }
        }
    }

    public void OnBack() {
        UIManager.ActivePage = Enumerators.UIState.PAGE_MAIN_MENU;
    }

    public PageScoreTab() {
        prefabName = "Page_Scoretab";
    }
}