﻿using UnityEngine;
using System.Collections;

public class Enumerators {

    public enum AppState {
        MENU,
        GAME
    }

    public enum UIState {
        PAGE_MAIN_MENU,
        PAGE_OPTIONS,
        PAGE_SCORETAB,
    }

    public enum EnemyDirection {
        LU, // Left Up
        LD, // Left Down
        RU, // Right Up
        RD  // Right Down
    }

    public enum PlayerDirection {
        L,  // Left
        D,  // Down
        R,  // Right
        U   // Up
    }


    public enum StaticSquareType {
        GRD, // Ground
        WTR, // Water
        TRK, // Track
    }

    public enum EnemyType {
        EGR, // On-ground enemy
        EWT, // On-water enemy
    }

    public enum SoundType {
        WIN,
        LOSE,
        SCORETAB
    }
}
