using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public static class FieldManager {
    private const int _blocksX = 80, _blocksY = 45;

    public enum SquareType { GROUND, WATER, TRACK };
    public enum PlayerDirection { L, R, U, D };

    private static Image[,] _sceneImgs   = new Image[_blocksX, _blocksY];
    private static SquareType[,] _typePixels = new SquareType[_blocksX, _blocksY];

    private static GameObject _field,
                       _blockPrefab;

    private static Sprite _playerImg,
                   _enemyGroundImg,
                   _enemyWaterImg;

    private static Color _groundColor = Color.cyan,
                         _trackColor = Color.magenta,
                         _waterColor = Color.black,
                         _enemyColorWater = Color.red,
                         _enemyColorGround = Color.yellow,
                         _playerColor = Color.blue;

    public static void Init() {
        _field = UIManager.Canvas.transform.Find("Field").gameObject;
        _blockPrefab = Resources.Load<GameObject>("Prefabs/Image");
        _playerImg = Resources.Load<Sprite>("Images/Image_MainChar");
        _enemyGroundImg = Resources.Load<Sprite>("Images/Image_Enemy_Ground");
        _enemyWaterImg = Resources.Load<Sprite>("Images/Image_Enemy_Water");

        for (int x = 0; x < _blocksX; x++) {
            for (int y = 0; y < _blocksY; y++) {
                GameObject new_instance = GameObject.Instantiate(_blockPrefab);
                _sceneImgs[x, y] = new_instance.GetComponent<Image>();
                new_instance.transform.SetParent(_field.transform, false);
            }
        }
    }

    public static void HideField() {
        _field.SetActive(false);
    }

    public static void ShowField() {
        _field.SetActive(true);
    }

    /////////////////////////////////////////////////////////

    public static SquareType GetType(int x, int y) {
        return _typePixels[x, y];
    }

    public static Image GetImage(int x, int y) {
        return _sceneImgs[x, y];
    }

    public static void ResetField() {
        for (int x = 0; x < _blocksX; x++) {
            for (int y = 0; y < _blocksY; y++) {
                if (x < 2 || x >= _blocksX - 2 || y < 2 || y >= _blocksY - 2) {
                    _sceneImgs[x, y].color = _groundColor;
                    _typePixels[x, y] = SquareType.GROUND;
                }
                else {
                    _sceneImgs[x, y].color = _waterColor;
                    _typePixels[x, y] = SquareType.WATER;
                }
            }
        }
    }
}