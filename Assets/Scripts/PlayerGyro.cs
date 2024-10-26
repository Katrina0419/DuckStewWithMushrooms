using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using DG.Tweening;
using System;
using Unity.VisualScripting;
using Cinemachine;
using Random = UnityEngine.Random;
using static UnityEditor.Progress;
using UnityEngine.UI;
using Microsoft.Unity.VisualStudio.Editor;

//��ҿ���
public class PlayerGyro : MonoBehaviour
{
    Rigidbody2D rb;
    float dirX, dirY, dirZ;
    public float moveSpeed;

    public GameObject body, head;

    public Vector2 playerTF;

    //����ú����
    public GameObject follow;
    public Transform fishes;
    public float followDistance; // ����֮��ĸ������
    public List<GameObject> items = new List<GameObject>();

    //��ȡ������
    public GameObject duckRoom;

    //�������
    public CinemachineVirtualCamera CVCame;

    //�Ƿ����
    public bool isSnake = false;
    public UnityEngine.UI.Image btnSP;
    public List<Sprite> sp = new List<Sprite>();

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerTF = transform.position;
    }


    [System.Obsolete]
    private void FixedUpdate()
    {
        if (transform.position.x < duckRoom.transform.position.x)
        {
            //��ͷ��������
            float b = LinearInterpolation(transform.position.x, playerTF.x, duckRoom.transform.position.x, 1, 0);
            duckRoom.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, b);

            //��ͷ
            float d = LinearInterpolation(transform.position.x, playerTF.x, duckRoom.transform.position.x, 150, 140);
            CVCame.m_Lens.FieldOfView = d;
        }


        dirX = Input.acceleration.x * moveSpeed;
        dirY = Input.acceleration.y * moveSpeed;
        dirZ = (-Input.acceleration.z - 0.5f) * moveSpeed;

        if (dirZ < 100) //��
        {
            //rb.velocityY = dirZ + 80f;
            rb.velocityY = dirY + 50f;
        }
        else //��
        {
            rb.velocityY = -(dirZ - 50f) * 1.7f;
        }

        if (dirX < -20) //��
        {
            rb.velocityX = dirX * 1.5f;
            transform.DOScaleX(-1f, 0f);

            transform.rotation = Quaternion.EulerRotation(0, 0, -Input.acceleration.y * 0.7f);
            //body.transform.rotation = Quaternion.EulerRotation(0, 0, -Input.acceleration.y * 0.7f);
            //head.transform.rotation = Quaternion.EulerRotation(0, 0, -Mathf.Clamp(Input.acceleration.y, -40f, 10f));
        }
        else //��
        {
            rb.velocityX = dirX * 1.5f;
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

            float angle = Random.Range(0f, 360f);
            float distance = Random.Range(5f, 10f);//��Χ���
            Vector3 randomPosition = new Vector3(fishes.position.x + Mathf.Cos(angle) * distance, fishes.position.y + Mathf.Sin(angle) * distance, 0);
            GameObject item = Instantiate(follow, randomPosition, Quaternion.Euler(0, 0, 0), fishes);

            items.Add(item);
        }
        
        if (collision.gameObject.name == "record") //reset���õ�
        {
            playerTF = collision.gameObject.transform.position;
            btnSP.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "CameraColl") //����ͼ����
        {
            transform.position = playerTF;
        }
    }

    public void isSnakeNow()
    {
        if (isSnake) //����
        {
            btnSP.sprite = sp[1];

            for (int i = 0; i < items.Count; i++)
            {
                items[i].GetComponent<NavFollowAi>().agent.enabled = false;

                float angle = Random.Range(0f, 360f);
                float distance = Random.Range(5f, 10f);
                Vector3 randomPosition = new Vector3(fishes.localPosition.x + Mathf.Cos(angle) * distance, fishes.localPosition.y + Mathf.Sin(angle) * distance, 0);

                items[i].transform.DOLocalMove(randomPosition, 1f);
                items[i].transform.localRotation = Quaternion.EulerAngles(0, 0, 0);
            }
        }
        else //�ж�
        {
            btnSP.sprite = sp[0];

            for (int i = 0; i < items.Count; i++)
            {
                Vector3 offset = new Vector3(-followDistance * (i + 1), 0, 0);
                items[i].transform.DOMove(transform.position + offset, 1f);

                items[i].GetComponent<NavFollowAi>().agent.enabled = true;
                items[0].GetComponent<NavFollowAi>().followOne = this.gameObject; //One By One
                if(i > 0)
                {
                    items[i].GetComponent<NavFollowAi>().followOne = items[i - 1].GetComponent<NavFollowAi>().gameObject;
                }

                items[i].transform.localRotation = Quaternion.EulerAngles(0, 0, 0);
            }
        }

        isSnake = !isSnake;
    }

    /// <summary> ���Բ�ֵ������a����������bֵ��</summary>
    public virtual float LinearInterpolation(float a, float startA, float endA, float startB, float endB)
    {
        float b = startB + (a - startA) / (endA - startA) * (endB - startB);
        return b;
    }
}
