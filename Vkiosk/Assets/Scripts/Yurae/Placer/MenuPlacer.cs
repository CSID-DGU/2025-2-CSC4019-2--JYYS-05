// Unity
using UnityEngine;

/// <summary>
/// 메뉴 윈도우에 들어가는 정보 클래스
/// </summary>
[System.Serializable]
public class MenuWindowInfo
{
    public string menuId;
    public string name;
    public int price;
    [TextArea] public string description;
    public Sprite sprite;
}

public class MenuPlacer : WindowPlacer
{
    [SerializeField] private MenuWindowInfo[] windows;      // 윈도우 정보 클래스 배열
    [SerializeField] private GameObject windowPrefab;       // 윈도우 프리팹

    [Header("카테고리 ID")]
    [SerializeField] private string categoryID;

    /// <summary>
    /// 윈도우 배치 메소드 오버라이딩
    /// </summary>
    public void Place()
    {
        PlaceWindow(windows.Length, windowPrefab);
    }

    /// <summary>
    /// 윈도우 초기화 메소드 오버라이딩 (메뉴 이름, Description)
    /// </summary>
    /// <param name="_window"></param>
    /// <param name="_index"></param>
    public override void Init(GameObject _window, int _index)
    {
        MenuWindow menuWindow = _window.GetComponent<MenuWindow>();

        if (menuWindow != null)
        {
            menuWindow.UpdateInfoToMenuWindow(windows[_index].name, windows[_index].price, windows[_index].description, windows[_index].sprite);
            menuWindow.SetWindow(windows[_index].menuId, windows[_index].name, windows[_index].price, windows[_index].sprite);
        }
    }

    public string GetCategoryID()
    {
        return categoryID;
    }
}
