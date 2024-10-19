using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreventSleep : MonoBehaviour
{
    void Start()
    {
        // 禁止设备进入休眠状态
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    void OnDestroy()
    {
        // 恢复默认设置，允许设备休眠
        Screen.sleepTimeout = SleepTimeout.SystemSetting;
    }
}
