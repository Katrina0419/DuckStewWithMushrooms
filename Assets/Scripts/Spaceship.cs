using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceship : MonoBehaviour
{
    Rigidbody2D rb;
    float dirX;
    float moveSpeed = 20f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        dirX = Input.acceleration.x * moveSpeed;
        //transform.position = new Vector2(Mathf.Clamp(transform.position.x, -7.5f, 7.5f), transform.position.y);
        //transform.eulerAngles = new Vector3(0, 0, Mathf.Clamp(transform.rotation.z, -90f, 90f));
    }

    private void FixedUpdate()
    {
        rb.MoveRotation(dirX * 5f);
    }
}
