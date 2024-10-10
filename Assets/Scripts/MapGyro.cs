using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGyro : MonoBehaviour
{
    Rigidbody2D rb;
    float dirX;
    float moveSpeed = 10f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        dirX = Input.acceleration.x * moveSpeed;
        //transform.eulerAngles = new Vector3(0, 0, Mathf.Clamp(transform.rotation.z, -90f, 90f));
    }

    private void FixedUpdate()
    {
        rb.MoveRotation(-dirX * 3f);
    }
}
