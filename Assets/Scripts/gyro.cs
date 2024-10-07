using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;
using Input = UnityEngine.Input;

public class gyro : MonoBehaviour
{
    bool gyroEnabled;
    Gyroscope theGyro;
    Quaternion qua;

    public GameObject cameraContainer;

    private void Start()
    {
        cameraContainer = transform.parent.gameObject;
        cameraContainer.transform.position = transform.position;
        transform.SetParent(cameraContainer.transform);

        gyroEnabled = GyroEnabled();
    }

    bool GyroEnabled()
    {
        if(SystemInfo.supportsGyroscope)
        {
            theGyro = Input.gyro;
            theGyro.enabled = true;

            cameraContainer.transform.rotation = Quaternion.Euler(90, -90, 0);
            qua = new Quaternion(0, 0, 1, 0);

            return true;
        }
        return false;
    }

    private void FixedUpdate()
    {
        if(gyroEnabled)
        {
            transform.localRotation = theGyro.attitude * qua;

            //Debug.LogError(theGyro.attitude);
            //transform.Rotate(new Vector3(0, Time.deltaTime * 1000, 0));
        }
    }

    private void OnApplicationQuit()
    {
        theGyro.enabled = false;
    }
}
