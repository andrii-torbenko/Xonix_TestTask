using UnityEngine;
using System.Collections.Generic;

public static class UIManager {

    private static Enumerators.UIState _activePage;
    
    private static List<Page> _pages = new List<Page>();

    private static Page _pageMainMenu,
                        _pageOptions,
                        _pageScoretab;

    private static Canvas _canvas;

    private static GameObject _background,
                              _authorText;

    public static Canvas Canvas
    {
        get { return _canvas; }
    }
    
    public static Enumerators.UIState ActivePage
    {
        get { return _activePage; }
        set
        {
            _activePage = value;
            ShowPage(value);
        }
    }

    public static void ShowPage(Enumerators.UIState page) {
        _background.SetActive(true);
        _authorText.SetActive(true);
        for (var i = 0; i < _pages.Count; i++) {
            _pages[i].Hide();
        }
        switch (page) {
            case Enumerators.UIState.PAGE_MAIN_MENU:
                _pageMainMenu.Show();
                break;
            case Enumerators.UIState.PAGE_OPTIONS:
                _pageOptions.Show();
                break;
            case Enumerators.UIState.PAGE_SCORETAB:
                _pageScoretab.Show();
                ((PageScoreTab)_pageScoretab).HidePopup(null);
                break;
        }
    }

    public static void HideAll() {
        for (var i = 0; i < _pages.Count; i++) {
            _pages[i].Hide();
        }
        _background.SetActive(false);
        _authorText.SetActive(false);
    }

    public static void UpdateScoretab() {
        ((PageScoreTab)_pageScoretab).UpdateTab();
    }

    public static void OpenScoretabWithInput() {
        ShowPage(Enumerators.UIState.PAGE_SCORETAB);
        AudioManager.PlaySoundType(Enumerators.SoundType.SCORETAB);
        ((PageScoreTab)_pageScoretab).ShowPopup();
    }

    public static void Init() {
        if (!StateManager.isAppStarted) {
            _canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

            _background = _canvas.transform.Find("Image_Background").gameObject;
            _authorText = _canvas.transform.Find("Text_Author").gameObject;
            _pageOptions = new PageOptions();
            _pageMainMenu = new PageMainMenu();
            _pageScoretab = new PageScoreTab();

            _pages.Add(_pageScoretab);
            _pages.Add(_pageMainMenu);
            _pages.Add(_pageOptions);

            for (var i = 0; i < _pages.Count; i++) {
                _pages[i].Load();
                _pages[i].Init();
            }
        }
        else {
            throw new System.NotImplementedException("Can't initialize ui manager more than once");
        }
    }
}
