// Unity
using UnityEngine;

[DisallowMultipleComponent]
public class GestureManager : MonoBehaviour
{
    [SerializeField] private WindowGrabber windowGrabber;
    [SerializeField] private ViewController sphereController;
    [SerializeField] private UIManager uiManager;

    public void UpdateGestureFromNetwork(string gesture)
    {
        WindowManager windowManager = FindObjectOfType<WindowManager>();

        switch (gesture)
        {
            case "up_swipe":
                sphereController.CategoryToMenu();
                break;
            case "down_swipe":
                sphereController.MenuToCategory();
                break;
            case "left_swipe":
                windowManager.RotationWindow(1);
                break;
            case "right_swipe":
                windowManager.RotationWindow(-1);
                break;
            case "grab": windowGrabber.GrabWindow(); break;
            case "drop": windowGrabber.DropWindow(); break;
            case "start": uiManager.StartToSelect(); break;
            case "disconnect": uiManager.GoStartMenu(); break;
        }
    }
}
