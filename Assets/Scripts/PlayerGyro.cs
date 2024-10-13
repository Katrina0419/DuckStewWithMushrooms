using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using DG.Tweening;
using System;
using Unity.VisualScripting;

public class PlayerGyro : MonoBehaviour
{
    Rigidbody2D rb;
    float dirX, dirY;
    float moveSpeed = 100f;

    public GameObject body, head;

    public List<GameObject> follow;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    [System.Obsolete]
    private void Update()
    {
        dirX = Input.acceleration.x * moveSpeed * 1.5f;
        dirY = Input.acceleration.y * moveSpeed;
    }

    [System.Obsolete]
    private void FixedUpdate()
    {
        if(rb.velocity.y <- 10) //ÏÂ
        {
            rb.velocity = new Vector2(dirX, dirY * 0.8f);
        }
        else //ÉÏ
        {
            rb.velocity = new Vector2(dirX, dirY * 1.2f);
        }

        if(rb.velocity.x < -10) //×ó
        {
            //body.GetComponent<SpriteRenderer>().flipX = true;
            //head.GetComponent<SpriteRenderer>().flipX = true;

            transform.DOScaleX(-1f, 0.3f);

            body.transform.rotation = Quaternion.EulerRotation(0, 0, -Input.acceleration.y * 0.7f);
            head.transform.rotation = Quaternion.EulerRotation(0, 0, -Mathf.Clamp(Input.acceleration.y, -40f, 10f));
        }
        else //×ó
        {
            //body.GetComponent<SpriteRenderer>().flipX = false;
            //head.GetComponent<SpriteRenderer>().flipX = false;

            transform.DOScaleX(1f, 0.3f);

            body.transform.rotation = Quaternion.EulerRotation(0, 0, Input.acceleration.y * 0.7f);
            head.transform.rotation = Quaternion.EulerRotation(0, 0, Mathf.Clamp(Input.acceleration.y, -40f, 10f));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Circle")
        {
            Destroy(collision.gameObject);

            var obj = Instantiate(Resources.Load("Follow"), transform.position, Quaternion.Euler(0, 0, 0));
            obj.GetComponent<NavFollowAi>().player = this.gameObject;
        }
    }
}
