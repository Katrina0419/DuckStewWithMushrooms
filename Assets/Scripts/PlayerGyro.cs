using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using DG.Tweening;
using System;
using Unity.VisualScripting;
using Cinemachine;

//��ҿ���
public class PlayerGyro : MonoBehaviour
{
    Rigidbody2D rb;
    float dirX, dirY;
    public float moveSpeed;

    public GameObject body, head, fishes;

    public Vector2 playerTF;

    //����ú����
    float followCount;

    //��ȡ������
    public GameObject duckRoom;

    //�������
    public CinemachineVirtualCamera CVCame;

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

        if(transform.position.x <= duckRoom.transform.position.x) 
        {
            //��ͷ��������
            float b = LinearInterpolation(transform.position.x, playerTF.x, duckRoom.transform.position.x, 1, 0);
            duckRoom.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, b);

            //��ͷ����
            float d = LinearInterpolation(transform.position.x, playerTF.x, duckRoom.transform.position.x, 130, 140);
            CVCame.m_Lens.FieldOfView = d;
        }
    }

    [System.Obsolete]
    private void FixedUpdate()
    {
        if (dirY < -20) //��
        {
            rb.velocityY = dirY;
        }
        else //��
        {
            rb.velocityY = dirY * 1.2f;
        }

        if (dirX < -20) //��
        {
            rb.velocityX = dirX * 1.7f;
            transform.DOScaleX(-1f, 0f);

            transform.rotation = Quaternion.EulerRotation(0, 0, -Input.acceleration.y * 0.7f);
            //body.transform.rotation = Quaternion.EulerRotation(0, 0, -Input.acceleration.y * 0.7f);
            //head.transform.rotation = Quaternion.EulerRotation(0, 0, -Mathf.Clamp(Input.acceleration.y, -40f, 10f));
        }
        else //��
        {
            rb.velocityX = dirX * 1.7f;
            transform.DOScaleX(1f, 0f);

            transform.rotation = Quaternion.EulerRotation(0, 0, Input.acceleration.y * 0.7f);
            //body.transform.rotation = Quaternion.EulerRotation(0, 0, Input.acceleration.y * 0.7f);
            //head.transform.rotation = Quaternion.EulerRotation(0, 0, Mathf.Clamp(Input.acceleration.y, -40f, 10f));
        }

        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Fish") //����ӵ���
        {
            collision.gameObject.SetActive(false);

            var obj = Instantiate(Resources.Load("Follow"), fishes.transform.position, Quaternion.Euler(0, 0, 0), fishes.transform);
            //obj.GetComponent<NavFollowAi>().player = this.gameObject;
            obj.GetComponent<AudioSource>().Play();

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

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "CameraColl") //����ͼ����
        {
            transform.position = playerTF;
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
