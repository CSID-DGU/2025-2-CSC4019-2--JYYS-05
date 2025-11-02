// Unity
using UnityEngine;

// DOT
using DG.Tweening;

/// <summary>
/// 현재 화면에 떠 있는 윈도우를 제어하기 위한 클래스
/// </summary>
[DisallowMultipleComponent]
public class WindowManager : MonoBehaviour
{

    [SerializeField] private float rotationTime = 0.5f; // 회전 속도
    private Window[] windows;                           // 현재 갖고 있는 자식 창 오브젝트 배열
    private Window currentWindow;                       // 현재 Center 윈도우 컴포넌트

    private bool canRotate;                             // 회전 가능 여부를 판단하기 위한 변수

    [Header("0: Category, 1: Menu")]
    [Range(0,1)]public int windowMode;

    private void Start()
    {
        // 회전 가능 여부 판단 변수 초기화
        canRotate = true;
    }

    /// <summary>
    /// 모든 윈도우를 초기화 하기 위한 메소드
    /// </summary>
    public void InitWindows()
    {
        // 현재 자식 오브젝트에서 모든 Window를 가져옴
        windows = GetComponentsInChildren<Window>();

        // 모든 윈도우의 상태를 비활성화 시킴
        DeactiveAllWindow();

        // 현재 윈도우 초기화
        currentWindow = windows[0];

        // 윈도우의 수만큼 반복
        for (int i = 1; i < windows.Length; i++)
        {
            // 현재 가장 카메라에 가까운 윈도우를 현재 윈도우로 설정
            if (currentWindow.transform.position.z > windows[i].transform.position.z)
            {
                currentWindow = windows[i];
            }
        }

        // 현재 윈도우 활성화
        currentWindow.ActiveCurrentWindow();
    }

    /// <summary>
    /// 모든 윈도우를 비활성화하기 위한 메소드
    /// </summary>
    public void DeactiveAllWindow()
    {
        foreach(Window window in windows)
        {
            window.DeactiveCurrentWindow();
        }
    }

    private void Update()
    {
        // Test Code
        if (Input.GetKeyDown(KeyCode.R)) RotationWindow(-1);
        if (Input.GetKeyDown(KeyCode.L)) RotationWindow(1);
    }

    /// <summary>
    /// 윈도우를 회전시키기 위한 메소드
    /// </summary>
    /// <param name="_direction"></param>
    public void RotationWindow(int _direction)
    {
        if (!canRotate) return; // 회전 중일 경우 Return

        canRotate = false;      // 중복 실행 방지
        DeactiveAllWindow();    // 모든 윈도우 비활성화

        // Y 회전량 계산 (현재 각도에서 추가 각도 및 방향으로 계산됨)
        float targetY = transform.eulerAngles.y + (360f / windows.Length) * _direction;

        // 회전
        transform.DORotate(new Vector3(0, targetY, 0), rotationTime, RotateMode.FastBeyond360)
         .OnComplete(() =>
         {
             // 회전이 완료 될 경우 -> 회전 가능 변수 초기화 및 윈도우 초기화
             canRotate = true;
             InitWindows();
         });
    }

    /// <summary>
    /// 현재 윈도우 컴포넌트를 가져오기 위한 메소드
    /// </summary>
    /// <returns></returns>
    public Window GetCurrentWindow()
    {
        return currentWindow;
    }
}
