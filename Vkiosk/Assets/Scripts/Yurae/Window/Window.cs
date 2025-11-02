// Unity
using UnityEngine;
using UnityEngine.Rendering;

[DisallowMultipleComponent]
public class Window : MonoBehaviour
{
    [Header("Materials")]
    [SerializeField] private Material selectedMaterial;
    [SerializeField] private Material defaultMaterial;

    private Renderer thisRenderer;
    private bool canGrab = false;

    private Quaternion initRotation;
   
    private string windowID;
    private string windowName;
    private int price;
    private Sprite sprite;

    private void Awake()
    {
        thisRenderer = GetComponent<Renderer>();
        initRotation = transform.rotation;
    }

    private void LateUpdate()
    {
        // 회전 값 고정 (부모 회전 값에 관여 받지 않음)
        transform.rotation = initRotation;
    }

    /// <summary>
    /// 윈도우 활성화 메소드
    /// </summary>
    public void ActiveCurrentWindow()
    {
        if(transform.parent.GetComponent<WindowManager>().windowMode == 1) canGrab = true;
        thisRenderer.material = selectedMaterial;
    }

    /// <summary>
    /// 윈도우 비활성화 메소드
    /// </summary>
    public void DeactiveCurrentWindow()
    {
        canGrab = false;
        thisRenderer.material = defaultMaterial;
    }

    /// <summary>
    /// 그랩 가능 여부 판단 메소드
    /// </summary>
    /// <returns></returns>
    public bool CanGrab()
    {
        return canGrab;
    }

    /// <summary>
    /// 윈도우 ID 설정 메소드
    /// </summary>
    /// <param name="_ID"></param>
    public void SetWindow(string _id, string _name, int _price, Sprite _sprite)
    {
        windowID = _id;
        windowName = _name;
        price = _price;
        sprite = _sprite;
    }

    /// <summary>
    /// 윈도우 ID를 가져오기 위한 메소드
    /// </summary>
    /// <returns></returns>
    public string GetWindowID()
    {
        return windowID;
    }

    public string GetWindowName()
    {
        return windowName;
    }

    public int GetPrice()
    {
        return price;
    }

    public Sprite GetSprite()
    {
        return sprite;
    }
}
