// Unity
using UnityEngine;

[DisallowMultipleComponent]
public class WindowGrabber : MonoBehaviour
{
    [Header("장바구니에 담긴 위한 최소 y 좌표")]
    [SerializeField] private float minYAxis = -0.6f;

    [SerializeField] private CartManager cartManager;

    private Camera mainCamera;

    private GameObject grabbedWindow = null;        
    private float grabDistance = 0f;              
    private Vector3 grabOffset;

    private Vector3 lastWindowPosition;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Test Code
        if (Input.GetMouseButton(0)) GrabWindow();
        if (Input.GetMouseButtonUp(0)) DropWindow();
        MoveGrabbedWindow();
    }

    public void GrabWindow()
    {
        if (grabbedWindow == null)
        {
            // Ray : criteria is mouse position
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            // if out is not null
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("Window"))
                {
                    Window window = hit.transform.GetComponent<Window>();
                    
                    if (window.CanGrab())
                    {
                        grabbedWindow = hit.collider.gameObject;
                        lastWindowPosition = grabbedWindow.transform.position;

                        grabDistance = Vector3.Distance(mainCamera.transform.position, hit.point);
                        grabOffset = grabbedWindow.transform.position - hit.point;
                    }
                }
            }
        }
    }

    public void MoveGrabbedWindow()
    {
        if (grabbedWindow != null)
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition); 
            Vector3 targetPoint = ray.GetPoint(grabDistance);
            Vector3 newPos = new Vector3(targetPoint.x + grabOffset.x,
                                          targetPoint.y + grabOffset.y,
                                          lastWindowPosition.z);
            grabbedWindow.transform.position = newPos;    
        }
    }

    public void DropWindow()
    {
        if (grabbedWindow != null)
        {
            // 장바구니에 넣기
            if (grabbedWindow.transform.localPosition.y <= minYAxis)
            {
                Window window = grabbedWindow.transform.GetComponent<Window>();

                if (window != null)
                {
                    // 메뉴 정보 로드
                    string id = window.GetWindowID();
                    string name = window.GetWindowName();
                    int price = window.GetPrice();
                    Sprite sprite = window.GetSprite();

                    // 장바구니 추가
                    cartManager.AddCart(id, name, price, sprite);
                }
            }

            grabbedWindow.transform.position = lastWindowPosition;
        }

        grabbedWindow = null;
    }
}
