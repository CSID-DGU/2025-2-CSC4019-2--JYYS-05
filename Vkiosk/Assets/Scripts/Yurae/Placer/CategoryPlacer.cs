// Unity
using UnityEngine;

[System.Serializable]
public class CategoryWindowInfo
{
    public string id;
    public string name;
    public int price;
    public Sprite sprite;
}

public class CategoryPlacer : WindowPlacer
{
    [SerializeField] private CategoryWindowInfo[] windows;      // 윈도우 정보 클래스 배열
    [SerializeField] private GameObject windowPrefab;           // 윈도우 프리팹

    /// <summary>
    /// 윈도우 배치 메소드 오버라이딩
    /// </summary>
    public void Place()
    {
        PlaceWindow(windows.Length, windowPrefab);
    }

    /// <summary>
    /// 윈도우 초기화 메소드 오버라이딩 (카테고리 이름)
    /// </summary>
    /// <param name="_window"></param>
    /// <param name="_index"></param>
    public override void Init(GameObject _window, int _index)
    {
        CategoryWindow categoryWindow = _window.GetComponent<CategoryWindow>();

        if (categoryWindow != null)
        {
            categoryWindow.UpdateInfoToCategoryWindow(windows[_index].name, windows[_index].sprite);
            categoryWindow.SetWindow(windows[_index].id, windows[_index].name, windows[_index].price, windows[_index].sprite);
        }
    }
}
