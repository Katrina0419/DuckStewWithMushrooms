using UnityEngine;


public class PreventSleep : MonoBehaviour
{
    void Start()
    {
        // ��ֹ�豸��������״̬
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    void OnDestroy()
    {
        // �ָ�Ĭ�����ã������豸����
        Screen.sleepTimeout = SleepTimeout.SystemSetting;
    }
}
