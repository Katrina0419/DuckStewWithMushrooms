using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using DG.Tweening;
using System;
using Unity.VisualScripting;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

//��ҿ���
public class PlayerGyro : MonoBehaviour
{
    Rigidbody2D rb;
    float dirX, dirY;
    float moveSpeed = 100f;

    public GameObject body, head;

    public Vector2 playerTF;

    //����ú����
    float followCount;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerTF = transform.position;
    }

    [System.Obsolete]
    private void Update()
    {
        if (SystemInfo.supportsGyroscope)
        {
            dirX = Input.acceleration.x * moveSpeed * 1.5f;
            dirY = Input.acceleration.y * moveSpeed;
        }
        else
        {
            //Vector2 move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            //Vector2 pos = rb.position;
            //pos += 5 * Time.deltaTime * move;
            //rb.MovePosition(pos);
        }
    }

    [System.Obsolete]
    private void FixedUpdate()
    {
        if (rb.velocity.y < -10) //��
        {
            rb.velocity = new Vector2(dirX, dirY * 0.8f);
        }
        else //��
        {
            rb.velocity = new Vector2(dirX, dirY * 1.2f);
        }

        if (rb.velocity.x < -10) //��
        {
            //body.GetComponent<SpriteRenderer>().flipX = true;
            //head.GetComponent<SpriteRenderer>().flipX = true;

            transform.DOScaleX(-1f, 0.3f);

            body.transform.rotation = Quaternion.EulerRotation(0, 0, -Input.acceleration.y * 0.7f);
            head.transform.rotation = Quaternion.EulerRotation(0, 0, -Mathf.Clamp(Input.acceleration.y, -40f, 10f));
        }
        else //��
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
        if (collision.gameObject.name == "Circle") //������
        {
            collision.gameObject.SetActive(false);

            var obj = Instantiate(Resources.Load("Follow"), transform.position, Quaternion.Euler(0, 0, 0));
            obj.GetComponent<NavFollowAi>().player = this.gameObject;

            followCount++;
        }
        
        if (collision.gameObject.name == "record") //reset���õ�
        {
            playerTF = collision.gameObject.transform.position;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "JiGuan_0")//��������
        {

        }     
    }

}
