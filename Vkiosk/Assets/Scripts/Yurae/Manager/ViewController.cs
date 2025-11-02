// Unity
using UnityEngine;
using TMPro;

public class ViewController : MonoBehaviour
{
    [SerializeField] private CategoryPlacer categoryPlacer;
    [SerializeField] private WindowManager categoryWindow;

    [SerializeField] private MenuPlacer[] menuPlacer;
    [SerializeField] private TextMeshProUGUI headerTMP;

    [SerializeField] private SessionManager sessionManager;

    [Header("Category Guide")]
    [SerializeField] private TextMeshProUGUI categoryGuide;

    [Header("Menu Guide")]
    [SerializeField] private TextMeshProUGUI menuGuide;
    private GameObject currentMenuSphere;
    

    private void Start()
    {
        Place();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)) CategoryToMenu();
        if (Input.GetKeyDown(KeyCode.V)) MenuToCategory();
    }

    public void Init()
    {
        currentMenuSphere = null;
    }

    public void Place()
    {
        categoryPlacer.Place();
        categoryPlacer.transform.rotation = Quaternion.Euler(0, 0, 0);

        foreach (MenuPlacer placer in menuPlacer)
        {
            placer.Place();
            placer.gameObject.SetActive(false);
        }

        categoryPlacer.gameObject.SetActive(false);
    }

    public void CategoryToMenu()
    {
        int index = FindMatchedCategoryIDIndex();

        // 현재 메뉴 스페어가 활성화 되어 있거나, 없는 ID의 메뉴일 경우 Skip
        if (currentMenuSphere != null || index == -1) return;

        categoryPlacer.gameObject.SetActive(false);
        menuPlacer[index].gameObject.SetActive(true);  // 현재 카테고리 ID에 해당하는 메뉴 오브젝트 활성화

        currentMenuSphere = menuPlacer[index].gameObject;

        headerTMP.text = "MENU";

        sessionManager.StartSession();
        categoryGuide.gameObject.SetActive(false);
        menuGuide.gameObject.SetActive(true);
    }

    public void MenuToCategory()
    {
        if (currentMenuSphere == null) return;

        currentMenuSphere.SetActive(false);

        currentMenuSphere = null;

        categoryPlacer.gameObject.SetActive(true);

        headerTMP.text = "CATEGORY";

        sessionManager.StartSession();

        categoryGuide.gameObject.SetActive(true);
        menuGuide.gameObject.SetActive(false);
    }

    private string GetCurrentCategoryID()
    {
        return categoryWindow.GetCurrentWindow().GetWindowID();
    }

    private int FindMatchedCategoryIDIndex()
    {
        for (int i = 0; i < menuPlacer.Length; i++)
        {
            if (menuPlacer[i].GetCategoryID() == GetCurrentCategoryID()) return i;
        }

        return -1;
    }
}
