using UnityEngine;
using System.Collections.Generic;

public static class UIManager {

    private static Enumerators.UIState _activePage;
    
    private static List<Page> _pages;

    private static Page _pageCredits,
                        _pageMainMenu,
                        _pageOptions,
                        _pageScoretab;

    private static Canvas _canvas;

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
        for (var i = 0; i < _pages.Count; i++) {
            _pages[i].Hide();
        }
        switch (page) {
            case Enumerators.UIState.PAGE_CREDITS:
                _pageCredits.Show();
                break;
            case Enumerators.UIState.PAGE_MAIN_MENU:
                _pageMainMenu.Show();
                break;
            case Enumerators.UIState.PAGE_OPTIONS:
                _pageOptions.Show();
                break;
            case Enumerators.UIState.PAGE_SCORETAB:
                _pageScoretab.Show();
                break;
        }
    }

    public static void Init() {
        if (!StateManager.isAppStarted) {
            _canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

            _pages.Add(_pageCredits);
            _pages.Add(_pageMainMenu);
            _pages.Add(_pageOptions);
            _pages.Add(_pageScoretab);

            for (var i = 0; i < _pages.Count; i++) {
                _pages[i].Init();
            }
        }
        else {
            throw new System.NotImplementedException("Can't initialize ui manager more than once");
        }
    }
}
