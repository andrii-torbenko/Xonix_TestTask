using UnityEngine;

public abstract class Page {
    private GameObject prefab,
                       instance;

    private static string prefabsPath = "Prefabs/Pages/";

    protected string prefabName;

    public bool IsActive() {
        return instance.activeSelf;
    }

    public void Init() {
        prefab = Resources.Load<GameObject>(prefabsPath + prefabName);
        instance = Object.Instantiate(prefab);
        instance.transform.SetParent(UIManager.Canvas.transform, false);
    }
    public void Show() {
        instance.SetActive(true);
    }
    public void Hide() {
        instance.SetActive(false);
    }
}
