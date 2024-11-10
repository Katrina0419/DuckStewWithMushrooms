using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//µØÍ¼¸úËæÐý×ª
public class MapMove : MonoBehaviour
{
    public Transform playerTF;
    public Transform mapTF;

    Vector3 mapStartTF;

    private void Start()
    {
        mapStartTF = mapTF.position;
    }

    private void FixedUpdate()
    {
        transform.position = playerTF.position;
        mapTF.position = new Vector3(mapStartTF.x - transform.position.x, mapStartTF.y + transform.position.y, 0);
    }

}
