using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//机关管理
public class Organ : MonoBehaviour
{
    public Transform map;

    public GameObject jiGuanCheng, jiGuangQiang;

    private void Update()
    {
        transform.localPosition = map.localPosition;
    }
}
