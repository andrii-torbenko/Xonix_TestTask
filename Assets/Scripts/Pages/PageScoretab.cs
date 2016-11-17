using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PageScoreTab : Page {

    private Button _buttonSound,
                   _buttonBack;
    private GameObject _playerPanelPrefab,
                       _playerPanelHandler;
    public override void Init() {
        _buttonBack = instance.transform.Find("Button_Back").GetComponent<Button>();
        _buttonBack.onClick.AddListener(OnBack);

        _playerPanelHandler = instance.transform.Find("ScrollRect_Scores/Grid_Players").gameObject;
        _playerPanelPrefab = _playerPanelHandler.transform.Find("Panel_Player").gameObject;

        for (int i = 0; i < DataManager.DBCount; i++) {
            GameObject new_instance = GameObject.Instantiate(_playerPanelPrefab);
            new_instance.transform.SetParent(_playerPanelHandler.transform, false);
            new_instance.transform.Find("Text_Position").GetComponent<Text>().text = (i + 1).ToString();
            new_instance.transform.Find("Text_Name").GetComponent<Text>().text = DataManager.GetNameByIndex(i);
            new_instance.transform.Find("Text_Score").GetComponent<Text>().text = DataManager.GetScoreByIndex(i).ToString();
            new_instance.transform.Find("Text_Level").GetComponent<Text>().text = DataManager.GetLevelByIndex(i).ToString();

        }

        GameObject.Destroy(_playerPanelPrefab);
    }

    public void OnBack() {
        UIManager.ActivePage = Enumerators.UIState.PAGE_MAIN_MENU;
    }

    public PageScoreTab() {
        prefabName = "Page_Scoretab";
    }
}