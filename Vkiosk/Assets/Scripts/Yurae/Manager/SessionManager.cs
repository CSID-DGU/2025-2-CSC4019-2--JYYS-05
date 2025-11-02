// Unity
using UnityEngine;
using TMPro;

public class SessionManager : MonoBehaviour
{
    [Header("Session 유지 시간")]
    [SerializeField] private float sessionTime;
    [SerializeField] private TextMeshProUGUI sessionTMP;
    [SerializeField] private UIManager uiManager;
    private float curTime;

    private bool startSession;
    private bool isStop;

    private void Start()
    {
        startSession = false;
        isStop = false;
    }

    public void StartSession()
    {
        startSession = true;
        curTime = sessionTime;
    }

    public void ExitSession()
    {
        startSession = false;
    }

    public void StopSession()
    {
        isStop = true;
    }

    public void ResumeSession()
    {
        isStop = false;
    }

    public void Update()
    {
        if (isStop) return;

        if (startSession == true)
        {
            if (curTime >= 0)
            {
                curTime -= Time.deltaTime;
                sessionTMP.text = Mathf.FloorToInt(curTime).ToString() + "초";
            }
            else
            {
                ExitSession();
                uiManager.GoStartMenu();
            }
        }
    }


}
