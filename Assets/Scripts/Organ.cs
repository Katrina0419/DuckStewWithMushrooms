using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���ع���
public class Organ : MonoBehaviour
{
    public Transform map;

    public GameObject jiGuanCheng, jiGuangQiang;

    private void Update()
    {
        transform.localPosition = map.localPosition;
    }
}
