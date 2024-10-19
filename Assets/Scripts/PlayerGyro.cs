using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using DG.Tweening;
using System;
using Unity.VisualScripting;

//��ҿ���
public class PlayerGyro : MonoBehaviour
{
    Rigidbody2D rb;
    float dirX, dirY;
    public float moveSpeed;

    public GameObject body, head;

    public Vector2 playerTF;

    //����ú����
    float followCount;

    //���������
    public GameObject duckRoom;

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
            dirX = Input.acceleration.x * moveSpeed;
            dirY = Input.acceleration.y * moveSpeed;
        }
        else
        {
        }

        if(transform.position.x <= duckRoom.transform.position.x) //��ͷ��������
        {
            float b = LinearInterpolation(transform.position.x, playerTF.x, duckRoom.transform.position.x, 1, 0);
            duckRoom.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, b);
        }
    }

    [System.Obsolete]
    private void FixedUpdate()
    {
        if (rb.velocity.y < -20) //��
        {
            rb.velocity = new Vector2(dirX, dirY * 0.5f);
        }
        else //��
        {
            rb.velocity = new Vector2(dirX * 1.5f, dirY * 1.5f);
        }

        if (rb.velocity.x < -20) //��
        {
            transform.DOScaleX(-1f, 1f);

            body.transform.rotation = Quaternion.EulerRotation(0, 0, -Input.acceleration.y * 0.7f);
            head.transform.rotation = Quaternion.EulerRotation(0, 0, -Mathf.Clamp(Input.acceleration.y, -40f, 10f));
        }
        else //��
        {
            transform.DOScaleX(1f, 1f);

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


    /// <summary>
    /// ���Բ�ֵ������a����������bֵ��
    /// </summary>
    public virtual float LinearInterpolation(float a, float startA, float endA, float startB, float endB)
    {
        float b = startB + (a - startA) / (endA - startA) * (endB - startB);
        return b;
    }
}
