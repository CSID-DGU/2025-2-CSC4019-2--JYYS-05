// Unity
using UnityEngine;

/// <summary>
/// 윈도우 배치를 위한 추상 클래스 (IWindowPlacer Implement)
/// </summary>
public abstract class WindowPlacer : MonoBehaviour
{
    [SerializeField] private WindowManager windowManager;
    [SerializeField] private float offSet;

    /// <summary>
    /// 윈도우 배치 가상 메소드
    /// </summary>
    /// <param name="_windowCount"></param>
    /// <param name="_prefab"></param>
    public void PlaceWindow(int _windowCount, GameObject _prefab)
    {
        Transform reference = Camera.main.transform;
        Vector3 baseDir = reference.forward;
        baseDir.y = 0;
        baseDir.Normalize();

        for (int i = 0; i < _windowCount; i++)
        {
            float angle = 360f / _windowCount * i;

            Vector3 dir = Quaternion.Euler(0, angle, 0) * (-baseDir);

            Vector3 position = transform.position + dir * offSet;

            GameObject window = Instantiate(_prefab, position, Quaternion.identity);
            Init(window, i);

            window.transform.rotation = Quaternion.Euler(0, 0, 0);
            window.transform.SetParent(transform);
        }
        windowManager.InitWindows();
    }

    public virtual void Init(GameObject _window, int _index) { }
}

    

