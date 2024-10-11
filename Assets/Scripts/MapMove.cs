using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMove : MonoBehaviour
{
    public Transform playerTF;
    public Transform mapTF;

    Transform mapStartTF;

    private void Awake()
    {
        mapStartTF.position = mapTF.position;
    }

    private void FixedUpdate()
    {
        transform.position = playerTF.position;
        mapTF.position = new Vector3(mapStartTF.position.x - transform.position.x, mapStartTF.position.y + transform.position.y, 0);
    }

}
