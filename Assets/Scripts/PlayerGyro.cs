using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGyro : MonoBehaviour
{
    Rigidbody2D rb;
    float dirX, dirY;
    float moveSpeed = 100f;

    public Transform body, head;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        dirX = Input.acceleration.x * moveSpeed * 0.5f;
        dirY = Input.acceleration.y * moveSpeed;
    }

    [System.Obsolete]
    private void FixedUpdate()
    {
        rb.velocity = new Vector2(dirX, dirY);
        head.rotation = Quaternion.EulerAngles(0, 0, Input.acceleration.y);
    }
}
